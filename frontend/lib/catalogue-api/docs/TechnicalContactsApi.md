# CatalogueApi.TechnicalContactsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiTechnicalContactsBySolutionBySolutionIdGet**](TechnicalContactsApi.md#apiTechnicalContactsBySolutionBySolutionIdGet) | **GET** /api/TechnicalContacts/BySolution/{solutionId} | Retrieve all Technical Contacts for a solution in a paged list,  given the solution’s CRM identifier
[**apiTechnicalContactsDelete**](TechnicalContactsApi.md#apiTechnicalContactsDelete) | **DELETE** /api/TechnicalContacts | Delete an existing Technical Contact for a Solution
[**apiTechnicalContactsPost**](TechnicalContactsApi.md#apiTechnicalContactsPost) | **POST** /api/TechnicalContacts | Create a new Technical Contact for a Solution
[**apiTechnicalContactsPut**](TechnicalContactsApi.md#apiTechnicalContactsPut) | **PUT** /api/TechnicalContacts | Update a Technical Contact with new information


<a name="apiTechnicalContactsBySolutionBySolutionIdGet"></a>
# **apiTechnicalContactsBySolutionBySolutionIdGet**
> PaginatedListTechnicalContacts apiTechnicalContactsBySolutionBySolutionIdGet(solutionId, opts)

Retrieve all Technical Contacts for a solution in a paged list,  given the solution’s CRM identifier

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

var apiInstance = new CatalogueApi.TechnicalContactsApi();

var solutionId = "solutionId_example"; // String | CRM identifier of solution

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiTechnicalContactsBySolutionBySolutionIdGet(solutionId, opts).then(function(data) {
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

[**PaginatedListTechnicalContacts**](PaginatedListTechnicalContacts.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiTechnicalContactsDelete"></a>
# **apiTechnicalContactsDelete**
> apiTechnicalContactsDelete(opts)

Delete an existing Technical Contact for a Solution

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

var apiInstance = new CatalogueApi.TechnicalContactsApi();

var opts = { 
  'techCont': new CatalogueApi.TechnicalContacts() // TechnicalContacts | existing Technical Contact information
};
apiInstance.apiTechnicalContactsDelete(opts).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **techCont** | [**TechnicalContacts**](TechnicalContacts.md)| existing Technical Contact information | [optional] 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

<a name="apiTechnicalContactsPost"></a>
# **apiTechnicalContactsPost**
> TechnicalContacts apiTechnicalContactsPost(opts)

Create a new Technical Contact for a Solution

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

var apiInstance = new CatalogueApi.TechnicalContactsApi();

var opts = { 
  'techCont': new CatalogueApi.TechnicalContacts() // TechnicalContacts | new Technical Contact information
};
apiInstance.apiTechnicalContactsPost(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **techCont** | [**TechnicalContacts**](TechnicalContacts.md)| new Technical Contact information | [optional] 

### Return type

[**TechnicalContacts**](TechnicalContacts.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: application/json

<a name="apiTechnicalContactsPut"></a>
# **apiTechnicalContactsPut**
> apiTechnicalContactsPut(opts)

Update a Technical Contact with new information

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

var apiInstance = new CatalogueApi.TechnicalContactsApi();

var opts = { 
  'techCont': new CatalogueApi.TechnicalContacts() // TechnicalContacts | Technical Contact with updated information
};
apiInstance.apiTechnicalContactsPut(opts).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **techCont** | [**TechnicalContacts**](TechnicalContacts.md)| Technical Contact with updated information | [optional] 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

