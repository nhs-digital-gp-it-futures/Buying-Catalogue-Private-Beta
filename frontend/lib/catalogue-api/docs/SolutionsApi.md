# CatalogueApi.SolutionsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiSolutionsByFrameworkByFrameworkIdGet**](SolutionsApi.md#apiSolutionsByFrameworkByFrameworkIdGet) | **GET** /api/Solutions/ByFramework/{frameworkId} | Get existing solution/s on which were onboarded onto a framework,  given the CRM identifier of the framework
[**apiSolutionsByIdByIdGet**](SolutionsApi.md#apiSolutionsByIdByIdGet) | **GET** /api/Solutions/ById/{id} | Get an existing solution given its CRM identifier  Typically used to retrieve previous version
[**apiSolutionsByOrganisationByOrganisationIdGet**](SolutionsApi.md#apiSolutionsByOrganisationByOrganisationIdGet) | **GET** /api/Solutions/ByOrganisation/{organisationId} | Retrieve all current solutions in a paged list for an organisation,  given the organisation’s CRM identifier
[**apiSolutionsDelete**](SolutionsApi.md#apiSolutionsDelete) | **DELETE** /api/Solutions | Delete an existing solution  DEVELOPMENT MODE ONLY
[**apiSolutionsPost**](SolutionsApi.md#apiSolutionsPost) | **POST** /api/Solutions | Create a new solution for an organisation
[**apiSolutionsPut**](SolutionsApi.md#apiSolutionsPut) | **PUT** /api/Solutions | Update an existing solution with new information


<a name="apiSolutionsByFrameworkByFrameworkIdGet"></a>
# **apiSolutionsByFrameworkByFrameworkIdGet**
> PaginatedListSolutions apiSolutionsByFrameworkByFrameworkIdGet(frameworkId, opts)

Get existing solution/s on which were onboarded onto a framework,  given the CRM identifier of the framework

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

var apiInstance = new CatalogueApi.SolutionsApi();

var frameworkId = "frameworkId_example"; // String | CRM identifier of framework

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiSolutionsByFrameworkByFrameworkIdGet(frameworkId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **frameworkId** | **String**| CRM identifier of framework | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListSolutions**](PaginatedListSolutions.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiSolutionsByIdByIdGet"></a>
# **apiSolutionsByIdByIdGet**
> Solutions apiSolutionsByIdByIdGet(id)

Get an existing solution given its CRM identifier  Typically used to retrieve previous version

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

var apiInstance = new CatalogueApi.SolutionsApi();

var id = "id_example"; // String | CRM identifier of solution to find

apiInstance.apiSolutionsByIdByIdGet(id).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **String**| CRM identifier of solution to find | 

### Return type

[**Solutions**](Solutions.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiSolutionsByOrganisationByOrganisationIdGet"></a>
# **apiSolutionsByOrganisationByOrganisationIdGet**
> PaginatedListSolutions apiSolutionsByOrganisationByOrganisationIdGet(organisationId, opts)

Retrieve all current solutions in a paged list for an organisation,  given the organisation’s CRM identifier

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

var apiInstance = new CatalogueApi.SolutionsApi();

var organisationId = "organisationId_example"; // String | CRM identifier of organisation

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiSolutionsByOrganisationByOrganisationIdGet(organisationId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **organisationId** | **String**| CRM identifier of organisation | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListSolutions**](PaginatedListSolutions.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiSolutionsDelete"></a>
# **apiSolutionsDelete**
> apiSolutionsDelete(opts)

Delete an existing solution  DEVELOPMENT MODE ONLY

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

var apiInstance = new CatalogueApi.SolutionsApi();

var opts = { 
  'solution': new CatalogueApi.Solutions() // Solutions | solution
};
apiInstance.apiSolutionsDelete(opts).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **solution** | [**Solutions**](Solutions.md)| solution | [optional] 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

<a name="apiSolutionsPost"></a>
# **apiSolutionsPost**
> Solutions apiSolutionsPost(opts)

Create a new solution for an organisation

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

var apiInstance = new CatalogueApi.SolutionsApi();

var opts = { 
  'solution': new CatalogueApi.Solutions() // Solutions | new solution information
};
apiInstance.apiSolutionsPost(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **solution** | [**Solutions**](Solutions.md)| new solution information | [optional] 

### Return type

[**Solutions**](Solutions.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: application/json

<a name="apiSolutionsPut"></a>
# **apiSolutionsPut**
> apiSolutionsPut(opts)

Update an existing solution with new information

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

var apiInstance = new CatalogueApi.SolutionsApi();

var opts = { 
  'solution': new CatalogueApi.Solutions() // Solutions | solution with updated information
};
apiInstance.apiSolutionsPut(opts).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **solution** | [**Solutions**](Solutions.md)| solution with updated information | [optional] 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

