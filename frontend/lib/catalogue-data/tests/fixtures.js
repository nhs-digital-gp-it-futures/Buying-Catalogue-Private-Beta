/* eslint-env jest */

const { DataProvider } = require('../index')

module.exports = {
  mockApi: function () {
    function MockApi () {
    }

    function MockContactsApi () {
      this.apiContactsByEmailByEmailGet = jest.fn()
      this.apiContactsByOrganisationByOrganisationIdGet = jest.fn()
    }

    function MockOrganisationsApi () {
      this.apiOrganisationsByContactByContactIdGet = jest.fn()
    }

    function MockSolutionsApi () {
      this.apiSolutionsByOrganisationByOrganisationIdGet = jest.fn()
    }

    function MockSolutionsExApi () {
      this.apiPorcelainSolutionsExBySolutionBySolutionIdGet = jest.fn()
      this.apiPorcelainSolutionsExUpdatePut = jest.fn()
      this.apiPorcelainSolutionsExByOrganisationByOrganisationIdGet = jest.fn()
    }

    function MockCapabilityMappingsApi () {
      this.apiPorcelainCapabilityMappingsGet = jest.fn()
    }

    function MockCapabilitiesImplementedEvidenceApi () {
      this.apiCapabilitiesImplementedEvidenceByClaimByClaimIdGet = jest.fn()
    }

    return new DataProvider({
      ContactsApi: MockContactsApi,
      OrganisationsApi: MockOrganisationsApi,
      SolutionsApi: MockSolutionsApi,
      SolutionsExApi: MockSolutionsExApi,
      CapabilityMappingsApi: MockCapabilityMappingsApi,
      CapabilitiesImplementedEvidenceApi: MockCapabilitiesImplementedEvidenceApi
    })
  }
}
