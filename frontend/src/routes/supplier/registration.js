const _ = require('lodash')
const router = require('express').Router({ strict: true, mergeParams: true })
const { checkSchema, validationResult } = require('express-validator/check')
const { matchedData } = require('express-validator/filter')
const { dataProvider } = require('catalogue-data')

const registrationPageValidation = checkSchema(require('./registration-validation').registration)
const capabilitiesPageValidation = checkSchema(require('./registration-validation').capabilities)

// all routes in this module require CSRF protection
router.use(require('csurf')())

// registering a new solution is the same basic flow as editing an existing one
// except that there is no solution to load

router
  .route('/new/')
  .get(onboardingStatusPage)

router
  .route('/new/register/')
  .get(registrationPageGet)
  .post(registrationPreValidation, registrationPageValidation, registrationPagePost)

// all the remaining routes need to load a specified solution
router.param('solution_id', async (req, res, next, solutionId) => {
  try {
    req.solution = await dataProvider.solutionForRegistration(solutionId)
    next()
  } catch (err) {
    next(err)
  }
})

router
  .route('/:solution_id/')
  .get(onboardingStatusPage)

router
  .route('/:solution_id/register/')
  .get(registrationPageGet)
  .post(registrationPreValidation, registrationPageValidation, registrationPagePost)

router
  .route('/:solution_id/capabilities/')
  .get(capabilitiesPageGet)
  .post(capabilitiesPageValidation, capabilitiesPagePost)

function commonOnboardingContext (req) {
  return {
    solution: req.solution,
    csrfToken: req.csrfToken(),
    activeForm: {
      title: req.solution && _([req.solution.name, req.solution.version]).filter().join(', ')
    }
  }
}

function onboardingStatusPage (req, res) {
  const context = {
    ...commonOnboardingContext(req),
    breadcrumbs: [
      { label: 'MySolutions.Title', url: '../../' },
      { label: 'Onboarding.Title' }
    ],
    stages: [
      {
        url: 'register#content'
      },
      {
        class: 'unavailable'
      },
      {
        class: 'unavailable'
      },
      // solution page will be unavailable for the time being
      {
        class: 'unavailable'
      }
    ]
  }

  if (req.solution) {
    const status = +req.solution.status

    context.stages[1].status = 'Not started'
    context.stages[1].link = 'Start'
    context.stages[1].class = 'unavailable'
    context.stages[1].url = `../../capabilities/${req.solution.id}`

    context.stages[2].status = 'Not started'
    context.stages[2].link = 'Start'
    context.stages[2].class = 'unavailable'
    context.stages[2].url = `../../compliance/${req.solution.id}`

    // Capability Assessment has some evidence
    if (req.solution._raw.claimedCapabilityEvidence.length) {
      context.stages[1].status = 'Draft Saved'
      context.stages[1].link = 'Edit'
    }

    // Standards Compliance has some evidence
    if (req.solution._raw.claimedStandardEvidence.length) {
      context.stages[2].status = 'Draft Saved'
      context.stages[2].link = 'Edit'
    }

    // Standards Compliance has a none draft standard
    if (req.solution._raw.claimedStandard.some((std) => +std.status !== 0)) {
      context.stages[2].status = 'In Progress'
      context.stages[2].link = 'Edit'
    }

    if (status === 0) { // draft
      context.stages[0].status = 'Draft Saved'
      context.stages[0].link = 'Edit'
      context.stages[1].link = ''
      context.stages[2].link = ''
    }

    if (status === 1) { // registered
      context.stages[0].status = 'Complete'
      context.stages[0].class = 'complete'
      context.stages[0].link = 'Edit'
      context.stages[1].class = ''
      context.stages[2].class = ''

      if ('registered' in req.query) {
        context.registrationComplete = true
      }
    }
    if (status === 2) { // capability assessment
      context.stages[0].status = 'Complete'
      context.stages[0].class = 'complete'
      context.stages[0].link = 'View'
    }
    if (status === 3) { // Standards Compliance
      context.stages[0].status = 'Complete'
      context.stages[0].class = 'complete'
      context.stages[0].link = 'View'
    }

    if (status === 2 || status === 3) {
      if ('capabilityAssessmentSubmitted' in req.query) {
        context.capabilityAssessmentSubmitted = true
      }
      // if all capabilities have passed, then display complete
      if (req.solution._raw.claimedCapability.every((cap) => +cap.status === 3)) {
        context.stages[1].status = 'Complete'
        context.stages[1].class = 'complete'
        context.stages[1].link = 'View'
      } else {
        context.stages[1].status = 'Awaiting outcome'
        context.stages[1].link = 'View'
      }

      // if all standards have passed, then display complete
      if (req.solution._raw.claimedStandard.every((std) => +std.status === 4)) {
        context.stages[2].status = 'Complete'
        context.stages[2].class = 'complete'
        context.stages[2].link = 'View'
      }
    }
  } else {
    context.stages[0].status = 'Not started'
    context.stages[0].link = 'Start'
  }

  res.render('supplier/registration/index', context)
}

function registrationPageContext (req) {
  const context = {
    ...commonOnboardingContext(req),
    breadcrumbs: [
      { label: 'Onboarding.Title', url: '../' },
      { label: 'Onboarding.Details.Breadcrumb' }
    ]
  }

  context.activeForm.id = 'registration-form'

  if (!context.solution) {
    context.solution = { id: 'new' }
  } else {
    switch (+context.solution.status) {
      case 0: // draft - all fields editable
        break
      case 1: // registered - name and version is not editable
        context.nameReadOnly = true
        context.versionReadOnly = true
        break
      default: // any other status - no fields editable
        context.versionReadOnly = true
        context.nameReadOnly = true
        context.readOnly = true
    }
  }

  return context
}

// Handlebars templates can't do string synthesis and that is needed to lookup
// the name of a field in the errors.controls array. Instead, pass a dictionary for the
// contacts that yields the control names.
function addContactFieldsToContext (context) {
  context.contactFields = _.map(context.solution.contacts,
    (c, i) => _(['contactType', 'firstName', 'lastName', 'emailAddress', 'phoneNumber'])
      .map(f => [f, `solution.contacts[${i}].${f}`])
      .fromPairs()
      .value()
  )
}

function registrationPageGet (req, res) {
  const context = {
    ...registrationPageContext(req)
  }

  if ('saved' in req.query) {
    context.solutionSaved = true
  }

  addContactFieldsToContext(context)

  /* If Solution has progressed further than registering. */
  if (+context.solution.status && +context.solution.status !== 0 && +context.solution.status !== 1) {
    res.redirect(`../../../capabilities/${req.solution.id}/summary`)
  } else {
    res.render('supplier/registration/1-details', context)
  }
}

// before attempting to validate the body for registration,
// remove any contacts that are entirely empty
function registrationPreValidation (req, res, next) {
  if (req.body && req.body.solution && req.body.solution.contacts) {
    req.body.solution.contacts = _.filter(
      req.body.solution.contacts,
      c => `${c.contactType}${c.firstName}${c.lastName}${c.emailAddress}${c.phoneNumber}`.trim().length
    )
  }

  next()
}

async function registrationPagePost (req, res) {
  const sanitisedInput = matchedData(req, {
    locations: 'body',
    includeOptionals: true,
    onlyValidData: false
  })
  const context = _.merge({
    ...registrationPageContext(req)
  }, sanitisedInput)
  context.solution.contacts = sanitisedInput.solution.contacts

  addContactFieldsToContext(context)

  const valres = validationResult(req)
  if (!valres.isEmpty()) {
    context.errors = {
      items: valres.array({ onlyFirstError: true }),
      controls: _.mapValues(valres.mapped(), res => ({
        ...res,
        action: res.msg + 'Action'
      }))
    }
    context.errors.fieldsets = {
      'NameDescVersion': 'solution.name' in context.errors.controls ||
        'solution.description' in context.errors.controls ||
        'solution.version' in context.errors.controls,
      // FIXME the following opaque monstrosity yields an object keyed by the index
      // of any contact that has validation errors (_.toPath being ideal here)
      'Contacts': _(context.errors.controls)
        .keys().map(_.toPath).filter(p => p[0] === 'solution' && p[1] === 'contacts')
        .map(p => p[2]).uniq().map(k => [k, true]).fromPairs().value()
    }
  } else if (!context.readOnly) {
    try {
      if (context.solution.id === 'new') {
        req.solution = await dataProvider.createSolutionForRegistration({
          name: context.solution.name,
          description: context.solution.description,
          version: context.solution.version
        }, req.user)
      } else {
        req.solution.name = context.solution.name
        req.solution.description = context.solution.description
        req.solution.version = context.solution.version
      }

      req.solution.contacts = context.solution.contacts

      await dataProvider.updateSolutionForRegistration(req.solution)
    } catch (err) {
      context.errors = {
        items: [{ msg: err.toString() }]
      }
    }
  }

  if (context.errors) {
    return res.render('supplier/registration/1-details', context)
  }

  // redirect based on action chosen and whether a new solution was just created
  let redirectUrl = context.solution.id === 'new'
    ? `../../${req.solution.id}/register/`
    : './'

  if (req.body.action) {
    if (req.body.action.continue) redirectUrl += '../capabilities/'
    else if (req.body.action.exit) redirectUrl = '/'
    else if (req.body.action.save) redirectUrl += '?saved'
  }

  res.redirect(redirectUrl)
}

async function capabilitiesPageContext (req) {
  const context = {
    ...commonOnboardingContext(req),
    ...await dataProvider.capabilityMappings(),
    breadcrumbs: [
      { label: 'Onboarding.Title', url: '../' },
      { label: 'Onboarding.Capabilities.Breadcrumb' }
    ]
  }

  if (+context.solution.status && +context.solution.status !== 0 && +context.solution.status !== 1) {
    context.readOnly = true
  }

  context.activeForm.id = 'capability-selector-form'

  context.capabilities = _(context.capabilities)
    .values()
    .orderBy(a => a.name.toLowerCase())
    .map(c => ({
      ...c,
      standardIds: _(c.standards)
        .reject(c => context.standards[c.id].isOverarching)
        .map('id')
        .value()
    }))
    .value()

  context.capabilitiesByGroup = _.zipObject(
    ['core', 'noncore'],
    _.partition(context.capabilities, c => c.type === 'C')
  )

  context.standardsByGroup = _.mapValues(
    _.zipObject(
      ['overarching', 'associated'],
      _.partition(context.standards, 'isOverarching')
    ),
    stds => _.orderBy(stds, a => a.name.toLowerCase())
  )
  // console.log(context.standardsByGroup.associated)

  return context
}

async function capabilitiesPageGet (req, res) {
  const context = await capabilitiesPageContext(req)

  if ('saved' in req.query) {
    context.solutionSaved = true
  }

  context.capabilities.forEach(cap => {
    cap.selected = _.get(_.find(req.solution.capabilities, { capabilityId: cap.id }), 'id')
  })

  /* If Solution has progressed further than registration. */
  if (+context.solution.status && +context.solution.status !== 0 && +context.solution.status !== 1) {
    res.redirect(`../../../capabilities/${req.solution.id}/summary`)
  } else {
    res.render('supplier/registration/2-capabilities', context)
  }
}

async function capabilitiesPagePost (req, res) {
  const context = await capabilitiesPageContext(req)

  // redirect based on action chosen
  let redirectUrl = '../'

  if (req.body.action) {
    if (req.body.action.save) redirectUrl = './?saved'
    else if (req.body.action.exit) redirectUrl = '/'
  }

  // the "selected" property holds the current ID for each claimed capability,
  // or a newly generated ID for an added capability
  context.capabilities.forEach(cap => {
    cap.selected = _.has(req.body.capabilities, cap.id) &&
      (req.body.capabilities[cap.id] || require('node-uuid-generator').generate())
  })

  const valres = validationResult(req)
  if (!valres.isEmpty() && req.body.action.continue) {
    context.errors = {
      items: valres.array({ onlyFirstError: true }),
      controls: valres.mapped()
    }
  } else if (!context.readOnly) {
    req.solution.capabilities = _(context.capabilities)
      .filter('selected')
      .map(cap => ({
        id: cap.selected,
        capabilityId: cap.id,
        status: '0',
        solutionId: req.solution.id
      }))
      .value()

    req.solution.standards = _(req.solution.capabilities)
      .flatMap(({ capabilityId }) => _.find(context.capabilities, { id: capabilityId }).standards)
      .uniqBy('id')
      .map(std => ({
        standardId: std.id,
        status: '0',
        solutionId: req.solution.id,
        ownerId: null
      }))
      .value()

    if (req.body.action && req.body.action.continue) {
      if (+req.solution.status === 0) {
        req.solution.status = '1'
        redirectUrl += '?registered'
      }
    }

    try {
      await dataProvider.updateSolutionForRegistration(req.solution)
    } catch (err) {
      context.errors = {
        items: [{ msg: err.toString() }]
      }
    }
  }

  if (context.errors) {
    res.render('supplier/registration/2-capabilities', context)
  } else {
    res.redirect(redirectUrl)
  }
}

module.exports = router
