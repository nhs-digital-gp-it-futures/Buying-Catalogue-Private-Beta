/* global fixture, test */

import { Selector } from 'testcafe'

import { asSupplier } from './roles'
import { supplierDashboardPage } from './pages'

fixture('Supplier Homepage')
  .page(supplierDashboardPage.baseUrl)
  .afterEach(supplierDashboardPage.checkAccessibility)

test('Login as supplier', async t => {
  await asSupplier(t)
    .expect(Selector('#account .user').innerText).contains('Welcome, Dr')
})

test('Clicking logo returns to supplier homepage', async t => {
  await asSupplier(t)
    .click('body > header a[href^="/about"]')
    .click(supplierDashboardPage.homeLink)

    .expect(Selector('#my-solutions-page > h1').innerText).eql('My Solutions')
})

test('Solutions that are currently onboarding are listed', async t => {
  await asSupplier(t)
    .expect(supplierDashboardPage.firstOnboardingSolutionName.textContent).eql('Really Kool Document Manager | 1')
})
