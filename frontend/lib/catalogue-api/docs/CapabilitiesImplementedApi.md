# CatalogueApi.CapabilitiesImplementedApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiCapabilitiesImplementedByIdGet**](CapabilitiesImplementedApi.md#apiCapabilitiesImplementedByIdGet) | **GET** /api/CapabilitiesImplemented/{id} | Retrieve claim, given the claim’s CRM identifier
[**apiCapabilitiesImplementedBySolutionBySolutionIdGet**](CapabilitiesImplementedApi.md#apiCapabilitiesImplementedBySolutionBySolutionIdGet) | **GET** /api/CapabilitiesImplemented/BySolution/{solutionId} | Retrieve all claimed capabilities for a solution in a paged list,  given the solution’s CRM identifier
[**apiCapabilitiesImplementedDelete**](CapabilitiesImplementedApi.md#apiCapabilitiesImplementedDelete) | **DELETE** /api/CapabilitiesImplemented | Delete an existing claimed capability for a solution
[**apiCapabilitiesImplementedPost**](CapabilitiesImplementedApi.md#apiCapabilitiesImplementedPost) | **POST** /api/CapabilitiesImplemented | Create a new claimed capability for a solution
[**apiCapabilitiesImplementedPut**](CapabilitiesImplementedApi.md#apiCapabilitiesImplementedPut) | **PUT** /api/CapabilitiesImplemented | Update an existing claimed capability with new information


<a name="apiCapabilitiesImplementedByIdGet"></a>
# **apiCapabilitiesImplementedByIdGet**
> CapabilitiesImplemented apiCapabilitiesImplementedByIdGet(id)

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedApi();

var id = "id_example"; // String | CRM identifier of claim

apiInstance.apiCapabilitiesImplementedByIdGet(id).then(function(data) {
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

[**CapabilitiesImplemented**](CapabilitiesImplemented.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiCapabilitiesImplementedBySolutionBySolutionIdGet"></a>
# **apiCapabilitiesImplementedBySolutionBySolutionIdGet**
> PaginatedListCapabilitiesImplemented apiCapabilitiesImplementedBySolutionBySolutionIdGet(solutionId, opts)

Retrieve all claimed capabilities for a solution in a paged list,  given the solution’s CRM identifier

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedApi();

var solutionId = "solutionId_example"; // String | CRM identifier of solution

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiCapabilitiesImplementedBySolutionBySolutionIdGet(solutionId, opts).then(function(data) {
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

[**PaginatedListCapabilitiesImplemented**](PaginatedListCapabilitiesImplemented.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiCapabilitiesImplementedDelete"></a>
# **apiCapabilitiesImplementedDelete**
> apiCapabilitiesImplementedDelete(opts)

Delete an existing claimed capability for a solution

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedApi();

var opts = { 
  'claimedcapability': new CatalogueApi.CapabilitiesImplemented() // CapabilitiesImplemented | existing claimed capability information
};
apiInstance.apiCapabilitiesImplementedDelete(opts).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimedcapability** | [**CapabilitiesImplemented**](CapabilitiesImplemented.md)| existing claimed capability information | [optional] 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

<a name="apiCapabilitiesImplementedPost"></a>
# **apiCapabilitiesImplementedPost**
> CapabilitiesImplemented apiCapabilitiesImplementedPost(opts)

Create a new claimed capability for a solution

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedApi();

var opts = { 
  'claimedcapability': new CatalogueApi.CapabilitiesImplemented() // CapabilitiesImplemented | new claimed capability information
};
apiInstance.apiCapabilitiesImplementedPost(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimedcapability** | [**CapabilitiesImplemented**](CapabilitiesImplemented.md)| new claimed capability information | [optional] 

### Return type

[**CapabilitiesImplemented**](CapabilitiesImplemented.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: application/json

<a name="apiCapabilitiesImplementedPut"></a>
# **apiCapabilitiesImplementedPut**
> apiCapabilitiesImplementedPut(opts)

Update an existing claimed capability with new information

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedApi();

var opts = { 
  'claimedcapability': new CatalogueApi.CapabilitiesImplemented() // CapabilitiesImplemented | claimed capability with updated information
};
apiInstance.apiCapabilitiesImplementedPut(opts).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimedcapability** | [**CapabilitiesImplemented**](CapabilitiesImplemented.md)| claimed capability with updated information | [optional] 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

