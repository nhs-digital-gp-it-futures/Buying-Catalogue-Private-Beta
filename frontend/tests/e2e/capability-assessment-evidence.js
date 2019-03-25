/* global fixture, test */

import { Selector, RequestLogger } from 'testcafe'

import { asSupplier } from './roles'
import { supplierDashboardPage, onboardingDashboardPage, capabilityEvidencePage } from './pages'

fixture('Capability Assessment - Evidence')
  .page(supplierDashboardPage.baseUrl)
  .beforeEach(navigateToCapabilityAssessment)
  .afterEach(supplierDashboardPage.checkAccessibility)

function navigateToCapabilityAssessment (t) {
  return asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.secondOnboardingSolutionName)
    .click(onboardingDashboardPage.capabilityAssessmentButton)
    .expect(Selector('#capability-assessment-page, #capability-submission-confirmation').exists).ok()
}

test('All capabilities registered against the solution are present in the alphabetic order', async t => {
  await t
    .expect(Selector('fieldset.collapsible#C1 > legend').textContent).eql('Appointments Management - Citizen')
    .expect(Selector('fieldset.collapsible#C2 > legend').textContent).eql('Communicate With Practice - Citizen')
    .expect(Selector('fieldset.collapsible#C3 > legend').textContent).eql('Prescription Ordering - Citizen')

    .expect(Selector('form#capability-assessment-form > fieldset.collapsible:nth-of-type(1)').id).eql('C1')
    .expect(Selector('form#capability-assessment-form > fieldset.collapsible:nth-of-type(2)').id).eql('C2')
    .expect(Selector('form#capability-assessment-form > fieldset.collapsible:nth-of-type(3)').id).eql('C3')
})

test('Collapsible fieldsets behave as an accordion', async t => {
  const allTheFieldsets = Selector('form#capability-assessment-form > fieldset.collapsible')

  await t
    .expect(allTheFieldsets.nth(0).hasClass('collapsed')).notOk()
    .expect(allTheFieldsets.nth(1).hasClass('collapsed')).ok()
    .expect(allTheFieldsets.nth(2).hasClass('collapsed')).ok()

    .click(allTheFieldsets.nth(2).find('legend'))

    .expect(allTheFieldsets.nth(0).hasClass('collapsed')).ok()
    .expect(allTheFieldsets.nth(1).hasClass('collapsed')).ok()
    .expect(allTheFieldsets.nth(2).hasClass('collapsed')).notOk()

    .click(allTheFieldsets.nth(0).find('legend'))
    .click(allTheFieldsets.nth(1).find('legend'))

    .expect(allTheFieldsets.nth(0).hasClass('collapsed')).ok()
    .expect(allTheFieldsets.nth(1).hasClass('collapsed')).notOk()
    .expect(allTheFieldsets.nth(2).hasClass('collapsed')).ok()
})

test('No default selection of upload/live demo', async t => {
  const allTheFieldsets = Selector('form#capability-assessment-form > fieldset.collapsible')

  await t
    .expect(allTheFieldsets.nth(0).find('input[type=radio]:checked').count).eql(0)

    .click(allTheFieldsets.nth(1).find('legend'))
    .expect(allTheFieldsets.nth(1).find('input[type=radio]:checked').count).eql(0)

    .click(allTheFieldsets.nth(2).find('legend'))
    .expect(allTheFieldsets.nth(2).find('input[type=radio]:checked').count).eql(0)
})

function uploadFile (t, sel, filename) {
  return t
    .click(sel.child('legend'))
    .click(sel.find('input[type=radio][value=yes]'))
    .setFilesToUpload(
      sel.find('input[type=file]'),
      `./uploads/${filename}`
    )
}

function uploadFileWithMessage (t, sel, filename, message) {
  return uploadFile(t, sel, filename)
    .typeText(
      sel.find('textarea'),
      message,
      { replace: true }
    )
}

function verifyFileUploaded (t, sel, filename) {
  return t
    .click(sel.child('legend'))
    .expect(sel.find('.latest-upload').textContent).contains('last file you uploaded was')
    .expect(sel.find('.latest-upload a').textContent).eql(filename)
}

test('After uploading a file, the supplier dashboard and onboarding dashboard should show updated status', async t => {
  // we'll add evidence for "GP Resource Management"
  const capSection = Selector('fieldset.collapsible#C3')

  await uploadFileWithMessage(t, capSection, 'Dummy TraceabilityMatrix.xlsx', 'Automation testing message sent with uploaded file')
    .click(capabilityEvidencePage.globalSaveAndExitButton)
    .click(supplierDashboardPage.secondOnboardingSolutionName)

    .expect(onboardingDashboardPage.capabilityAssessmentButton.textContent).eql('Edit')
    .expect(onboardingDashboardPage.capabilityAssessmentStatus.textContent).contains('Draft Saved')
})

test('After uploading a file, the name of the file should show against the capability', async t => {
  const capSection = Selector('fieldset.collapsible#C3')

  await verifyFileUploaded(t, capSection, 'Dummy TraceabilityMatrix.xlsx')
})

const requestLogger = RequestLogger(
  request => request.url.startsWith(capabilityEvidencePage.baseUrl),
  { logResponseBody: true }
)

test
  .requestHooks(requestLogger)(
    'On download, the previously uploaded file should be identical', async t => {
      const capSection = Selector('fieldset.collapsible#C3')

      requestLogger.clear()

      await t
        .click(capSection.child('legend'))
        .click(capSection.find('.latest-upload a'))

        // Ensure that the response has been received and that its status code is 200.
        .expect(requestLogger.contains(record => record.response.statusCode === 200)).ok()

      // Compute the SHA1 hash of the downloaded data and compare to the known hash
      // of the file that was uploaded
      const crypto = require('crypto')
      const hash = crypto.createHash('sha1')
      hash.update(requestLogger.requests[0].response.body)

      await t
        .expect(hash.digest('hex')).eql('21642af57eb7fd206395cdf745672bdafb8b96ae', 'Download does not match upload')
    }
  )

test('Messages sent along with the uploaded file should be preserved', async t => {
  const capSection = Selector('fieldset.collapsible#C3')

  await verifyFileUploaded(t, capSection, 'Dummy TraceabilityMatrix.xlsx')
    .expect(capSection.find('textarea').value).eql('Automation testing message sent with uploaded file')
})

// Test skipped due to Testcafé bug #?
// Which we discovered with this very test 🙂
test.skip('Files and messages can be saved against multiple capabilities at once', async t => {
  // note that the second capability is done first so that the accordion click doesn't
  // close the first capability's fieldset
  const capSection1 = Selector('fieldset.collapsible#C2')
  await uploadFileWithMessage(t, capSection1, 'Dummy TraceabilityMatrix 2.xlsx', 'Automated test message for C2')

  const capSection2 = Selector('fieldset.collapsible#C1')
  await uploadFileWithMessage(t, capSection2, 'Dummy TraceabilityMatrix 3.xlsx', 'Automated test message for C1')

  await t
    .click(capabilityEvidencePage.globalSaveButton)

  await t.expect(capabilityEvidencePage.savedNoticeTitle.innerText).contains('Saved')

  await verifyFileUploaded(t, capSection1, 'Dummy TraceabilityMatrix 2.xlsx')
    .expect(capSection1.find('textarea').value).eql('Automated test message for C2')
  await verifyFileUploaded(t, capSection2, 'Dummy TraceabilityMatrix 3.xlsx')
    .expect(capSection2.find('textarea').value).eql('Automated test message for C1')
})

test('Clicking Continue with incomplete evidence should trigger a validation message', async t => {
  // submitting with no selections should trigger an error for all capabilities along side the message.
  await t
    .click(capabilityEvidencePage.continueButton)
    .expect(Selector('#errors').exists).ok()
    .expect(Selector('#errors li:nth-child(1)').textContent).contains('Appointments Management - Citizen requires an option to be selected')
    .expect(Selector('#errors li:nth-child(2)').textContent).contains('Communicate With Practice - Citizen requires an option to be selected')

  // submitting with only one selection made sand nothing else done should trigger some error messages...
  await t
    .click(Selector('fieldset.collapsible#C1').find('input[type=radio][value=yes]'))
    .click(capabilityEvidencePage.continueButton)
    .expect(Selector('#errors').exists).ok()
    .expect(Selector('#errors li:nth-child(1)').textContent).contains('Appointments Management - Citizen is missing a video')
    .expect(Selector('#errors li:nth-child(2)').textContent).contains('Appointments Management - Citizen is missing a description')
    .expect(Selector('#errors li:nth-child(3)').textContent).contains('Communicate With Practice - Citizen requires an option to be selected')

  // clicking save should not trigger any validation errors if one option is selected, but no all.
  await t
    .click(capabilityEvidencePage.globalSaveButton)
    .expect(Selector('#errors').exists).notOk()

  const firstCapSection = Selector('fieldset.collapsible#C1')
  const secondCapSection = Selector('fieldset.collapsible#C2')

  // when all have indicated they want videos, then each capability should have a video missing and description missing error
  await t
    .click(firstCapSection).click(firstCapSection.find('input[type=radio][value=yes]'))
    .click(secondCapSection).click(secondCapSection.find('input[type=radio][value=yes]'))
    .click(capabilityEvidencePage.continueButton)
    .expect(Selector('#errors').exists).ok()
    .expect(Selector('#errors li:nth-child(1)').textContent).contains('Appointments Management - Citizen is missing a video')
    .expect(Selector('#errors li:nth-child(2)').textContent).contains('Appointments Management - Citizen is missing a description')
    .expect(Selector('#errors li:nth-child(3)').textContent).contains('Communicate With Practice - Citizen is missing a video')
    .expect(Selector('#errors li:nth-child(4)').textContent).contains('Communicate With Practice - Citizen is missing a description')

  // Uploading a file for a capability should remove it's missing video evidence error
  // ensure first capability is requesting a video upload
  await t.click(
    Selector('fieldset.collapsible#C1').find('input[type=radio][value=yes]')
  )

  // collapse the open invalid collapsible sections
  const capSection1 = Selector('fieldset.collapsible#C2')
  const capSection2 = Selector('fieldset.collapsible#C1')
  await t.click(capSection1.child('legend'))
  await t.click(capSection2.child('legend'))

  // note that the second capability is done first so that the accordion click doesn't
  // close the first capability's fieldset
  await uploadFileWithMessage(t, capSection1, 'Dummy TraceabilityMatrix 2.xlsx', 'Automated test message for C2')

  await t
    .click(capabilityEvidencePage.continueButton)
    .expect(Selector('#errors').exists).ok()
    .expect(Selector('#errors li:nth-child(1)').textContent).contains('Appointments Management - Citizen is missing a video')
    .expect(Selector('#errors li:nth-child(2)').textContent).contains('Appointments Management - Citizen is missing a description')

  // collapse the open invalid collapsible section
  await t.click(capSection2.child('legend'))

  // clicking save should not trigger any validation errors if options are clicked, but only one file is uploaded
  await t
    .click(capabilityEvidencePage.globalSaveButton)
    .expect(Selector('#errors').exists).notOk()

  // as the first capability has no file or message, the missing file validation error should trigger
  // even with a validation message, the submitted file and message should hold
  await verifyFileUploaded(t, capSection1, 'Dummy TraceabilityMatrix 2.xlsx')
    .expect(capSection1.find('textarea').value).eql('Automated test message for C2')

  // uploading a file for the first capability without setting a message should
  // trigger a missing description validation error
  await uploadFile(t, capSection2, 'Dummy TraceabilityMatrix 3.xlsx')

  await t
    .click(capabilityEvidencePage.continueButton)
    .expect(Selector('#errors').exists).ok()

  // even with a validation message, the submitted file and message should hold
  await t.click(capSection2.child('legend')) // close the section because the next call opens it
  await verifyFileUploaded(t, capSection2, 'Dummy TraceabilityMatrix 3.xlsx')
})

test('Clicking Continue with complete evidence should lead to summary page with correct details', async t => {
  const capSection1 = Selector('fieldset.collapsible#C1')

  // Set one capability to request a live demo
  await t
    .click(capSection1.find('input[type=radio][value=no]'))
    .click(capabilityEvidencePage.continueButton)

  // Assert Summary section headings are there.
  await t
    .expect(Selector('#summary-details h2').innerText).contains('Solution details')
    .expect(Selector('#summary-capabilities h2').innerText).contains('Capabilities')
    .expect(Selector('#summary-evidence h2').innerText).contains('Provide evidence')

  // Assert Assessment Evidence is correct.
  const evidenceTypes = Selector('#summary-evidence .evidence p:nth-child(2)')
  await t
    .expect(evidenceTypes.nth(0).innerText).contains('Evidence: Live Witness Demonstration')
    .expect(evidenceTypes.nth(1).innerText).contains('Evidence: Recorded Video')
    .expect(evidenceTypes.nth(2).innerText).contains('Evidence: Recorded Video')
})

test('Submitting from the summary page should update the status on the dashboard and onboarding page', async t => {
  const capSection1 = Selector('fieldset.collapsible#C1')

  await t
    .click(capSection1.find('input[type=radio][value=no]'))
    .click(capabilityEvidencePage.continueButton)
    .click('button[name="action\\[continue\\]"]')
    .expect(Selector('.callout .title').innerText).contains('Solution')
    .expect(Selector('.callout .title').innerText).contains('submitted')
    .expect(Selector('a[href^="../../compliance/"]')).ok()
})

test('After Submitting Evidence for Capability Assessment, the Registration Pages should redirect to a Summary Page', async t => {
  await asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.secondOnboardingSolutionName)

  await t
    .click(onboardingDashboardPage.continueRegistrationButton)
    .expect(Selector('#summary-details h2').innerText).contains('Enter Solution details')
    .expect(Selector('#summary-capabilities h2').innerText).contains('Which Capabilities')
    .expect(Selector('#summary-evidence h2').innerText).contains('Provide evidence')
})

test('After Submitting Evidence for Capability Assessment, the Capability Assessment page should redirect to a summary page.', async t => {
  await asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.secondOnboardingSolutionName)
    .click(onboardingDashboardPage.capabilityAssessmentButton)

  await t
    .click(Selector('#summary-capabilities > details > summary > span'))
    .expect(Selector('#summary-details h2').innerText).contains('Enter Solution details')
    .expect(Selector('#summary-capabilities h2').innerText).contains('Which Capabilities')
    .expect(Selector('#summary-evidence h2').innerText).contains('Provide evidence')
})

test('The Capability Assessment Summary page should display capabilities alphabetically.', async t => {
  await asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.secondOnboardingSolutionName)
    .click(onboardingDashboardPage.capabilityAssessmentButton)

  await t.click(Selector('#summary-capabilities > details > summary > span'))

  const allCapabilities = Selector('#summary-capabilities div.capabilities p a')

  await t
    .expect(allCapabilities.nth(0).innerText).contains('Appointments Management')
    .expect(allCapabilities.nth(1).innerText).contains('Communicate With Practice')
    .expect(allCapabilities.nth(2).innerText).contains('Prescription Ordering')
})

test('The Capability Assessment Summary page should display the number of applicable standards.', async t => {
  await asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.secondOnboardingSolutionName)
    .click(onboardingDashboardPage.capabilityAssessmentButton)

  // should be 19 standards in totals. 8 Associated and 11 Overarching.
  await t.expect(Selector('#summary-capabilities > details > summary > span').innerText).contains('19 Standards')
})

test('The Capability Assessment Summary page should display Associated Standards alphabetically.', async t => {
  await asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.secondOnboardingSolutionName)
    .click(onboardingDashboardPage.capabilityAssessmentButton)

  await t.click(Selector('#summary-capabilities > details > summary > span'))

  const allStandards = Selector('#summary-capabilities details div p a')

  await t
    .expect(allStandards.nth(0).innerText).contains('Appointments Management - Citizen - Standard')
    .expect(allStandards.nth(1).innerText).contains('Citizen Access')
    .expect(allStandards.nth(2).innerText).contains('Communicate with Practice - Citizen - Standard')
    .expect(allStandards.nth(3).innerText).contains('IM1 - Interface Mechanism')
})

test('The Capability Assessment Summary page should display Associated Standards alphabetically.', async t => {
  await asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.secondOnboardingSolutionName)
    .click(onboardingDashboardPage.capabilityAssessmentButton)

  await t.click(Selector('#summary-capabilities > details > summary > span'))

  const allStandards = Selector('#summary-capabilities details div p a')

  // first 15 (0 - 14 inclusive) are Associated, next 11 (15 - 25 inclusive) are overarching
  await t
    .expect(allStandards.nth(8).innerText).contains('Business Continuity & Disaster Recovery')
    .expect(allStandards.nth(9).innerText).contains('Clinical Safety')
    .expect(allStandards.nth(10).innerText).contains('Commercial Standard')
    .expect(allStandards.nth(11).innerText).contains('Data Migration')
})
