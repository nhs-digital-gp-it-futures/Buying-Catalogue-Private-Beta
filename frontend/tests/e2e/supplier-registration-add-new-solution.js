/* global fixture, test */

import { Selector } from 'testcafe'

import { asSupplier } from './roles'
import { page, supplierDashboardPage, onboardingDashboardPage, registrationPage } from './pages'

fixture('Solution Registration - Add New Solution')
  .page(supplierDashboardPage.baseUrl)
  .beforeEach(async t => {
    return asSupplier(t)
      .click(supplierDashboardPage.homeLink)
  })
  .afterEach(supplierDashboardPage.checkAccessibility)

test('Creating a new solution leads to an empty form via a customised status page', async t => {
  await t
    .click(supplierDashboardPage.addNewSolutionButton)

    .expect(onboardingDashboardPage.continueRegistrationButton.textContent).eql('Start')

    .click(onboardingDashboardPage.continueRegistrationButton)
    .click(registrationPage.leadContactFieldset)

    .expect(page.globalSolutionName.exists).notOk()
    .expect(registrationPage.solutionNameInput.value).eql('')
    .expect(registrationPage.solutionNameCounter.textContent).contains(' 60 characters remaining (out of 60)')
    .expect(registrationPage.solutionDescriptionInput.value).eql('')
    .expect(registrationPage.solutionDescriptionCounter.textContent).contains(' 300 characters remaining (out of 300)')
    .expect(registrationPage.solutionVersionInput.value).eql('')
    .expect(registrationPage.leadContactFirstNameInput.value).eql('')
    .expect(registrationPage.leadContactLastNameInput.value).eql('')
    .expect(registrationPage.leadContactEmailInput.value).eql('')
    .expect(registrationPage.leadContactPhoneInput.value).eql('')
})

test('Multiple, empty additional contacts are removed', async t => {
  await t
    .click(supplierDashboardPage.addNewSolutionButton)
    .expect(onboardingDashboardPage.continueRegistrationButton.textContent).eql('Start')

    .click(onboardingDashboardPage.continueRegistrationButton)
    .click(registrationPage.addNewContactButton)
    .click(registrationPage.addNewContactButton)
    .click(registrationPage.globalSaveButton)

    .expect(Selector('#errors').exists).ok()
    .expect(registrationPage.newContactFieldset.exists).notOk()
})

test('Solution cannot have the same name and version as another existing solution', async t => {
  await t
    .click(supplierDashboardPage.addNewSolutionButton)
    .click(onboardingDashboardPage.continueRegistrationButton)

    .typeText(registrationPage.solutionNameInput, 'Really Kool Document Manager')
    .typeText(registrationPage.solutionDescriptionInput, 'This is the koolest kore system')
    .typeText(registrationPage.solutionVersionInput, '1')

    .click(registrationPage.leadContactFieldset)
    .typeText(registrationPage.leadContactFirstNameInput, 'Automated')
    .typeText(registrationPage.leadContactLastNameInput, 'Testing')
    .typeText(registrationPage.leadContactEmailInput, 'autotest@example.com')
    .typeText(registrationPage.leadContactPhoneInput, '123 456 78910')

    .click(page.globalSaveButton)

    .expect(Selector('#errors #error-solution\\.name').textContent).contains('Solution name and version already exists')

    .selectText(registrationPage.solutionNameInput)
    .typeText(registrationPage.solutionNameInput, 'Really Kool Kore System')

    .click(page.globalSaveButton)

    .expect(Selector('#errors').exists).notOk()
    .expect(page.globalSolutionName.textContent).eql('Really Kool Kore System, 1')
})

test('Solution description character counting is correct, consistent and gramattical', async t => {
  const longDescription = `This is the koolest kore system!

Multiple lines are supported.

  Indents
  are
  preserved

01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789

AB`

  await t
    .click(supplierDashboardPage.secondOnboardingSolutionName)
    .click(onboardingDashboardPage.continueRegistrationButton)

    .typeText(
      registrationPage.solutionDescriptionInput,
      longDescription,
      { replace: true, paste: true }
    )
    .expect(registrationPage.solutionDescriptionCounter.textContent).contains(' 2 characters remaining (out of 300)')

  await t
    .typeText(registrationPage.solutionDescriptionInput, 'C')
    .expect(registrationPage.solutionDescriptionCounter.textContent).contains(' 1 character remaining (out of 300)')

  await t
    .typeText(registrationPage.solutionDescriptionInput, 'D')
    .expect(registrationPage.solutionDescriptionCounter.textContent).contains(' 0 characters remaining (out of 300)')

  await t
    .typeText(registrationPage.solutionDescriptionInput, 'E')
    .expect(registrationPage.solutionDescriptionCounter.textContent).contains(' 1 character too many (out of 300)')

  await t
    .typeText(registrationPage.solutionDescriptionInput, 'F')
    .expect(registrationPage.solutionDescriptionCounter.textContent).contains(' 2 characters too many (out of 300)')

  await t
    .pressKey('backspace backspace')
    .expect(registrationPage.solutionDescriptionCounter.textContent).contains(' 0 characters remaining (out of 300)')

  await t
    .click(page.globalSaveButton)

    .expect(Selector('#errors').exists).notOk()
    .expect(registrationPage.solutionDescriptionCounter.textContent).contains(' 0 characters remaining (out of 300)')
    .expect(registrationPage.solutionDescriptionInput.value).eql(`${longDescription}CD`)
})

test('Newly created solution appears in the correct place on the supplier dashboard', async t => {
  await t
    .expect(supplierDashboardPage.secondOnboardingSolutionName.textContent).eql('Really Kool Kore System | 1')
})

test('Attempting to leave with unsaved changes triggers an alert', async t => {
  // Navigate to an in-progress solution.
  await t
    .click(supplierDashboardPage.secondOnboardingSolutionName)
    .click(onboardingDashboardPage.continueRegistrationButton)

    // Edit the text
    .typeText(registrationPage.solutionNameInput, 'ch-ch-ch-ch-ch-changes')

    // Click breadcrumb to leave...
    .click(registrationPage.breadcrumb)

    // The warning banner should be there now.
    .expect(supplierDashboardPage.unsavedChangesBanner).ok()
})

test('continuing without saving with unsaved changes lets you continue and does not save', async t => {
  // Navigate to an in-progress solution.
  await t
    .click(supplierDashboardPage.secondOnboardingSolutionName)
    .click(onboardingDashboardPage.continueRegistrationButton)

    // Edit the text
    .typeText(registrationPage.solutionNameInput, 'ch-ch-ch-ch-ch-changes')

    // click the Homepage anchor...
    .click(supplierDashboardPage.homeLink)

    // click the continue without saving button
    .click(supplierDashboardPage.continueWithoutSavingButton)

    // You should be on the dashboard now with the original solution and name.
    .expect(supplierDashboardPage.secondOnboardingSolutionName.textContent).eql('Really Kool Kore System | 1')
})
