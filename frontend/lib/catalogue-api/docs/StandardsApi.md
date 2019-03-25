# CatalogueApi.StandardsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiStandardsByCapabilityByCapabilityIdGet**](StandardsApi.md#apiStandardsByCapabilityByCapabilityIdGet) | **GET** /api/Standards/ByCapability/{capabilityId} | Get existing/optional Standard/s which are in the given Capability
[**apiStandardsByFrameworkByFrameworkIdGet**](StandardsApi.md#apiStandardsByFrameworkByFrameworkIdGet) | **GET** /api/Standards/ByFramework/{frameworkId} | Get existing Standard/s which are in the given Framework
[**apiStandardsByIdByIdGet**](StandardsApi.md#apiStandardsByIdByIdGet) | **GET** /api/Standards/ById/{id} | Get an existing standard given its CRM identifier  Typically used to retrieve previous version
[**apiStandardsByIdsPost**](StandardsApi.md#apiStandardsByIdsPost) | **POST** /api/Standards/ByIds | Get several existing Standards given their CRM identifiers
[**apiStandardsGet**](StandardsApi.md#apiStandardsGet) | **GET** /api/Standards | Retrieve all current standards in a paged list


<a name="apiStandardsByCapabilityByCapabilityIdGet"></a>
# **apiStandardsByCapabilityByCapabilityIdGet**
> PaginatedListStandards apiStandardsByCapabilityByCapabilityIdGet(capabilityId, opts)

Get existing/optional Standard/s which are in the given Capability

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

var apiInstance = new CatalogueApi.StandardsApi();

var capabilityId = "capabilityId_example"; // String | CRM identifier of Capability

var opts = { 
  'isOptional': true, // Boolean | true if the specified Standard is optional with the Capability
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiStandardsByCapabilityByCapabilityIdGet(capabilityId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **capabilityId** | **String**| CRM identifier of Capability | 
 **isOptional** | **Boolean**| true if the specified Standard is optional with the Capability | [optional] 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListStandards**](PaginatedListStandards.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiStandardsByFrameworkByFrameworkIdGet"></a>
# **apiStandardsByFrameworkByFrameworkIdGet**
> PaginatedListStandards apiStandardsByFrameworkByFrameworkIdGet(frameworkId, opts)

Get existing Standard/s which are in the given Framework

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

var apiInstance = new CatalogueApi.StandardsApi();

var frameworkId = "frameworkId_example"; // String | CRM identifier of Framework

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiStandardsByFrameworkByFrameworkIdGet(frameworkId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **frameworkId** | **String**| CRM identifier of Framework | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListStandards**](PaginatedListStandards.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiStandardsByIdByIdGet"></a>
# **apiStandardsByIdByIdGet**
> Standards apiStandardsByIdByIdGet(id)

Get an existing standard given its CRM identifier  Typically used to retrieve previous version

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

var apiInstance = new CatalogueApi.StandardsApi();

var id = "id_example"; // String | CRM identifier of standard to find

apiInstance.apiStandardsByIdByIdGet(id).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **String**| CRM identifier of standard to find | 

### Return type

[**Standards**](Standards.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiStandardsByIdsPost"></a>
# **apiStandardsByIdsPost**
> [Standards] apiStandardsByIdsPost(opts)

Get several existing Standards given their CRM identifiers

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

var apiInstance = new CatalogueApi.StandardsApi();

var opts = { 
  'ids': [new CatalogueApi.[String]()] // [String] | Array of CRM identifiers of Standards to find
};
apiInstance.apiStandardsByIdsPost(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **ids** | **[String]**| Array of CRM identifiers of Standards to find | [optional] 

### Return type

[**[Standards]**](Standards.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: application/json

<a name="apiStandardsGet"></a>
# **apiStandardsGet**
> PaginatedListStandards apiStandardsGet(opts)

Retrieve all current standards in a paged list

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

var apiInstance = new CatalogueApi.StandardsApi();

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiStandardsGet(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListStandards**](PaginatedListStandards.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

