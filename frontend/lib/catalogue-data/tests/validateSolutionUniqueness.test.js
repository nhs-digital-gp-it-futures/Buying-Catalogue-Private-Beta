/* eslint-env jest */

let subject

beforeEach(() => {
  subject = require('./fixtures').mockApi()
})

describe('validateSolutionUniqueness', () => {
  beforeEach(() => {
    subject.solutionsApi.apiSolutionsByOrganisationByOrganisationIdGet.mockReturnValue({
      items: [
        { id: 'id1', name: 'This is a duplicate', version: '1' },
        { id: 'id2', name: 'This is not a duplicate', version: '2' }
      ]
    })
  })

  it('correctly detects duplicate name and version for new solutions', async () => {
    let result = await subject.validateSolutionUniqueness({
      name: 'This is a duplicate',
      version: '1',
      organisationId: 'orgId'
    })
    expect(result).toBeFalsy()

    result = await subject.validateSolutionUniqueness({
      name: 'This is not a duplicate',
      version: '1',
      organisationId: 'orgId'
    })
    expect(result).toBeTruthy()

    result = await subject.validateSolutionUniqueness({
      name: 'This is not a duplicate',
      version: '2',
      organisationId: 'orgId'
    })
    expect(result).toBeFalsy()

    result = await subject.validateSolutionUniqueness({
      name: 'This is a duplicate',
      version: '2',
      organisationId: 'orgId'
    })
    expect(result).toBeTruthy()
  })

  it('correctly detects duplicate name and version for existing solutions', async () => {
    let result = await subject.validateSolutionUniqueness({
      id: 'id1',
      name: 'This is a duplicate',
      version: '1',
      organisationId: 'orgId'
    })
    expect(result).toBeTruthy()

    result = await subject.validateSolutionUniqueness({
      id: 'id1',
      name: 'This is not a duplicate',
      version: '1',
      organisationId: 'orgId'
    })
    expect(result).toBeTruthy()

    result = await subject.validateSolutionUniqueness({
      id: 'id1',
      name: 'This is not a duplicate',
      version: '2',
      organisationId: 'orgId'
    })
    expect(result).toBeFalsy()
  })
})
