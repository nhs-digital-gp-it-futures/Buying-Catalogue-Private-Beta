/* eslint-env jest */

let subject

beforeEach(() => {
  subject = require('./fixtures').mockApi()
})

describe('contactByEmail', () => {
  it('throws if no contact exists with the specified email address', async () => {
    await expect(subject.contactByEmail('bogus')).rejects.toThrowError('No contact found')
    expect(subject.contactsApi.apiContactsByEmailByEmailGet.mock.calls.length).toBe(1)
  })

  it('throws if no organisation can be found for the contact', async () => {
    subject.contactsApi.apiContactsByEmailByEmailGet.mockReturnValue({
      id: 'bogus'
    })
    await expect(subject.contactByEmail('supplier@test.com')).rejects.toThrowError('No organisation found')
    expect(subject.orgsApi.apiOrganisationsByContactByContactIdGet.mock.calls.length).toBe(1)
  })

  it('returns the contact and organisation with supplier flag set correctly', async () => {
    const testContact = {
      id: 'testContact',
      email: 'supplier@test'
    }

    const testSupplierOrg = {
      id: 'testSupplier',
      primaryRoleId: 'RO92'
    }

    const testNonSupplierOrg = {
      id: 'testNonSupplier'
    }

    subject.contactsApi.apiContactsByEmailByEmailGet.mockReturnValue(testContact)
    subject.orgsApi.apiOrganisationsByContactByContactIdGet
      .mockReturnValueOnce(testSupplierOrg)
      .mockReturnValue(testNonSupplierOrg)

    await expect(subject.contactByEmail('supplier@test')).resolves.toMatchObject({
      contact: {
        id: 'testContact',
        email: 'supplier@test'
      },
      org: {
        id: 'testSupplier',
        isSupplier: true
      }
    })

    await expect(subject.contactByEmail('non-supplier@test')).resolves.toMatchObject({
      contact: {
        id: 'testContact',
        email: 'supplier@test'
      },
      org: {
        id: 'testNonSupplier',
        isSupplier: false
      }
    })
  })
})
