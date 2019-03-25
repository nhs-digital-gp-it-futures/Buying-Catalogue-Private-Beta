/* eslint-env jest */

let subject

beforeEach(() => {
  subject = require('./fixtures').mockApi()
})

describe('capabilityMappings', () => {
  it('correctly maps the incoming static data', async () => {
    subject.capabilityMappingsApi.apiPorcelainCapabilityMappingsGet.mockReturnValue(
      require('./capabilityMappings.test-data')
    )

    const result = await subject.capabilityMappings()

    expect(Object.keys(result.capabilities)).toHaveLength(18)
    expect(result.capabilities['CAP1'].name).toBe('Appointments Management - Citizen')
    expect(result.capabilities['CAP1'].standards).toHaveLength(12)

    expect(Object.keys(result.standards)).toHaveLength(50)
    expect(result.standards['OS1'].name).toBe('Business Continuity / Disaster Recovery')
    expect(result.standards['OS1'].isOverarching).toBeTruthy()
  })
})
