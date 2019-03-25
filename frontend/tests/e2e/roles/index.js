import { Role } from 'testcafe'

const supplier = Role('http://localhost:3000/oidc/authenticate', async t => {
  await t
    .typeText('[name=email]', 'drkool@kool.com')
    .typeText('[name=password]', 'kool')
    .click('[name=submit]')
})

export function asSupplier (t) {
  return t.useRole(supplier)
}
