/* eslint-env jest */

let subject

beforeEach(() => {
  subject = require('./fixtures').mockApi()
})

describe('solutionsForSupplierDashboard', () => {
  function mockResult (resultData) {
    subject.solutionsApi.apiSolutionsByOrganisationByOrganisationIdGet.mockReturnValue(resultData)
  }

  it('returns empty arrays if there are no solutions', async () => {
    mockResult([])
    await expect(subject.solutionsForSupplierDashboard(12345)).resolves.toEqual({
      onboarding: [],
      live: []
    })
    expect(subject.solutionsApi.apiSolutionsByOrganisationByOrganisationIdGet.mock.calls.length).toBe(1)
    expect(subject.solutionsApi.apiSolutionsByOrganisationByOrganisationIdGet.mock.calls[0][0]).toBe(12345)
  })

  const testData = {
    items: [
      { id: 'failed', status: '-1', name: 'It Failed', version: 'f' },
      { id: 'draft', status: '0', name: 'A Draft' },
      { id: 'registered', status: '1', name: 'B Registered', version: '1.0r' },
      { id: 'assessment-draft', status: '1', name: 'C Assessment', version: '2.1a' },
      { id: 'assessment', status: '2', name: 'D Assessment', version: '2.2a' },
      { id: 'compliance', status: '3', name: 'E Compliance', version: '3c' },
      { id: 'approval', status: '4', name: 'F Approval' },
      { id: 'solution', status: '5', name: 'G Solution Page' },
      { id: 'live', status: '6', name: 'H Live', version: 'LLL' }
    ]
  }

  it('separates live and currently onboarding solutions correctly', async () => {
    mockResult(testData)
    const result = await subject.solutionsForSupplierDashboard(12345)

    expect(result.live).toHaveLength(1)
    expect(result.live[0].id).toEqual('live')
  })

  it('correctly formats the solution name and version for display on the dashboard', async () => {
    mockResult(testData)
    const result = await subject.solutionsForSupplierDashboard(12345)

    expect(result.onboarding.find(_ => _.id === 'draft').displayName).toEqual('A Draft')
    expect(result.onboarding.find(_ => _.id === 'compliance').displayName).toEqual('E Compliance | 3c')
    expect(result.live[0].displayName).toEqual('H Live | LLL')
  })

  it.skip('correctly describes the properties of onboarding solutions', async () => {
    // skipped as the requirements are not well-defined enough
  })

  it.skip('correctly describes the properties of live solutions', async () => {
    // skipped as the requirements are not well-defined enough
  })

  it('correctly applies a supplier mapping function to the results', async () => {
    const testMapper = (soln) => ({
      ...soln,
      copiedId: soln.id.toUpperCase()
    })
    mockResult(testData)
    const result = await subject.solutionsForSupplierDashboard(12345, testMapper)

    expect(result.onboarding.map(_ => _.copiedId)).toEqual([
      'FAILED', 'DRAFT', 'REGISTERED', 'ASSESSMENT-DRAFT', 'ASSESSMENT', 'COMPLIANCE', 'APPROVAL', 'SOLUTION'
    ])
    expect(result.live).toHaveLength(1)
    expect(result.live[0].copiedId).toEqual('LIVE')
  })
})
