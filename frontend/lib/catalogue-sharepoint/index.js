
const FakeSharePointProvider = require('./providers/FakeSharePointProvider')
const RealSharePointProvider = require('./providers/RealSharePointProvider')

module.exports = {
  sharePointProvider: process.env.SHAREPOINT_PROVIDER_ENV === 'test'
    ? new FakeSharePointProvider()
    : new RealSharePointProvider(),
  SharePointProvider: require('./providers/SharePointProvider')
}
