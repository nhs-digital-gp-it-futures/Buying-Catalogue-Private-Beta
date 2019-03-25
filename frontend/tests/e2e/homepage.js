/* global fixture, test */

//
//  These tests are really to ensure that content wasn't accidentally deleted
//  on the homepage at some point...
//

import { homePage } from './pages'

const HOMEPAGE_TITLE = 'NHS Buying Catalogue'
const LOGIN_TEXT = 'Log in/Sign up'
const NUMBER_OF_CARDS = 3
const NUMBER_OF_STEPS = 4
const NUMBER_OF_CHILDREN_PER_CARD = 1

fixture('Homepage')
  .page(homePage.baseUrl)
  .afterEach(homePage.checkAccessibility)

test('a11y: logged out homepage', async t => {
  await t.expect(homePage.loginLink.innerText).eql(LOGIN_TEXT)
})

test('Home Page has a welcome banner containing the Title of the product', async t => {
  const title = await homePage.welcomeBanner.find('h1')
  await t.expect(title.innerText).contains(HOMEPAGE_TITLE)
})

test('Home Page has 3 cards displaying information', async t => {
  await t.expect(homePage.cardContainer.childElementCount).eql(NUMBER_OF_CARDS)
})

/**
 * Test is being skipped, there are not links for each of the cards yet, meaning
 * that there are placeholder links that all have the same value.
 */
test.skip('Home Page Cards all have different links', async t => {
  const cards = homePage.cardContainer.find('a')

  const cardLinks = []
  for (let i = 0; i < await cards.count; i++) {
    cardLinks.push(await cards.nth(i).getAttribute('href'))
  }
  await t.expect(cardLinks.length).eql(3)
  await t.expect((new Set(cardLinks)).size).eql(cardLinks.length)
})

test('Home Page Cards have have one child that is an anchor', async t => {
  const cards = await homePage.cardContainer.child()

  for (let i = 0; i < await cards.count; i++) {
    const children = await cards.nth(i).child()
    await t
      .expect(children.count).eql(NUMBER_OF_CHILDREN_PER_CARD)
      .expect(children.nth(0).tagName).eql('a')
  }
})

test('Homepage have 4 steps displaying information', async t => {
  await t.expect(homePage.stepsContainer.childElementCount).eql(NUMBER_OF_STEPS)
})
