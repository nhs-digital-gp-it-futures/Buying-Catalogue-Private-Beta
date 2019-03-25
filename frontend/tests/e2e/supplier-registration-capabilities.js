/* global fixture, test */

import { Selector } from 'testcafe'

import { asSupplier } from './roles'
import { page, supplierDashboardPage, onboardingDashboardPage, registrationPage } from './pages'

fixture('Solution Registration - Capabilities')
  .page(supplierDashboardPage.baseUrl)
  .beforeEach(navigateToSupplierOnboardingSolutionCapabilities)
  .afterEach(supplierDashboardPage.checkAccessibility)

function navigateToSupplierOnboardingSolutionCapabilities (t) {
  return asSupplier(t)
    .click(supplierDashboardPage.homeLink)
    .click(supplierDashboardPage.firstOnboardingSolutionName)
    .click(onboardingDashboardPage.continueRegistrationButton)
    .click(registrationPage.continueButton)
    .expect(Selector('#capability-selector').exists).ok()
}

test('Capabilities page shows correct information accessibly', async t => {
  const allCoreCapNames = Selector('#capabilities-core .capability .name')
  const allNonCoreCapNames = Selector('#capabilities-non-core .capability .name')

  await t

    .expect(Selector('#capabilities-core .capability').count).eql(6)
    .expect(Selector('#capabilities-non-core .capability').count).eql(35)

    .expect(allCoreCapNames.nth(0).innerText).eql('Appointments Management - GP')
    .expect(allCoreCapNames.nth(3).innerText).eql('Recording Consultations')
    .expect(allCoreCapNames.nth(5).innerText).eql('Resource Management')

    .expect(allNonCoreCapNames.nth(0).innerText).eql('Appointments Management - Citizen')
    .expect(allNonCoreCapNames.nth(13).innerText).eql('Domiciliary Care')
    .expect(allNonCoreCapNames.nth(34).innerText).eql('Workflow')

    .expect(Selector('#capability-selector .capability[data-cap-id="C4"].selected')).ok()
})

test('Capabilities page validation is correct and accessible', async t => {
  const selectedCapabilities = Selector('#capability-selector .capability.selected')
  const selectedCapabilitiesCount = await selectedCapabilities.count
  for (let i = 0; i < selectedCapabilitiesCount; i++) {
    await t
      .click(selectedCapabilities.nth(i))
      .click(selectedCapabilities.nth(i).find('input:checked'))
  }

  await t
    .expect(Selector('#capability-selector .capability.revealed').exists).notOk()

    .click(page.continueButton)
    .expect(Selector('#errors #error-capabilities').innerText).contains('Select at least one capability to continue')
    .expect(selectedCapabilities.exists).notOk('No capabilities should be selected after reload')
})

test('Capabilities can be changed, summary updates and data save correctly', async t => {
  const capabilityCount = Selector('#capability-summary .capability-count').innerText
  const standardCount = Selector('#capability-summary .standard-count').innerText
  const selSelectedCapabilities = Selector('#capability-summary .capabilities [data-id].selected')
  const selSelectedStandards = Selector('#capability-summary .standards [data-id].selected')

  await t

    .click('#capability-selector .capability[data-cap-id="C4"].selected')
    .click('[type=checkbox][data-id="C4"]')

    .expect(capabilityCount).eql('0 Capabilities selected')
    .expect(standardCount).eql('11 Standards will be required')
    .expect(Selector('#capability-summary .standards .associated').visible).notOk()

    .click('#capability-selector .capability[data-cap-id="C1"]:not(.selected)')
    .click('[type=checkbox][data-id="C1"]')

    .click('#capability-selector .capability[data-cap-id="C37"]:not(.selected)')
    .click('[type=checkbox][data-id="C37"]')

    .click('#capability-selector .capability[data-cap-id="C20"]:not(.selected)')
    .click('[type=checkbox][data-id="C20"]')

    .expect(capabilityCount).eql('3 Capabilities selected')
    .expect(standardCount).eql('19 Standards will be required')
    .expect(Selector('#capability-summary .standards .associated').visible).ok()

    .expect(selSelectedCapabilities.nth(0).textContent).eql('Appointments Management - Citizen')
    .expect(selSelectedCapabilities.nth(1).textContent).eql('Social Prescribing')
    .expect(selSelectedCapabilities.nth(2).textContent).eql('Workflow')

    .expect(selSelectedStandards.nth(0).textContent).eql('Appointments Management - Citizen - Standard')
    .expect(selSelectedStandards.nth(1).textContent).eql('Citizen Access')
    .expect(selSelectedStandards.nth(2).textContent).eql('Common Reporting')
    .expect(selSelectedStandards.nth(4).textContent).eql('Interoperability Standard')
    .expect(selSelectedStandards.nth(17).textContent).eql('Testing')

    .click(page.globalSaveButton)

  await t.expect(registrationPage.savedNoticeTitle.innerText).contains('Saved')

  await t
    .expect(Selector('#errors').exists).notOk()
    .expect(capabilityCount).eql('3 Capabilities selected')
    .expect(standardCount).eql('19 Standards will be required')

    .click('#capability-selector .capability[data-cap-id="C1"].selected')
    .click('[type=checkbox][data-id="C1"]')

    .click('#capability-selector .capability[data-cap-id="C37"].selected')
    .click('[type=checkbox][data-id="C37"]')

    .click('#capability-selector .capability[data-cap-id="C20"].selected')
    .click('[type=checkbox][data-id="C20"]')

    .expect(capabilityCount).eql('0 Capabilities selected')
    .expect(standardCount).eql('11 Standards will be required')
    .expect(Selector('#capability-summary .standards .associated').visible).notOk()

    .click('#capability-selector .capability[data-cap-id="C4"]:not(.selected)')
    .click('[type=checkbox][data-id="C4"]')

    .expect(capabilityCount).eql('1 Capability selected')
    .expect(standardCount).eql('17 Standards will be required')
    .expect(Selector('#capability-summary .standards .associated').visible).ok()

    .click(page.globalSaveAndExitButton)

    .expect(Selector('#errors').exists).notOk()
})

test('Registering the solution shows a confirmation message and status change on dashboard', async t => {
  await t
    .click(page.continueButton)
    .expect(Selector('#onboarding.dashboard.page .callout .title').textContent).eql('Solution details entered.')
    .expect(Selector('.onboarding-stages :first-child.complete').exists).ok()
})

test('A registered solution cannot have its name edited', async t => {
  await t
    .click('#content a.back-link')
    .expect(Selector('#content [readonly]').count).eql(2)
    .expect(registrationPage.solutionNameInput.hasAttribute('readonly')).ok()
    .expect(registrationPage.solutionNameCounter.exists).notOk()
})

test('A registered solution cannot have its version edited', async t => {
  await t
    .click('#content a.back-link')
    .expect(Selector('#content [readonly]').count).eql(2)
    .expect(registrationPage.solutionVersionInput.hasAttribute('readonly')).ok()
})
