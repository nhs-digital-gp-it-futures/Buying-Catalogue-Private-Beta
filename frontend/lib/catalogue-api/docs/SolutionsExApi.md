# CatalogueApi.SolutionsExApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiPorcelainSolutionsExByOrganisationByOrganisationIdGet**](SolutionsExApi.md#apiPorcelainSolutionsExByOrganisationByOrganisationIdGet) | **GET** /api/porcelain/SolutionsEx/ByOrganisation/{organisationId} | Get a list of Solutions, each with a list of corresponding TechnicalContact, ClaimedCapability, ClaimedStandard et al
[**apiPorcelainSolutionsExBySolutionBySolutionIdGet**](SolutionsExApi.md#apiPorcelainSolutionsExBySolutionBySolutionIdGet) | **GET** /api/porcelain/SolutionsEx/BySolution/{solutionId} | Get a Solution with a list of corresponding TechnicalContact, ClaimedCapability, ClaimedStandard et al
[**apiPorcelainSolutionsExUpdatePut**](SolutionsExApi.md#apiPorcelainSolutionsExUpdatePut) | **PUT** /api/porcelain/SolutionsEx/Update | Update an existing Solution, TechnicalContact, ClaimedCapability, ClaimedStandard et al with new information


<a name="apiPorcelainSolutionsExByOrganisationByOrganisationIdGet"></a>
# **apiPorcelainSolutionsExByOrganisationByOrganisationIdGet**
> [SolutionEx] apiPorcelainSolutionsExByOrganisationByOrganisationIdGet(organisationId)

Get a list of Solutions, each with a list of corresponding TechnicalContact, ClaimedCapability, ClaimedStandard et al

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

var apiInstance = new CatalogueApi.SolutionsExApi();

var organisationId = "organisationId_example"; // String | CRM identifier of Organisation

apiInstance.apiPorcelainSolutionsExByOrganisationByOrganisationIdGet(organisationId).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **organisationId** | **String**| CRM identifier of Organisation | 

### Return type

[**[SolutionEx]**](SolutionEx.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiPorcelainSolutionsExBySolutionBySolutionIdGet"></a>
# **apiPorcelainSolutionsExBySolutionBySolutionIdGet**
> SolutionEx apiPorcelainSolutionsExBySolutionBySolutionIdGet(solutionId)

Get a Solution with a list of corresponding TechnicalContact, ClaimedCapability, ClaimedStandard et al

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

var apiInstance = new CatalogueApi.SolutionsExApi();

var solutionId = "solutionId_example"; // String | CRM identifier of Solution

apiInstance.apiPorcelainSolutionsExBySolutionBySolutionIdGet(solutionId).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **solutionId** | **String**| CRM identifier of Solution | 

### Return type

[**SolutionEx**](SolutionEx.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiPorcelainSolutionsExUpdatePut"></a>
# **apiPorcelainSolutionsExUpdatePut**
> apiPorcelainSolutionsExUpdatePut(opts)

Update an existing Solution, TechnicalContact, ClaimedCapability, ClaimedStandard et al with new information

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

var apiInstance = new CatalogueApi.SolutionsExApi();

var opts = { 
  'solnEx': new CatalogueApi.SolutionEx() // SolutionEx | Solution, TechnicalContact, ClaimedCapability, ClaimedStandard et al with updated information
};
apiInstance.apiPorcelainSolutionsExUpdatePut(opts).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **solnEx** | [**SolutionEx**](SolutionEx.md)| Solution, TechnicalContact, ClaimedCapability, ClaimedStandard et al with updated information | [optional] 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

