/* eslint-env jest */

let subject

beforeEach(() => {
  subject = require('./fixtures').mockApi()
})

describe('solutionForRegistration', () => {
  // I've elided the unhappy path test here as whatever is thrown is decided
  // by the API layer - generally an Error('Not Found')

  it('transforms the returned SolutionEx correctly', async () => {
    subject.solutionsExApi.apiPorcelainSolutionsExBySolutionBySolutionIdGet.mockReturnValue({
      solution: {
        id: 'testid',
        name: 'testname'
      },
      claimedCapability: [
        { claimedCapabilityId: 'testCCId' }
      ],
      claimedStandard: [
        { claimedStandardId: 'testCSId' }
      ],
      technicalContact: [
        { contactId: 'testCId3', contactType: '3rd type' },
        { contactId: 'testCId1', contactType: 'Lead Contact' },
        { contactId: 'testCId2', contactType: '2nd type' }
      ]
    })

    await expect(subject.solutionForRegistration('testId')).resolves.toMatchObject({
      id: 'testid',
      name: 'testname',
      capabilities: [
        { claimedCapabilityId: 'testCCId' }
      ],
      standards: [
        { claimedStandardId: 'testCSId' }
      ],
      contacts: [
        { contactId: 'testCId1', contactType: 'Lead Contact' },
        { contactId: 'testCId2', contactType: '2nd type' },
        { contactId: 'testCId3', contactType: '3rd type' }
      ]
    })
  })
})

describe('updateSolutionForRegistration', () => {
  beforeEach(() => {
    subject.solutionsExApi.apiPorcelainSolutionsExBySolutionBySolutionIdGet.mockReturnValue({
      solution: {},
      claimedCapability: [
        { id: 'testCId1', capabilityId: 'CAP1' },
        { id: 'testCId2', capabilityId: 'CAP2' }
      ],
      claimedStandard: [
        { id: 'testSId1', standardId: 'STD1' },
        { id: 'testSId2', standardId: 'STD2' }
      ],
      technicalContact: [],
      claimedCapabilityEvidence: [
        { id: 'cce1', claimId: 'testCId1' },
        { id: 'cce2', claimId: 'testCId2' }
      ],
      claimedCapabilityReview: [
        { id: 'ccr1', evidenceId: 'cce1' },
        { id: 'ccr2', evidenceId: 'cce2' }
      ],
      claimedStandardEvidence: [
        { id: 'cse1', claimId: 'testSId1' },
        { id: 'cse2', claimId: 'testSId2' }
      ],
      claimedStandardReview: [
        { id: 'csr1', evidenceId: 'cse1' },
        { id: 'csr2', evidenceId: 'cse2' }
      ]
    })
  })

  it('should reformat input into a SolutionEx and send to API', async () => {
    const input = {
      id: 'testid',
      name: 'testname',
      capabilities: [
        { id: 'testCCId', capabilityId: 'C1' }
      ],
      standards: [
        { id: 'testCSId', standardId: 'S1' }
      ],
      contacts: [
        { id: '1234', contactId: 'testCId1', contactType: 'Lead Contact' },
        { id: '4567', contactId: 'testCId2', contactType: '2nd type' },
        { id: '8910', contactId: 'testCId3', contactType: '3rd type' }
      ]
    }

    await subject.updateSolutionForRegistration(input)
    expect(subject.solutionsExApi.apiPorcelainSolutionsExUpdatePut.mock.calls.length).toBe(1)
    expect(subject.solutionsExApi.apiPorcelainSolutionsExUpdatePut.mock.calls[0][0]).toMatchObject({
      solnEx: {
        solution: {
          id: 'testid',
          name: 'testname'
        },
        claimedCapability: [
          { id: 'testCCId', capabilityId: 'C1' }
        ],
        claimedStandard: [
          { id: 'testCSId', standardId: 'S1' }
        ],
        technicalContact: [
          { id: '1234', contactId: 'testCId1', contactType: 'Lead Contact' },
          { id: '4567', contactId: 'testCId2', contactType: '2nd type' },
          { id: '8910', contactId: 'testCId3', contactType: '3rd type' }
        ]
      }
    })
  })

  it('should set correct dummy IDs for new technical contact entitites', async () => {
    const input = {
      id: 'testid',
      name: 'testname',
      contacts: [
        { id: '1234', contactId: 'testCId1', contactType: 'Lead Contact' },
        { contactId: 'testCId2', contactType: '2nd type' },
        { id: '8910', contactId: 'testCId3', contactType: '3rd type' }
      ]
    }

    await subject.updateSolutionForRegistration(input)
    expect(subject.solutionsExApi.apiPorcelainSolutionsExUpdatePut.mock.calls.length).toBe(1)
    expect(subject.solutionsExApi.apiPorcelainSolutionsExUpdatePut.mock.calls[0][0]).toMatchObject({
      solnEx: {
        solution: {
          id: 'testid',
          name: 'testname'
        },
        technicalContact: [
          { id: '1234', contactId: 'testCId1', contactType: 'Lead Contact' },
          { id: expect.stringMatching(/^[0-9A-F]{8}-[0-9A-F]{4}-[4][0-9A-F]{3}-[89AB][0-9A-F]{3}-[0-9A-F]{12}$/i), contactId: 'testCId2', contactType: '2nd type' },
          { id: '8910', contactId: 'testCId3', contactType: '3rd type' }
        ]
      }
    })
  })

  it('should preserve existing claimed standard with the same standard ID', async () => {
    const input = {
      id: 'testid',
      name: 'testname',
      standards: [
        { id: 'testSId2', standardId: 'STD2' },
        { id: 'newstd', standardId: 'STD1' }
      ]
    }

    await subject.updateSolutionForRegistration(input)
    expect(subject.solutionsExApi.apiPorcelainSolutionsExUpdatePut.mock.calls.length).toBe(1)

    const result = subject.solutionsExApi.apiPorcelainSolutionsExUpdatePut.mock.calls[0][0]
    expect(result).toMatchObject({
      solnEx: {
        solution: {
          id: 'testid',
          name: 'testname'
        },
        claimedStandard: [
          { id: 'testSId2', standardId: 'STD2' },
          { id: 'testSId1', standardId: 'STD1' }
        ]
      }
    })
  })

  it('should cascade delete evidence and reviews for removed capabilities and standards', async () => {
    const input = {
      id: 'testid',
      name: 'testname',
      capabilities: [
        { id: 'newcap', capabilityId: 'CAP6' },
        { id: 'testCId1', capabilityId: 'CAP1' }
      ],
      standards: [
        { id: 'testSId2', standardId: 'STD2' },
        { id: 'newstd', standardId: 'STD3' }
      ]
    }

    await subject.updateSolutionForRegistration(input)
    expect(subject.solutionsExApi.apiPorcelainSolutionsExUpdatePut.mock.calls.length).toBe(1)

    const result = subject.solutionsExApi.apiPorcelainSolutionsExUpdatePut.mock.calls[0][0]
    expect(result).toMatchObject({
      solnEx: {
        solution: {
          id: 'testid',
          name: 'testname'
        },
        claimedCapability: [
          { id: 'newcap', capabilityId: 'CAP6' },
          { id: 'testCId1', capabilityId: 'CAP1' }
        ],
        claimedStandard: [
          { id: 'testSId2', standardId: 'STD2' },
          { id: 'newstd', standardId: 'STD3' }
        ],
        claimedCapabilityEvidence: [
          { id: 'cce1', claimId: 'testCId1' }
        ],
        claimedCapabilityReview: [
          { id: 'ccr1', evidenceId: 'cce1' }
        ],
        claimedStandardEvidence: [
          { id: 'cse2', claimId: 'testSId2' }
        ],
        claimedStandardReview: [
          { id: 'csr2', evidenceId: 'cse2' }
        ]
      }
    })
    expect(result.solnEx.claimedCapabilityEvidence).toHaveLength(1)
    expect(result.solnEx.claimedCapabilityReview).toHaveLength(1)
    expect(result.solnEx.claimedStandardEvidence).toHaveLength(1)
    expect(result.solnEx.claimedStandardReview).toHaveLength(1)
  })
})
