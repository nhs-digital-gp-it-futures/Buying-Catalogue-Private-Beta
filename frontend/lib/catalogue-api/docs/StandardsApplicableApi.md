# CatalogueApi.StandardsApplicableApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiStandardsApplicableByIdGet**](StandardsApplicableApi.md#apiStandardsApplicableByIdGet) | **GET** /api/StandardsApplicable/{id} | Retrieve claim, given the claim’s CRM identifier
[**apiStandardsApplicableBySolutionBySolutionIdGet**](StandardsApplicableApi.md#apiStandardsApplicableBySolutionBySolutionIdGet) | **GET** /api/StandardsApplicable/BySolution/{solutionId} | Retrieve all claimed standards for a solution in a paged list,   given the solution’s CRM identifier
[**apiStandardsApplicableDelete**](StandardsApplicableApi.md#apiStandardsApplicableDelete) | **DELETE** /api/StandardsApplicable | Delete an existing claimed standard for a solution
[**apiStandardsApplicablePost**](StandardsApplicableApi.md#apiStandardsApplicablePost) | **POST** /api/StandardsApplicable | Create a new claimed standard for a solution
[**apiStandardsApplicablePut**](StandardsApplicableApi.md#apiStandardsApplicablePut) | **PUT** /api/StandardsApplicable | Update an existing claimed standard with new information


<a name="apiStandardsApplicableByIdGet"></a>
# **apiStandardsApplicableByIdGet**
> StandardsApplicable apiStandardsApplicableByIdGet(id)

Retrieve claim, given the claim’s CRM identifier

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

var apiInstance = new CatalogueApi.StandardsApplicableApi();

var id = "id_example"; // String | CRM identifier of claim

apiInstance.apiStandardsApplicableByIdGet(id).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **String**| CRM identifier of claim | 

### Return type

[**StandardsApplicable**](StandardsApplicable.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiStandardsApplicableBySolutionBySolutionIdGet"></a>
# **apiStandardsApplicableBySolutionBySolutionIdGet**
> PaginatedListStandardsApplicable apiStandardsApplicableBySolutionBySolutionIdGet(solutionId, opts)

Retrieve all claimed standards for a solution in a paged list,   given the solution’s CRM identifier

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

var apiInstance = new CatalogueApi.StandardsApplicableApi();

var solutionId = "solutionId_example"; // String | CRM identifier of solution

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiStandardsApplicableBySolutionBySolutionIdGet(solutionId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **solutionId** | **String**| CRM identifier of solution | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListStandardsApplicable**](PaginatedListStandardsApplicable.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiStandardsApplicableDelete"></a>
# **apiStandardsApplicableDelete**
> apiStandardsApplicableDelete(opts)

Delete an existing claimed standard for a solution

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

var apiInstance = new CatalogueApi.StandardsApplicableApi();

var opts = { 
  'claimedstandard': new CatalogueApi.StandardsApplicable() // StandardsApplicable | existing claimed standard information
};
apiInstance.apiStandardsApplicableDelete(opts).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimedstandard** | [**StandardsApplicable**](StandardsApplicable.md)| existing claimed standard information | [optional] 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

<a name="apiStandardsApplicablePost"></a>
# **apiStandardsApplicablePost**
> StandardsApplicable apiStandardsApplicablePost(opts)

Create a new claimed standard for a solution

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

var apiInstance = new CatalogueApi.StandardsApplicableApi();

var opts = { 
  'claimedstandard': new CatalogueApi.StandardsApplicable() // StandardsApplicable | new claimed standard information
};
apiInstance.apiStandardsApplicablePost(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimedstandard** | [**StandardsApplicable**](StandardsApplicable.md)| new claimed standard information | [optional] 

### Return type

[**StandardsApplicable**](StandardsApplicable.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: application/json

<a name="apiStandardsApplicablePut"></a>
# **apiStandardsApplicablePut**
> apiStandardsApplicablePut(opts)

Update an existing claimed standard with new information

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

var apiInstance = new CatalogueApi.StandardsApplicableApi();

var opts = { 
  'claimedstandard': new CatalogueApi.StandardsApplicable() // StandardsApplicable | claimed standard with updated information
};
apiInstance.apiStandardsApplicablePut(opts).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimedstandard** | [**StandardsApplicable**](StandardsApplicable.md)| claimed standard with updated information | [optional] 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

