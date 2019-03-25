/* global fixture, test */

import { Selector } from 'testcafe'

import { asSupplier } from './roles'
import { supplierDashboardPage, onboardingDashboardPage, registrationPage } from './pages'

fixture('Solution Registration - Details')
  .page(supplierDashboardPage.baseUrl)
  .beforeEach(navigateToSupplierOnboardingSolution)
  .afterEach(supplierDashboardPage.checkAccessibility)

function navigateToSupplierOnboardingSolution (t) {
  return asSupplier(t)
    .click(supplierDashboardPage.firstOnboardingSolutionName)
    .click(onboardingDashboardPage.continueRegistrationButton)
}

test('Registration page shows correct information accessibly', async t => {
  await t
    .expect(registrationPage.solutionNameInput.value).eql('Really Kool Document Manager')
    .expect(registrationPage.solutionNameCounter.textContent).contains(' 32 characters remaining (out of 60)')
    .expect(registrationPage.solutionDescriptionInput.value).eql('Does Really Kool document management')
    .expect(registrationPage.solutionDescriptionCounter.textContent).contains('264 characters remaining (out of 300)')
    .expect(registrationPage.solutionVersionInput.value).eql('1')
    .expect(Selector('#content [readonly]').count).eql(0)
})

test('Registration page validation is correct and accessible', async t => {
  await t
    .selectText(registrationPage.solutionNameInput).pressKey('backspace')
    .selectText(registrationPage.solutionDescriptionInput).pressKey('backspace')
    .click(registrationPage.continueButton)

    .expect(Selector('#errors #error-solution\\.name').innerText).contains('Solution name is missing')
    .expect(Selector('#errors #error-solution\\.description').innerText).contains('Summary description is missing')
    .expect(registrationPage.solutionNameInput.parent('.control.invalid').child('.action').textContent).contains('Please enter a Solution name')
    .expect(registrationPage.solutionDescriptionInput.parent('.control.invalid').child('.action').textContent).contains('Please enter a Summary description')
})

test('Solution name shows in top bar and updates correctly when saved', async t => {
  await t
    .expect(registrationPage.globalSolutionName.textContent).eql('Really Kool Document Manager, 1')

  await t
    .selectText(registrationPage.solutionNameInput)
    .typeText(registrationPage.solutionNameInput, 'Really Really Kool Document Manager')
    .selectText(registrationPage.solutionVersionInput).pressKey('backspace')
    .click(registrationPage.globalSaveButton)
    .expect(registrationPage.solutionNameInput.value).eql('Really Really Kool Document Manager')
    .expect(registrationPage.solutionVersionInput.value).eql('')
    .expect(registrationPage.globalSolutionName.textContent).eql('Really Really Kool Document Manager')

  await t
    .selectText(registrationPage.solutionNameInput)
    .typeText(registrationPage.solutionNameInput, 'Really Kool Document Manager')
    .selectText(registrationPage.solutionVersionInput)
    .typeText(registrationPage.solutionVersionInput, '1')
    .click(registrationPage.globalSaveAndExitButton)
    .click(supplierDashboardPage.firstOnboardingSolutionName)
    .click(onboardingDashboardPage.continueRegistrationButton)
    .expect(registrationPage.globalSolutionName.textContent).eql('Really Kool Document Manager, 1')
})

test('Global save buttons trigger validation and does not update top bar', async t => {
  await t
    .selectText(registrationPage.solutionNameInput).pressKey('backspace')
    .selectText(registrationPage.solutionDescriptionInput).pressKey('backspace')
    .click(registrationPage.globalSaveButton)

    .expect(Selector('#errors #error-solution\\.name').innerText).contains('Solution name is missing')
    .expect(registrationPage.globalSolutionName.textContent).eql('Really Kool Document Manager, 1')

  await t
    .click(registrationPage.globalSaveAndExitButton)
    .expect(Selector('#errors #error-solution\\.description').innerText).contains('Summary description is missing')
    .expect(registrationPage.globalSolutionName.textContent).eql('Really Kool Document Manager, 1')
})

test('Lead contact details can be changed and saved', async t => {
  await t
    .click(registrationPage.leadContactFieldset)

    .expect(registrationPage.leadContactFirstNameInput.value).eql('Helpma')
    .expect(registrationPage.leadContactLastNameInput.value).eql('Boab')
    .expect(registrationPage.leadContactEmailInput.value).eql('helpma.boab@example.com')
    .expect(registrationPage.leadContactPhoneInput.value).eql('N/A')

  await t
    .selectText(registrationPage.leadContactFirstNameInput)
    .typeText(registrationPage.leadContactFirstNameInput, 'Automated')
    .selectText(registrationPage.leadContactLastNameInput)
    .typeText(registrationPage.leadContactLastNameInput, 'Testing')
    .selectText(registrationPage.leadContactEmailInput)
    .typeText(registrationPage.leadContactEmailInput, 'autotest@example.com')
    .selectText(registrationPage.leadContactPhoneInput)
    .typeText(registrationPage.leadContactPhoneInput, '123 456 78910')
    .click(registrationPage.globalSaveAndExitButton)

  await navigateToSupplierOnboardingSolution(t)
    .click(registrationPage.leadContactFieldset)

    .expect(registrationPage.leadContactFirstNameInput.value).eql('Automated')
    .expect(registrationPage.leadContactLastNameInput.value).eql('Testing')
    .expect(registrationPage.leadContactEmailInput.value).eql('autotest@example.com')
    .expect(registrationPage.leadContactPhoneInput.value).eql('123 456 78910')

  await t
    .selectText(registrationPage.leadContactFirstNameInput)
    .typeText(registrationPage.leadContactFirstNameInput, 'Helpma')
    .selectText(registrationPage.leadContactLastNameInput)
    .typeText(registrationPage.leadContactLastNameInput, 'Boab')
    .selectText(registrationPage.leadContactEmailInput)
    .typeText(registrationPage.leadContactEmailInput, 'helpma.boab@example.com')
    .selectText(registrationPage.leadContactPhoneInput)
    .typeText(registrationPage.leadContactPhoneInput, 'N/A')
    .click(registrationPage.globalSaveAndExitButton)

  await navigateToSupplierOnboardingSolution(t)
    .click(registrationPage.leadContactFieldset)

    .expect(registrationPage.leadContactFirstNameInput.value).eql('Helpma')
    .expect(registrationPage.leadContactLastNameInput.value).eql('Boab')
    .expect(registrationPage.leadContactEmailInput.value).eql('helpma.boab@example.com')
    .expect(registrationPage.leadContactPhoneInput.value).eql('N/A')
})

test('Blanking any and all Lead Contact fields triggers validation', async t => {
  await t
    .click(registrationPage.leadContactFieldset)
    .selectText(registrationPage.leadContactFirstNameInput).pressKey('backspace')
    .click(registrationPage.globalSaveButton)

    .expect(Selector('#errors #error-solution\\.contacts\\[0\\]\\.firstName').textContent).contains('Contact first name is missing')

  await t
    .selectText(registrationPage.leadContactLastNameInput).pressKey('backspace')
    .selectText(registrationPage.leadContactEmailInput).pressKey('backspace')
    .selectText(registrationPage.leadContactPhoneInput).pressKey('backspace')
    .click(registrationPage.globalSaveButton)

    .expect(Selector('#errors #error-solution\\.contacts\\[0\\]\\.firstName').textContent).contains('Contact first name is missing')
    .expect(Selector('#errors #error-solution\\.contacts\\[0\\]\\.lastName').textContent).contains('Contact last name is missing')
    .expect(Selector('#errors #error-solution\\.contacts\\[0\\]\\.emailAddress').textContent).contains('Contact email is missing')
    .expect(Selector('#errors #error-solution\\.contacts\\[0\\]\\.phoneNumber').textContent).contains('Contact phone number is missing')
})

test('Creating a new contact, leaving it empty and saving removes it', async t => {
  await t
    .expect(registrationPage.newContactFieldset.exists).notOk()

  await t
    .click(registrationPage.addNewContactButton)
    .click(registrationPage.globalSaveButton)

    .expect(Selector('#errors').exists).notOk()
    .expect(registrationPage.newContactFieldset.exists).notOk()
})

test('Creating a new contact requires all fields to be filled if any are', async t => {
  await t
    .expect(registrationPage.newContactFieldset.exists).notOk()

  await t
    .click(registrationPage.addNewContactButton)
    .typeText(registrationPage.newContactContactTypeInput, 'Clinical Safety Unicorn')
    .click(registrationPage.globalSaveButton)

    .expect(Selector('#errors #error-solution\\.contacts\\[1\\]\\.firstName').textContent).contains('Contact first name is missing')
    .expect(Selector('#errors #error-solution\\.contacts\\[1\\]\\.lastName').textContent).contains('Contact last name is missing')
    .expect(Selector('#errors #error-solution\\.contacts\\[1\\]\\.emailAddress').textContent).contains('Contact email is missing')
    .expect(Selector('#errors #error-solution\\.contacts\\[1\\]\\.phoneNumber').textContent).contains('Contact phone number is missing')

  await t
    .typeText(registrationPage.newContactFirstNameInput, 'Zyra')
    .typeText(registrationPage.newContactLastNameInput, 'Smith Fotheringham-Shaw')
    .typeText(registrationPage.newContactEmailInput, 'safety.unicorn@example.com')
    .typeText(registrationPage.newContactPhoneInput, '555 123 4567')
    .click(registrationPage.globalSaveAndExitButton)

  await navigateToSupplierOnboardingSolution(t)
    .click(registrationPage.newContactFieldset)

    .expect(registrationPage.newContactContactTypeInput.value).eql('Clinical Safety Unicorn')
    .expect(registrationPage.newContactFirstNameInput.value).eql('Zyra')
    .expect(registrationPage.newContactLastNameInput.value).eql('Smith Fotheringham-Shaw')
    .expect(registrationPage.newContactEmailInput.value).eql('safety.unicorn@example.com')
    .expect(registrationPage.newContactPhoneInput.value).eql('555 123 4567')
})

test('New contact details can be changed and saved', async t => {
  await t
    .click(registrationPage.newContactFieldset)
    .selectText(registrationPage.newContactContactTypeInput)
    .typeText(registrationPage.newContactContactTypeInput, 'Chief Safety Unicorn')
    .selectText(registrationPage.newContactEmailInput)
    .typeText(registrationPage.newContactEmailInput, 'chief.safety.unicorn@example.com')
    .click(registrationPage.globalSaveAndExitButton)

  await navigateToSupplierOnboardingSolution(t)
    .click(registrationPage.newContactFieldset)

    .expect(registrationPage.newContactContactTypeInput.value).eql('Chief Safety Unicorn')
    .expect(registrationPage.newContactFirstNameInput.value).eql('Zyra')
    .expect(registrationPage.newContactLastNameInput.value).eql('Smith Fotheringham-Shaw')
    .expect(registrationPage.newContactEmailInput.value).eql('chief.safety.unicorn@example.com')
    .expect(registrationPage.newContactPhoneInput.value).eql('555 123 4567')
})

test('Blanking some optional contact fields triggers validation', async t => {
  await t
    .click(registrationPage.newContactFieldset)
    .selectText(registrationPage.newContactContactTypeInput).pressKey('backspace')
    .selectText(registrationPage.newContactEmailInput).pressKey('backspace')
    .click(registrationPage.globalSaveButton)

    .expect(Selector('#errors #error-solution\\.contacts\\[1\\]\\.contactType').textContent).contains('Contact type is missing')
    .expect(Selector('#errors #error-solution\\.contacts\\[1\\]\\.emailAddress').textContent).contains('Contact email is missing')
})

test('Blanking all optional contact fields removes the contact', async t => {
  await t
    .click(registrationPage.newContactFieldset)
    .selectText(registrationPage.newContactContactTypeInput).pressKey('backspace')
    .selectText(registrationPage.newContactFirstNameInput).pressKey('backspace')
    .selectText(registrationPage.newContactLastNameInput).pressKey('backspace')
    .selectText(registrationPage.newContactEmailInput).pressKey('backspace')
    .selectText(registrationPage.newContactPhoneInput).pressKey('backspace')
    .click(registrationPage.globalSaveButton)

    .expect(Selector('#errors').exists).notOk()
    .expect(registrationPage.newContactFieldset.exists).notOk()
})
