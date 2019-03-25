/* eslint-env jest */

let subject

beforeEach(() => {
  subject = require('./fixtures').mockApi()
})

describe('solutionForCompliance', () => {
  it('adds the extra properties to the SolutionEx standards correctly', async () => {
    subject.contactsApi.apiContactsByOrganisationByOrganisationIdGet.mockReturnValue({
      items: [
        { id: 'testOwner1', firstName: 'Zyra', lastName: 'Featherstonhaugh' }
      ]
    })

    subject.solutionsExApi.apiPorcelainSolutionsExBySolutionBySolutionIdGet.mockReturnValue({
      solution: {
        id: 'testid',
        name: 'testname'
      },
      claimedCapability: [
        { claimedCapabilityId: 'testCCId' }
      ],
      claimedStandard: [
        { status: 0, claimedStandardId: 'testCSId' },
        { status: 1, claimedStandardId: 'testCSId1', ownerId: 'testOwner1' },
        { status: 2, claimedStandardId: 'testCSId2' },
        { status: 3, claimedStandardId: 'testCSId3' },
        { status: 4, claimedStandardId: 'testCSId4' },
        { status: 7, claimedStandardId: 'testCSId7' }
      ],
      technicalContact: [
        { contactId: 'testCId3', contactType: '3rd type', firstName: 'Helpma', lastName: 'Boab' },
        { contactId: 'testCId1', contactType: 'Lead Contact', firstName: '🦄', lastName: 'Rainbow' },
        { contactId: 'testCId2', contactType: '2nd type' }
      ]
    })

    await expect(subject.solutionForCompliance('testId')).resolves.toMatchObject({
      id: 'testid',
      name: 'testname',
      capabilities: [
        { claimedCapabilityId: 'testCCId' }
      ],
      standards: [
        {
          status: 0,
          claimedStandardId: 'testCSId',
          statusClass: 'not-started',
          ownerContact: { displayName: '🦄 Rainbow' },
          withContact: { displayName: '🦄 Rainbow' }
        },
        {
          status: 1,
          claimedStandardId: 'testCSId1',
          statusClass: 'draft',
          ownerContact: { displayName: 'Zyra Featherstonhaugh' },
          withContact: { displayName: 'Zyra Featherstonhaugh' }
        },
        {
          status: 2,
          claimedStandardId: 'testCSId2',
          statusClass: 'submitted',
          ownerContact: { displayName: '🦄 Rainbow' },
          withContact: { displayName: 'NHS Digital' }
        },
        {
          status: 3,
          claimedStandardId: 'testCSId3',
          statusClass: 'remediation',
          ownerContact: { displayName: '🦄 Rainbow' },
          withContact: { displayName: '🦄 Rainbow' }
        },
        {
          status: 4,
          claimedStandardId: 'testCSId4',
          statusClass: 'approved',
          ownerContact: { displayName: '🦄 Rainbow' },
          withContact: { displayName: '🦄 Rainbow' }
        },
        {
          status: 7,
          claimedStandardId: 'testCSId7',
          statusClass: 'rejected',
          ownerContact: { displayName: '🦄 Rainbow' },
          withContact: { displayName: '🦄 Rainbow' }
        }
      ],
      contacts: [
        { contactId: 'testCId1', contactType: 'Lead Contact' },
        { contactId: 'testCId2', contactType: '2nd type' },
        { contactId: 'testCId3', contactType: '3rd type' }
      ]
    })
  })
})
