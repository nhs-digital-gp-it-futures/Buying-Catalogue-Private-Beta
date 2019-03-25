const SharePointProvider = require('./SharePointProvider')
// export a default, pre-configured instance of the data provider
// as well as the constructor to allow for using testing with a mock API class
class RealSharePointProvider extends SharePointProvider {
  constructor () {
    super(require('catalogue-api'))
    this.CatalogueApi.ApiClient.instance.basePath = process.env.API_BASE_URL || 'http://api:5100'
  }

  // support for the authentication layer
  setAuthenticationToken (token) {
    this.CatalogueApi.ApiClient.instance.authentications.oauth2.accessToken = token
  }
}

module.exports = RealSharePointProvider
