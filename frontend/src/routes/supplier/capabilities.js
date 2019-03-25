const _ = require('lodash')
const router = require('express').Router({ strict: true, mergeParams: true })
const path = require('path')

const { dataProvider } = require('catalogue-data')
const { sharePointProvider } = require('catalogue-sharepoint')

const multer = require('multer')
const storage = multer.memoryStorage()
const upload = multer({ storage: storage }).any()

/**
 * There is a boolean flag in the CRM that is set to 'true' when a live-witness demonstration was requested.
 * In order to improve the clarity of this message in the CRM, a string is also placed in the evidence description field.
 */
const LIVE_DEMO_MESSSAGE_INDICATOR = 'A live demonstration was requested for this Capability.'

// Multer is required to be used before CSRF for multi-part form parsing
router.use(upload)

// all routes in this module require CSRF protection
router.use(require('csurf')())

// all routes need to load a specified solution
router.param('solution_id', async (req, res, next, solutionId) => {
  try {
    req.solution = await dataProvider.solutionForAssessment(solutionId)
    next()
  } catch (err) {
    next(err)
  }
})

router
  .route('/:solution_id/')
  .get(solutionCapabilityPageGet)
  .post(solutionCapabilityPagePost)

router
  .route('/:solution_id/:claim_id/:file_name')
  .get(downloadEvidenceGet)

router
  .route('/:solution_id/confirmation')
  .get(confirmationPageGet)
  .post(confirmationPagePost)

router
  .route('/:solution_id/summary')
  .get(summaryPageGet)

function commonContext (req) {
  return {
    solution: req.solution,
    csrfToken: req.csrfToken(),
    activeForm: {
      title: req.solution && _([req.solution.name, req.solution.version]).filter().join(', ')
    }
  }
}

async function capabilityPageContext (req) {
  const context = {
    ...commonContext(req),
    ...await dataProvider.capabilityMappings(),
    errors: {
      items: []
    },
    breadcrumbs: [
      { label: 'Onboarding.Title', url: `../../solutions/${req.solution.id}` },
      { label: 'CapAssPages.Breadcrumb' }
    ]
  }

  context.activeForm.id = 'capability-assessment-form'

  const enumeration = await sharePointProvider.enumerateCapFolderFiles(req.solution.id).catch((err) => {
    context.errors.items.push(
      { msg: 'Validation.Capability.Evidence.Retrieval.FailedAction', err: err }
    )
  })

  context.solution.capabilities = context.solution.capabilities.map((cap) => {
    const files = enumeration[cap.claimID]

    let latestFile

    if (files) {
      latestFile = findLatestFile(files)
    }

    if (latestFile) {
      latestFile.downloadURL = path.join(req.baseUrl, req.path.replace('/confirmation', ''), cap.claimID, latestFile.name)
    }

    const latestEvidence = findLatestEvidence(cap.evidence)

    return {
      ...cap,
      latestFile: latestFile,
      latestEvidence: latestEvidence,
      isUploadingEvidence: latestEvidence && !latestEvidence.hasRequestedLiveDemo,
      hasRequestedLiveDemo: latestEvidence && latestEvidence.hasRequestedLiveDemo
    }
  })

  context.solution.capabilities = _.sortBy(context.solution.capabilities, 'name')

  return context
}

async function solutionCapabilityPageGet (req, res) {
  const context = {
    ...await capabilityPageContext(req)
  }

  // page is only editable if the solution is registered, and notyet submitted for assessment
  context.notEditable = context.solution.status !== dataProvider.getRegisteredStatusCode()

  if (context.notEditable) {
    delete context.activeForm.id
    return res.redirect('./summary')
  }

  if ('saved' in req.query) {
    context.solutionSaved = true
  }

  res.render('supplier/capabilities/index', context)
}

function validRequest (req) {
  const files = {}

  req.files.forEach((file) => {
    // Parsing the file fieldnames not parsed by multer
    const claimID = file.fieldname.replace('evidence-file[', '').replace(']', '')
    files[claimID] = {
      ...file,
      fieldname: claimID
    }
  })

  const claimedCapabilities = req.solution.capabilities.reduce((prev, cap) => ({ ...prev, [cap.claimID]: cap.name }), { })

  let errors = []

  const tooLong = '$t(Validation.Capability.Evidence.Description.Too Long)'
  const videoMissing = '$t(Validation.Capability.Evidence.File.Missing)'
  const descriptionMissing = '$t(Validation.Capability.Evidence.Description.Missing)'
  const optionNotSelected = '$t(Validation.Capability.Evidence.Not Selected)'

  const claimsNotSelected = !_.has(req.body, 'uploading-video-evidence')
    ? _.keys(claimedCapabilities)
    : _.filter(_.keys(claimedCapabilities), (claim) => !_.has(req.body['uploading-video-evidence'], claim))

  // filter all claims that don't require files, and don't have a file uploaded
  const previousUploads = req.body['evidence-file'] || {}
  const claimsRequiringFiles = _.keys(_.omitBy(req.body['uploading-video-evidence'], (val) => val !== 'yes'))

  const claimsMissingFiles = _.filter(claimsRequiringFiles, (claim) => _.isEmpty(files[claim]) && _.isEmpty(previousUploads[claim]))

  // Check that all claims requiring a file have a none false description
  const evidenceDescriptions = req.body['evidence-description']

  const claimsWithDescriptions = _.filter(claimsRequiringFiles, (claim) => !_.isEmpty(evidenceDescriptions[claim]))
  const claimsMissingDescriptions = _.filter(claimsRequiringFiles, (claim) => _.isEmpty(evidenceDescriptions[claim]))

  // we want to check description length regardless of the action.
  const claimsWithTooLongDescriptions = claimsWithDescriptions.filter((claim) => evidenceDescriptions[claim].length > 400)

  errors = errors.concat(claimsWithTooLongDescriptions.map((claim) => ({ claim, error: tooLong })))

  // if we're submitting, we want to check that everything is present.
  if (_.has(req, 'body.action.submit') || _.has(req, 'body.action.continue')) {
    errors = errors.concat(claimsMissingFiles.map((claim) => ({ claim, error: videoMissing })))
    errors = errors.concat(claimsMissingDescriptions.map((claim) => ({ claim, error: descriptionMissing })))
    errors = errors.concat(claimsNotSelected.map((claim) => ({ claim, error: optionNotSelected })))
  }

  errors = _.sortBy(errors.map((err) => ({
    error: err.error,
    claim: err.claim,
    name: claimedCapabilities[err.claim]
  })), 'name')

  return errors
}

async function solutionCapabilityPagePost (req, res) {
  const context = {
    errors: {
      items: []
    }
  }

  const valRes = validRequest(req)

  // Check if any validation Erors occured
  if (!_.isEmpty(valRes)) {
    const validationErrors = {
      items: valRes.map((err) => ({
        ...err,
        msg: `${err.name} ${err.error}`
      }))
    }
    context.errors = validationErrors
  }

  // parse anyfiles to be uploaded to SharePoint
  const files = parseFiles(req.files)

  const uploadingVideoEvidence = req.body['uploading-video-evidence']
  let systemError

  const evidenceDescriptions = await Promise.all(_.map(uploadingVideoEvidence, async (isUploading, claimID) => {
    let fileToUpload = files[claimID]
    let blobId = null

    if (fileToUpload) {
      blobId = await uploadFile(claimID, fileToUpload.buffer, fileToUpload.originalname).catch((err) => {
        context.errors.items.push({ msg: 'Validation.Capability.Evidence.Upload.FailedAction' })
        console.err(err)
        systemError = err
      })
    }

    const currentEvidence = _.filter(req.solution.evidence, { claimId: claimID })
    const latestEvidence = findLatestEvidence(currentEvidence)

    return {
      id: require('node-uuid-generator').generate(),
      previousId: latestEvidence ? latestEvidence.id : null,
      claimId: claimID,
      createdById: req.user.contact.id,
      createdOn: new Date().toISOString(),
      evidence: isUploading === 'yes' ? req.body['evidence-description'][claimID] : LIVE_DEMO_MESSSAGE_INDICATOR,
      hasRequestedLiveDemo: isUploading !== 'yes',
      blobId: blobId
    }
  }))

  // Update solution evidence, communicate the error if there isn't any already.
  await updateSolutionCapabilityEvidence(req.solution.id, evidenceDescriptions).catch((err) => {
    context.errors.items.push({ msg: 'Validation.Capability.Evidence.Update.FailedAction' })
    console.err(err)
    systemError = err
  })

  // only show validation errors if the user elected to continue, not just save
  if (systemError || context.errors.items.length) {
    // regenerate the full context (preserving errors)
    req.solution = await dataProvider.solutionForAssessment(req.params.solution_id)
    const fullContext = await capabilityPageContext(req)
    fullContext.errors = context.errors

    fullContext.solution.capabilities = fullContext.solution.capabilities.map((cap) => ({
      ...cap,
      hasError: !_.isEmpty(context.errors.items.find((err) => err.claim === cap.claimID))
    }))
    res.render('supplier/capabilities/index', fullContext)
  } else if (req.body.action.continue) {
    const url = req.url.split('?')[0]
    res.redirect(path.join(req.baseUrl, url, 'confirmation'))
  } else if (req.body.action.exit) {
    res.redirect('/')
  } else if (req.body.action.save) {
    let url = req.url.replace('?saved', '')
    let redirectUrl = path.join(req.baseUrl, `${url}?saved`)
    res.redirect(redirectUrl)
  } else {
    res.redirect(path.join(req.baseUrl, req.url))
  }
}

async function downloadEvidenceGet (req, res) {
  req.body.errors = req.body.errors || []

  const claimID = req.params.claim_id
  const fileName = req.params.file_name

  const files = await fetchFiles(claimID)
  const selectedFile = _.find(files.items, ['name', fileName])

  downloadFile(claimID, selectedFile.blobId).then((fileResponse) => {
    res.header('Content-Disposition', `attachment; filename="${fileName}"`)
    fileResponse.body.pipe(res)
  }).catch((err) => {
    req.body.errors.push(err)
    solutionCapabilityPageGet(req, res)
  })
}

async function confirmationPageContext (req) {
  const context = {
    ...await capabilityPageContext(req)
  }

  context.solution.standardsByGroup = context.solution.capabilities.reduce((obj, cap) => {
    cap.standardsByGroup.associated.forEach((std) => { obj.associated[std.id] = std })
    cap.standardsByGroup.overarching.forEach((std) => { obj.overarching[std.id] = std })
    return obj
  },
  { associated: {}, overarching: {} })

  context.solution.standardsByGroup.associated = _.sortBy(_.map(context.solution.standardsByGroup.associated), 'name')
  context.solution.standardsByGroup.overarching = _.sortBy(_.map(context.solution.standardsByGroup.overarching), 'name')

  context.solution.capabilities = context.solution.capabilities.map((cap) => {
    let isLiveDemo = false
    let missingEvidence = true

    if (cap.latestEvidence && cap.latestEvidence.hasRequestedLiveDemo) {
      isLiveDemo = true
      missingEvidence = false
    } else if (cap.latestEvidence && cap.latestFile) {
      missingEvidence = false
    }
    return {
      ...cap,
      isLiveDemo,
      missingEvidence
    }
  })

  context.solution.capabilities = _.sortBy(context.solution.capabilities, 'name')

  context.editUrls = {
    registration: `/suppliers/solutions/${req.params.solution_id}/register`,
    capabilities: {
      selection: `/suppliers/solutions/${req.params.solution_id}/capabilities`,
      evidence: `/suppliers/capabilities/${req.params.solution_id}`
    }
  }
  return context
}

async function summaryPageGet (req, res) {
  const context = {
    ...await confirmationPageContext(req)
  }
  context.breadcrumbs = [
    { label: 'Onboarding.Title', url: `../../solutions/${req.solution.id}` },
    { label: 'CapAssPages.Summary.Breadcrumb' }
  ]
  context.editUrls = {}
  context.backlink = `../../solutions/${req.solution.id}`

  context.solution.hasOptionalStandards = context.solution.standardsByGroup.associated.some((std) => std.isOptional)

  res.render('supplier/capabilities/summary', context)
}

async function confirmationPageGet (req, res, prevAction) {
  const context = {
    ...await confirmationPageContext(req)
  }

  if (_.get(req, 'body.action.save')) {
    context.solutionSaved = true
  }

  // page is only editable if the solution is registered, and notyet submitted for assessment
  context.notEditable = context.solution.status !== dataProvider.getRegisteredStatusCode()

  if (context.notEditable) {
    delete context.activeForm.id
  }

  // every solution needs an evidence file w/ a description, or needs to be a live demo.
  if (!context.solution.capabilities.every((cap) => cap.isLiveDemo || !cap.missingEvidence)) {
    res.redirect(path.join(req.baseUrl, req.url.replace('confirmation', '')))
  } else {
    res.render('supplier/capabilities/confirmation', context)
  }
}

async function confirmationPagePost (req, res) {
  const context = {
    ...await capabilityPageContext(req)
  }

  if (req.body.action.save) {
    return confirmationPageGet(req, res)
  } else if (req.body.action.exit) {
    return res.redirect('/')
  } else if (req.body.action.continue) {
    try {
      await submitCapabilityAssessment(req.solution.id)
      return redirectToOnboardingDashboard(req, res, 'capabilityAssessmentSubmitted')
    } catch (err) {
      context.errors = {
        items: [{ msg: 'Validation.Capability.Evidence.Submission.FailedAction' }]
      }
      res.redirect(path.join(req.baseUrl, req.url.replace('confirmation', '')))
    }
  }
}

function submitCapabilityAssessment (solutionID) {
  return dataProvider.submitSolutionForCapabilityAssessment(solutionID)
}

async function redirectToOnboardingDashboard (req, res, param) {
  const route = `${path.join('/suppliers/solutions', req.params.solution_id)}${param ? `?${param}` : ''}`
  return res.redirect(route)
}

async function updateSolutionCapabilityEvidence (solutionID, evidenceDescriptions) {
  return dataProvider.updateSolutionCapabilityEvidence(solutionID, evidenceDescriptions)
}

async function downloadFile (claimID, blobId) {
  return sharePointProvider.downloadCapEvidence(claimID, blobId)
}

async function fetchFiles (claimID) {
  return sharePointProvider.getCapEvidenceFiles(claimID)
}

function findLatestFile (files) {
  return _.maxBy(files, 'timeLastModified')
}

function findLatestEvidence (evidence) {
  // follow the trail of previousIds until there are no more
  let previousId = null
  let currentEvidence
  let latestEvidence

  while ((currentEvidence = _.find(evidence, { previousId }))) {
    latestEvidence = currentEvidence
    previousId = currentEvidence.id
  }

  return latestEvidence
}

function parseFiles (files) {
  let fileMap = {}
  files.map((file) => {
    return {
      ...file,
      claimID: extractClaimIDFromUploadFieldName(file.fieldname)
    }
  }).forEach((file) => {
    fileMap[file.claimID] = file
  })
  return fileMap
}

function extractClaimIDFromUploadFieldName (fieldName) {
  return fieldName.replace('evidence-file[', '').replace(']', '')
}

async function uploadFile (claimID, buffer, fileName) {
  let res = await sharePointProvider.uploadCapEvidence(claimID, buffer, fileName)
  return res
}

module.exports = router
