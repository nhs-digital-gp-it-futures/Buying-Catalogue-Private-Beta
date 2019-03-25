# CatalogueApi.CapabilityMappingsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiPorcelainCapabilityMappingsGet**](CapabilityMappingsApi.md#apiPorcelainCapabilityMappingsGet) | **GET** /api/porcelain/CapabilityMappings | Get capabilities with a list of corresponding, optional standards


<a name="apiPorcelainCapabilityMappingsGet"></a>
# **apiPorcelainCapabilityMappingsGet**
> CapabilityMappings apiPorcelainCapabilityMappingsGet()

Get capabilities with a list of corresponding, optional standards

### Example
```javascript
var CatalogueApi = require('catalogue-api');
var defaultClient = CatalogueApi.ApiClient.instance;

// Configure HTTP basic authorization: basic
var basic = defaultClient.authentications['basic'];
basic.username = 'YOUR USERNAME';
basic.password = 'YOUR PASSWORD';

// Configure OAuth2 access token for authorization: oauth2
var oauth2 = defaultClient.authentications['oauth2'];
oauth2.accessToken = 'YOUR ACCESS TOKEN';

var apiInstance = new CatalogueApi.CapabilityMappingsApi();
apiInstance.apiPorcelainCapabilityMappingsGet().then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters
This endpoint does not need any parameter.

### Return type

[**CapabilityMappings**](CapabilityMappings.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

