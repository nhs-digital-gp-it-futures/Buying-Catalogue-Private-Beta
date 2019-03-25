/* global fixture, test */

import { Selector, RequestLogger } from 'testcafe'

import { asSupplier } from './roles'
import { supplierDashboardPage, onboardingDashboardPage, standardsComplianceDashboardPage } from './pages'

const standardNameMap = {
  'Interoperability Standard': {
    id: '23446cf9-d461-4907-b099-41f35b65de62',
    standardId: 'S32'
  },
  'IM1 - Interface Mechanism': {
    id: '2c8c9cc8-141b-4d06-872a-8d5e5efb3641',
    standardId: 'S47'
  },
  'Information Governance': {
    id: '3a7735f2-759d-4f49-bca0-0828f32cf86c',
    standardId: 'S30'
  },
  'Management Information (MI) Reporting': {
    id: '3d10430f-1748-44ad-8df3-5d5d11544f75',
    standardId: 'S49'
  },
  'Citizen Access': {
    id: '522e58a7-6f14-4c28-9340-193482915401',
    standardId: 'S21'
  },
  'Data Standards': {
    id: '64c5d608-772f-4bc2-905b-4cee5d0b6f91',
    standardId: 'S27'
  },
  'View Record - Citizen - Standard': {
    id: '6cf61bc3-9714-4902-953f-76a238d5ffd5',
    standardId: 'S4'
  },
  'Testing': {
    id: '719722d0-2354-437e-acdc-4625989bbca8',
    standardId: 'S69'
  },
  'Service Management': {
    id: '7b62b29a-62a7-4b4a-bcc8-dfa65fb7e35c',
    standardId: 'S65'
  },
  'Non-Functional Questions': {
    id: '99619bdd-6452-4850-9244-a4ce9bec70ca',
    standardId: 'S63'
  },
  'Commercial Standard': {
    id: '9e7780e9-7263-4ee4-a722-b3e1aaff4476',
    standardId: 'S31'
  },
  'Clinical Safety': {
    id: 'a5774787-3ab3-48ff-9fa2-6db06be1b926',
    standardId: 'S25'
  },
  'Data Migration': {
    id: 'cd07ffcf-822b-460a-9f94-be42ad9b995b',
    standardId: 'S26'
  },
  'Hosting & Infrastructure': {
    id: 'cdfdebda-edde-4af4-aaf4-9d59fca7cdaa',
    standardId: 'S29'
  },
  'Business Continuity & Disaster Recovery': {
    id: 'e70fd085-ec42-4901-a7ba-a0d6f61fbbe1',
    standardId: 'S24'
  },
  'Training': {
    id: 'f49f91de-64fc-4cca-88bf-3238fb1de69b',
    standardId: 'S28'
  }
}

const downloadFileName = 'Dummy TraceabilityMatrix.xlsx'

function setFileToUpload (t, filename) {
  return t
    .setFilesToUpload(
      Selector('input[type=file]'),
      `./uploads/${filename}`
    )
}

const requestLogger = RequestLogger(
  request => request.url.startsWith(standardsComplianceDashboardPage.baseUrl),
  { logResponseBody: true }
)

fixture('Standards Compliance - Evidence')
  .page(supplierDashboardPage.baseUrl)
  .beforeEach(navigateToStandardsDashboard)
  .afterEach(supplierDashboardPage.checkAccessibility)

function navigateToStandardsDashboard (t) {
  return asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.lastOnboardingSolutionName)
    .click(onboardingDashboardPage.standardsComplianceButton)
    .expect(Selector('#compliance').exists).ok()
}

test('With no traceability Matrix present, the page displays a \'please wait\' message', async t => {
  const messageSelector = Selector('#compliance .notification h1')
  await t
    .click(`a[href*="${standardNameMap['Non-Functional Questions'].id}"]`)
    .expect(messageSelector.innerText).contains('Waiting for file')
})

test.skip('With a solution that has not submitted capabiility assessment but has a TM present, the page shows an appropriate notification', async t => {
  const messageSelector = Selector('#compliance .notification h1')

  await asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.capAssSubmittedSolutionName)
    .click(onboardingDashboardPage.standardsComplianceButton)
    .expect(Selector('#compliance').exists).ok()

  await t
    .click(`a[href*="${standardNameMap['Data Migration'].id}"]`)
    .expect(messageSelector.innerText).contains('File available')
})

test('a \'Not Started\' With a traceability Matrix present, the page shows a form with a file upload', async t => {
  await t
    .click(`a[href*="${standardNameMap['Testing'].id}"]`)
    .expect(Selector('.file-input')).ok()
})

test('A submitted standard does not allow further uploads but does allow download', async t => {
  await t
    .click(`a[href*="${standardNameMap['Information Governance'].id}"]`)
  await t
    .expect(Selector('header.page-main-heading h1').innerText).contains('Evidence Submitted')
    .expect(Selector('.has-feedback a.file-download-link').exists).ok()
})

// Skipped as the acceptance criteria did not clearly specify this as being required,
// meaning that it was not implemented with this in mind.
// This test should be re-enabled once the tech debt is resolved.
test.skip('A Failed standard should not allow the uploading of any more files', async t => {
  await t
    .click(`a[href*="${standardNameMap['Interoperability Standard'].id}"]`)
    .expect(Selector('.file-input').exists).notOk()
})

test
  .requestHooks(requestLogger)(
    'A Standard with a file to download should allow the download of that file', async t => {
      const testingStdSelector = await Selector(`a[href*="${standardNameMap['Testing'].id}"]`)

      await t.click(testingStdSelector) // go from the dashboard to the download page

      requestLogger.clear()

      await t.click(testingStdSelector) // click the download link.
        .expect(requestLogger.contains(record => record.response.statusCode === 200)).ok()

      await t
        .expect(
          requestLogger.requests[0].response.body.toString()
        ).eql('Content of the test "Dummy traceabilityMatrix.xlsx" file', 'Download does not match expected')
    }
  )

test('A \'Not Started\' standard should change to draft if a file is saved against it.', async t => {
  // navigate to 'business continuity...' standard evidence upload page.
  const businessContinuitySelector = Selector(`a[href*="${standardNameMap['Business Continuity & Disaster Recovery'].id}"]`)
  await t.click(businessContinuitySelector)
  await setFileToUpload(t, downloadFileName)
    .click('input[value="Save"]')
    .click('.breadcrumb li:nth-child(2) > a')

    // Selecting the Row that is the parent of the link, so that the sibling cell with containing status can be checked
    .expect(businessContinuitySelector.parent().nth(1).find('.status').innerText).eql('Draft')
    .expect(businessContinuitySelector.parent().nth(1).find('.owner').innerText).eql('With Helpma Boab')
})

test('The owner of a standard can be set correctly and is reflected on the Dashboard when saved', async t => {
  // navigate to 'business continuity...' standard evidence upload page.
  const businessContinuitySelector = Selector(`a[href*="${standardNameMap['Business Continuity & Disaster Recovery'].id}"]`)
  const ownerDropdown = Selector('#compliance [name="ownerId"]')
  const candidateOwners = ownerDropdown.child('option')

  await t
    .click(businessContinuitySelector)
    .expect(Selector('#compliance .standard-owner .current-owner').textContent).eql('Helpma Boab')
    .expect(ownerDropdown.visible).notOk()

  await t
    .click('#change-owner-button')
    .expect(candidateOwners.count).eql(4)
    .expect(candidateOwners.nth(0).textContent).contains('Lead Contact (Helpma Boab)')
    .expect(candidateOwners.nth(1).textContent).contains('Dr Kool')
    .expect(candidateOwners.nth(2).textContent).contains('Helpma Boab')
    .expect(candidateOwners.nth(3).textContent).contains('Zyra Featherstonhaugh')

  await setFileToUpload(t, downloadFileName)
    .click(ownerDropdown)
    .click(candidateOwners.nth(3))
    .click('input[value="Save"]')
    .click('.breadcrumb li:nth-child(2) > a')

    // Selecting the Row that is the parent of the link, so that the sibling cell with containing owner can be checked
    .expect(businessContinuitySelector.parent().nth(1).find('.owner').textContent).contains('Zyra Featherstonhaugh')
})

test('A standard should change to Submitted if evidence is submitted.', async t => {
  // navigate to 'business continuity...' standard evidence upload page.
  const clinicalSafetySelector = Selector(`a[href*="${standardNameMap['Clinical Safety'].id}"]`)

  await t.click(clinicalSafetySelector)

  await setFileToUpload(t, downloadFileName)
    .click('#submit-for-compliance')
    .expect(Selector('#compliance-confirmation > h1').innerText).contains('Review and submit')

  await t
    .click('#compliance-submission-confirmation-button')

    // Selecting the Row that is the parent of the link, so that the sibling cell with containing status can be checked
    .expect(clinicalSafetySelector.parent().nth(1).find('.status').innerText).eql('Submitted')
    .expect(clinicalSafetySelector.parent().nth(1).find('.owner').innerText).eql('With NHS Digital')
    .expect(Selector('.standard-submitted h3').innerText).contains('Evidence submitted')
})

test('A standard sent back to the supplier should allow the supplier to resubmit', async t => {
  // navigate to 'business continuity...' standard evidence upload page.
  const trainingSelector = Selector(`a[href*="${standardNameMap['Training'].id}"]`)

  await t.click(trainingSelector)

  await setFileToUpload(t, downloadFileName)
    .click('#submit-for-compliance')
    .expect(Selector('#compliance-confirmation > h1').innerText).contains('Review and submit')

  await t
    .click('#compliance-submission-confirmation-button')

    // Selecting the Row that is the parent of the link, so that the sibling cell with containing status can be checked
    .expect(trainingSelector.parent().nth(1).find('.status').innerText).eql('Submitted')
})
