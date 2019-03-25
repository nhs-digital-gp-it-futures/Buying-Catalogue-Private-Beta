const _ = require('lodash')
const router = require('express').Router({ strict: true, mergeParams: true })
const path = require('path')

const { dataProvider } = require('catalogue-data')
const { sharePointProvider } = require('catalogue-sharepoint')
const { catchHandler } = require('catalogue-authn-authz')

const multer = require('multer')
const storage = multer.memoryStorage()
const upload = multer({ storage: storage }).any()

// Multer is required to be used before CSRF for multi-part form parsing
router.use(upload)

// all routes in this module require CSRF protection
router.use(require('csurf')())

// all routes need to load a specified solution
router.param('solution_id', async (req, res, next, solutionId) => {
  try {
    req.solution = await dataProvider.solutionForCompliance(solutionId)
    next()
  } catch (err) {
    next(err)
  }
})

router
  .route('/:solution_id/')
  .get(solutionComplianceDashboard)

router
  .route('/:solution_id/evidence/:claim_id/')
  .get(solutionComplianceEvidencePageGet)

router
  .route('/:solution_id/evidence/:claim_id/')
  .post(solutionComplianceEvidencePagePost)

router
  .route('/:solution_id/evidence/:claim_id/confirmation')
  .get(solutionComplianceEvidenceConfirmationGet)

router
  .route('/:solution_id/evidence/:claim_id/:file_name')
  .get(downloadEvidenceGet)

router
  .route('/:solution_id/evidence/:claim_id/confirmation')
  .post(solutionComplianceEvidenceConfirmationPost)

function solutionInRegistrationComplete (solution) {
  return +solution.status === 1
}

function solutionInCompliance (solution) {
  return +solution.status === 3
}

function commonComplianceContext (req) {
  return {
    solution: req.solution,
    csrfToken: req.csrfToken(),
    activeForm: {
      title: req.solution && _([req.solution.name, req.solution.version]).filter().join(', ')
    }
  }
}

async function dashboardContext (req) {
  return {
    ...commonComplianceContext(req),
    ...await dataProvider.capabilityMappings()
  }
}

async function solutionComplianceDashboard (req, res) {
  let context = {
    breadcrumbs: [
      { label: 'Onboarding.Title', url: `../../solutions/${req.solution.id}` },
      { label: 'CompliancePages.Dashboard.Title' }
    ],
    errors: {},
    backlink: `../../solutions/${req.solution.id}`
  }

  try {
    const [contextData, evidenceFiles] = await Promise.all([
      dashboardContext(req),
      sharePointProvider.enumerateStdFolderFiles(req.solution.id)
    ])

    context = {
      ...context,
      ...contextData
    }

    const notReadyStatus = {
      statusClass: 'not-ready',
      statusTransKey: 'Statuses.Standard.NotReady',
      withContact: {
        displayName: 'NHS Digital'
      }
    }

    if (evidenceFiles) {
      context.solution.standards = _.sortBy(_(context.solution.standards)
        .map(std => ({
          ...context.standards[std.standardId],
          ...std,
          continueUrl: `evidence/${std.id}/`,
          isInterop: context.standards[std.standardId].type === 'I',
          ...!_.isEmpty(evidenceFiles[std.id]) || +std.status !== 0
            ? {}
            : notReadyStatus
        }))
        .value()
      , 'name')
    } else {
      // no evidence files object was obtained.
      context.solution.standards = []
      context.errors = { items: [{ msg: 'CompliancePages.Dashboard.Error.NoEnumeration' }] }
    }

    if ('submitted' in req.query) {
      const submittedStandard = context.solution.standards.find((std) => std.standardId === req.query.submitted)
      if (submittedStandard && +submittedStandard.status === 2) {
        context.submittedStandard = submittedStandard.name
      }
    }
  } catch (err) {
    catchHandler(err, res, context)
  }

  res.render('supplier/compliance/index', context)
}

function formatTimestampForDisplay (ts) {
  const timestamp = new Date(ts)
  return timestamp.toLocaleDateString('en-gb', {
    timeZone: 'Europe/London',
    day: 'numeric',
    month: 'long',
    year: 'numeric'
  }) + ' ' + timestamp.toLocaleTimeString('en-gb', {
    timeZone: 'Europe/London',
    hour: 'numeric',
    minute: '2-digit',
    hour12: false
  })
}

async function evidencePageContext (req, next) {
  const context = {
    ...commonComplianceContext(req),
    ...await dataProvider.capabilityMappings()
  }

  context.claim = _.find(context.solution.standards, { id: req.params.claim_id })

  if (!context.claim) {
    let err = new Error('Claim Not Found')
    return next(err)
  }

  context.claim.standard = context.standards[context.claim.standardId]

  context.claim.capabilities = _.filter(context.capabilities,
    cap => _.some(context.solution.capabilities, { capabilityId: cap.id }) &&
           _.some(cap.standards, { id: context.claim.standardId })
  )

  // compute the message history from the relevant evidence and reviews
  context.claim.submissionHistory = _(context.solution.evidence)
    .filter({ claimId: context.claim.id })
    .map((ev) => ({ ...ev, isFeedback: false })) // Flag all Evidence as not being Feedback
    .flatMap(ev => [
      ev,
      ..._
        .filter(context.solution.reviews, { evidenceId: ev.id })
        .map((re) => ({ ...re, isFeedback: true })) // Flag all Reviews as being Feedback
    ])
    .map(({ id, originalDate, createdById, message, evidence, isFeedback }) => ({
      id,
      createdOn: originalDate,
      createdById,
      message: message || evidence,
      isFeedback
    }))
    .orderBy('createdOn', 'asc')
    .each(msg => {
      msg.createdOn = formatTimestampForDisplay(msg.createdOn)
    })

  // set flags based on who sent the last message
  if (context.claim.submissionHistory.length) {
    const latestEntry = _.last(context.claim.submissionHistory)
    context.hasFeedback = latestEntry.isFeedback
    context.feedbackId = context.hasFeedback && latestEntry.id
  }

  context.isSubmitted = +context.claim.status === 2

  // load the contacts associated with the message history
  context.claim.historyContacts = _.keyBy(await Promise.all(
    _(context.claim.submissionHistory)
      .filter((msg) => !msg.isFeedback)
      .uniqBy('createdById') 
      .map('createdById')
      .map(id => dataProvider.contactById(id))
  ), 'id')

  // prepare list of candidate owners for dropdown
  context.candidateOwners = _(context.solution.candidateOwners)
    .map(owner => ({
      ...owner,
      current: owner.id === context.claim.ownerId
    }))
    // Lead contact is always first, followed by the other users in firstName order
    .orderBy(['contactType', 'firstName', 'lastName'])
    .value()

  context.errors = req.body.errors || []

  try {
    context.files = await fetchFiles(context.claim.id)
  } catch (err) {
    context.errors.push(err)
  }

  if ('saved' in req.query) {
    context.solutionSaved = true
  }

  context.breadcrumbs = [
    { label: 'Onboarding.Title', url: `../../../../solutions/${req.solution.id}` },
    { label: 'CompliancePages.Dashboard.Breadcrumb', url: `../../` },
    { label: context.claim.standard.name }
  ]

  context.latestReview = _(context.claim.submissionHistory)
  .filter((msg) => msg.isFeedback)
  .last()

  let latestFile

  if (context.files) {
    latestFile = findLatestFile(context.files.items)
  }

  if (latestFile) {
    latestFile.downloadURL = path.join(req.baseUrl, req.path, latestFile.name)

    context.latestFile = latestFile
    context.assessmentIncomplete = solutionInRegistrationComplete(context.solution)
    context.standardNotStarted = +context.claim.status === 0
    // only allow a submission if a file exists and;
    // a) evidence has not already been submitted, or
    // b) the latest submission is feedback from NHS Digital
    // c) the solution is in the standards compliance stage.
    context.allowSubmission = (!context.isSubmitted || context.hasFeedback) && solutionInCompliance(context.solution)
    if (context.allowSubmission) {
      context.activeForm.id = 'compliance-evidence-upload'
      context.allowDirectSubmission = context.claim.submissionHistory.length && !context.isSubmitted && !context.hasFeedback
    }
  }

  return context
}

async function solutionComplianceEvidencePageGet (req, res, next) {
  const context = {
    ...await evidencePageContext(req, next),
    errors: req.body.errors || []
  }

  res.render('supplier/compliance/evidence', context)
}

async function solutionComplianceEvidencePagePost (req, res, next) {
  const context = {
    ...await evidencePageContext(req, next),
    errors: req.body.errors || { items: [] },
    breadcrumbs: [
      { label: 'Onboarding.Title', url: `../../../../solutions/${req.solution.id}` },
      { label: 'CompliancePages.Dashboard.Breadcrumb', url: `../../` }
    ]
  }

  const action = req.body.action || {}

  if (!solutionInCompliance(req.solution)) {
    return res.redirect('../../')
  }

  // Generate a new evidence record.
  const evidenceRecord = {
    id: require('node-uuid-generator').generate(),
    claimId: req.params.claim_id,
    createdOn: new Date(),
    originalDate: new Date(),
    createdById: req.user.contact.id,
    evidence: '',
    blobId: '' // ID of the file that was just uploaded, this relates a message to a file.
  }

  // check if the message is too long
  if (req.body.message && req.body.message.length > 300) {
    context.errors.items.push({ msg: 'Validation.Standard.Evidence.Message.TooLong' })
  } else {
    evidenceRecord.evidence = req.body.message
  }

  if (!req.files.length && action.submit !== 'direct' && !action.save && !action.exit) {
    context.errors.items.push({ msg: 'Validation.Standard.Evidence.File.Missing' })
  }

  // If there is a file, upload it and get the blobId.
  if (req.files.length) {
    const fileToUpload = req.files[0]
    try {
      evidenceRecord.blobId = await uploadFile(req.params.claim_id, fileToUpload.buffer, fileToUpload.originalname)
    } catch (err) {
      context.errorsitems.push({
        err,
        msg: 'Validation.Standard.Evidence.File.Failed.Upload'
      })
      console.err(err)
    }
  }

  // update the claim details
  const claim = _.find(req.solution.standards, { id: req.params.claim_id })

  claim.status = '1' /* draft */
  claim.ownerId = req.body.ownerId || null

  // Add the evidence record to the solution.
  req.solution.evidence.push(evidenceRecord)

  // update the solution with the new evidence record.
  try {
    await dataProvider.updateSolutionForCompliance(req.solution)
  } catch (err) {
    context.errors.items.push({
      err,
      msg: 'Validation.Standard.Evidence.Failed.Update'
    })
  }

  // re-render if an error occurred
  if (context.errors.items.length) {
    res.render('supplier/compliance/evidence', context)
  } else {
    let redirectUrl = './'
    if (action.save) redirectUrl += '?saved'
    if (action.submit) redirectUrl = './confirmation'
    else if (action.exit) redirectUrl = '/'

    res.redirect(redirectUrl)
  }
}

async function downloadEvidenceGet (req, res, next) {
  const context = {
    ...await evidencePageContext(req, next)
  }
  req.body.errors = req.body.errors || []

  const claimID = req.params.claim_id
  const fileName = req.params.file_name

  const selectedFile = _.find(context.files.items, ['name', fileName])

  downloadFile(claimID, selectedFile.blobId).then((fileResponse) => {
    res.header('Content-Disposition', `attachment; filename="${fileName}"`)
    fileResponse.body.pipe(res)
  }).catch((err) => {
    req.body.errors.push(err)
    solutionComplianceEvidencePageGet(req, res)
  })
}

async function solutionComplianceEvidenceConfirmationGet (req, res) {
  const context = {
    ...await evidencePageContext(req),
    activeForm: {
      title: req.solution && _([req.solution.name, req.solution.version]).filter().join(', ')
    }
  }

  const claim = _.find(req.solution.standards, { id: req.params.claim_id })

  context.standard = claim.standard

  let latestFile

  if ('saved' in req.query) {
    context.solutionSaved = true
  }

  if (context.files) {
    latestFile = findLatestFile(context.files.items)
  }

  if (latestFile) {
    latestFile.downloadURL = path.join(req.baseUrl, req.path.replace('confirmation', ''), latestFile.name)
    context.latestFile = latestFile
  }

  context.latestSubmission = _.maxBy(claim.submissionHistory, (sub) => sub.createdOn)

  if (!context.latestSubmission || !solutionInCompliance(req.solution)) {
    return res.redirect('../../')
  }

  res.render('supplier/compliance/confirmation', context)
}

async function solutionComplianceEvidenceConfirmationPost (req, res) {
  const action = req.body.action || {}

  let redirectUrl = action.save
    ? './'
    : '../../'

  const claim = _.find(req.solution.standards, { id: req.params.claim_id })

  if (action.submit) {
    claim.status = '2' /* submitted */
    redirectUrl += `?submitted=${claim.standardId}`

    req.solution.evidence.push({
      id: require('node-uuid-generator').generate(),
      claimId: req.params.claim_id,
      createdOn: new Date(),
      createdById: req.user.contact.id,
      evidence: 'Evidence Submitted',
      blobId: '' // ID of the file that was just uploaded, this relates a message to a file.
    })

    await dataProvider.updateSolutionForCompliance(req.solution)
  }
  if (action.save) {
    redirectUrl += '?saved'
  }

  res.redirect(redirectUrl)
}

async function downloadFile (claimID, blobId) {
  return sharePointProvider.downloadStdEvidence(claimID, blobId)
}

async function fetchFiles (claimID) {
  return sharePointProvider.getStdEvidenceFiles(claimID)
}

function findLatestFile (files) {
  return _.maxBy(files, (f) => f.timeLastModified)
}

async function uploadFile (claimID, buffer, fileName) {
  return sharePointProvider.uploadStdEvidence(claimID, buffer, fileName)
}

module.exports = router
