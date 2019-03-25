/* global fixture, test */

import { Selector } from 'testcafe'

import { asSupplier } from './roles'
import { supplierDashboardPage, onboardingDashboardPage, standardsComplianceDashboardPage } from './pages'

/**
 * Expected statuses are predefined in the seed data used in testing.
 * There should be at least one of each status, a label is applied to those in draft
 * that don't yet have a file to download.
 * Id                                     StandardId  Status  Name
 * 23446cf9-d461-4907-b099-41f35b65de62	  S32	        -1	    Interoperability Standard
 * 2c8c9cc8-141b-4d06-872a-8d5e5efb3641	  S47	        0	      IM1 - Interface Mechanism
 * 3a7735f2-759d-4f49-bca0-0828f32cf86c	  S30	        2	      Information Governance
 * 3d10430f-1748-44ad-8df3-5d5d11544f75	  S49	        6	      Management Information (MI) Reporting
 * 522e58a7-6f14-4c28-9340-193482915401	  S21	        0	      Citizen Access
 * 64c5d608-772f-4bc2-905b-4cee5d0b6f91	  S27	        0	      Data Standards
 * 6cf61bc3-9714-4902-953f-76a238d5ffd5	  S4	        4	      View Record - Citizen - Standard
 * 719722d0-2354-437e-acdc-4625989bbca8	  S69	        0	      Testing
 * 7b62b29a-62a7-4b4a-bcc8-dfa65fb7e35c	  S65	        1	      Service Management
 * 99619bdd-6452-4850-9244-a4ce9bec70ca	  S63	        0	      Non-Functional Questions
 * 9e7780e9-7263-4ee4-a722-b3e1aaff4476	  S31	        0	      Commercial Standard
 * a5774787-3ab3-48ff-9fa2-6db06be1b926	  S25	        0	      Clinical Safety
 * cd07ffcf-822b-460a-9f94-be42ad9b995b	  S26	        0	      Data Migration
 * cdfdebda-edde-4af4-aaf4-9d59fca7cdaa	  S29	        5	      Hosting & Infrastructure
 * e70fd085-ec42-4901-a7ba-a0d6f61fbbe1	  S24	        0	      Business Continuity & Disaster Recovery
 * f49f91de-64fc-4cca-88bf-3238fb1de69b	  S28	        3	      Training
 */

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

const expectedStatusStandards = {
  'Waiting for TM': new Set([
    standardNameMap['Citizen Access'].id,
    standardNameMap['IM1 - Interface Mechanism'].id,
    standardNameMap['Data Standards'].id,
    standardNameMap['Non-Functional Questions'].id
  ]),
  'Failed': new Set([
    standardNameMap['Interoperability Standard'].id
  ]),
  'Draft': new Set([
    standardNameMap['Service Management'].id
  ]),
  'Not started': new Set([
    standardNameMap['Business Continuity & Disaster Recovery'].id,
    standardNameMap['Clinical Safety'].id,
    standardNameMap['Data Migration'].id,
    standardNameMap['Testing'].id,
    standardNameMap['Commercial Standard'].id
  ]),
  'Submitted': new Set([
    standardNameMap['Information Governance'].id
  ]),
  'Feedback': new Set([
    standardNameMap['Data Migration'].id,
    standardNameMap['Training'].id
  ]),
  'Approved': new Set([
    standardNameMap['Management Information (MI) Reporting'].id,
    standardNameMap['View Record - Citizen - Standard'].id,
    standardNameMap['Hosting & Infrastructure'].id
  ])
}

fixture('Standards Compliance - Dashboard')
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

test('Applicable standards are present and displayed in the correct sections', async t => {
  const assocStd = await standardsComplianceDashboardPage.associatedStandardsTable.find('.standard')
  const ovrchStd = await standardsComplianceDashboardPage.overarchingStandards.find('.standard')

  await t
    .expect(assocStd.count).eql(5)
    .expect(ovrchStd.count).eql(11)
})

test('Displayed Applicable Standards match expected standard\'s status descriptions', async t => {
  const standards = await Selector('.standard')
  const count = await standards.count

  for (let i = 0; i < count; i++) {
    const currentStandard = await standards.nth(i)
    const currentLinkUUID = (
      await currentStandard.find('a').getAttribute('href')
    ).match('evidence/([^/]+)/')[1] // I know Kung Fu (Thanks Paul!)
    const currentStatus = await currentStandard.find('.status').innerText
    await t.expect(expectedStatusStandards[currentStatus].has(currentLinkUUID)).ok()
  }
})
