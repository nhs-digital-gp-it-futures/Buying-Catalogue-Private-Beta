# CatalogueApi.CapabilitiesApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiCapabilitiesByFrameworkByFrameworkIdGet**](CapabilitiesApi.md#apiCapabilitiesByFrameworkByFrameworkIdGet) | **GET** /api/Capabilities/ByFramework/{frameworkId} | Get existing Capability/s which are in the given Framework
[**apiCapabilitiesByIdByIdGet**](CapabilitiesApi.md#apiCapabilitiesByIdByIdGet) | **GET** /api/Capabilities/ById/{id} | Get an existing capability given its CRM identifier  Typically used to retrieve previous version
[**apiCapabilitiesByIdsPost**](CapabilitiesApi.md#apiCapabilitiesByIdsPost) | **POST** /api/Capabilities/ByIds | Get several existing capabilities given their CRM identifiers
[**apiCapabilitiesByStandardByStandardIdGet**](CapabilitiesApi.md#apiCapabilitiesByStandardByStandardIdGet) | **GET** /api/Capabilities/ByStandard/{standardId} | Get existing Capability/s which require the given/optional Standard
[**apiCapabilitiesGet**](CapabilitiesApi.md#apiCapabilitiesGet) | **GET** /api/Capabilities | Retrieve all current capabilities in a paged list


<a name="apiCapabilitiesByFrameworkByFrameworkIdGet"></a>
# **apiCapabilitiesByFrameworkByFrameworkIdGet**
> PaginatedListCapabilities apiCapabilitiesByFrameworkByFrameworkIdGet(frameworkId, opts)

Get existing Capability/s which are in the given Framework

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

var apiInstance = new CatalogueApi.CapabilitiesApi();

var frameworkId = "frameworkId_example"; // String | CRM identifier of Framework

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiCapabilitiesByFrameworkByFrameworkIdGet(frameworkId, opts).then(function(data) {
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

[**PaginatedListCapabilities**](PaginatedListCapabilities.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiCapabilitiesByIdByIdGet"></a>
# **apiCapabilitiesByIdByIdGet**
> Capabilities apiCapabilitiesByIdByIdGet(id)

Get an existing capability given its CRM identifier  Typically used to retrieve previous version

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

var apiInstance = new CatalogueApi.CapabilitiesApi();

var id = "id_example"; // String | CRM identifier of capability to find

apiInstance.apiCapabilitiesByIdByIdGet(id).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **String**| CRM identifier of capability to find | 

### Return type

[**Capabilities**](Capabilities.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiCapabilitiesByIdsPost"></a>
# **apiCapabilitiesByIdsPost**
> [Capabilities] apiCapabilitiesByIdsPost(opts)

Get several existing capabilities given their CRM identifiers

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

var apiInstance = new CatalogueApi.CapabilitiesApi();

var opts = { 
  'ids': [new CatalogueApi.[String]()] // [String] | Array of CRM identifiers of capabilities to find
};
apiInstance.apiCapabilitiesByIdsPost(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **ids** | **[String]**| Array of CRM identifiers of capabilities to find | [optional] 

### Return type

[**[Capabilities]**](Capabilities.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: application/json

<a name="apiCapabilitiesByStandardByStandardIdGet"></a>
# **apiCapabilitiesByStandardByStandardIdGet**
> PaginatedListCapabilities apiCapabilitiesByStandardByStandardIdGet(standardId, opts)

Get existing Capability/s which require the given/optional Standard

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

var apiInstance = new CatalogueApi.CapabilitiesApi();

var standardId = "standardId_example"; // String | CRM identifier of Standard

var opts = { 
  'isOptional': true, // Boolean | true if the specified Standard is optional with the Capability
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiCapabilitiesByStandardByStandardIdGet(standardId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **standardId** | **String**| CRM identifier of Standard | 
 **isOptional** | **Boolean**| true if the specified Standard is optional with the Capability | [optional] 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListCapabilities**](PaginatedListCapabilities.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiCapabilitiesGet"></a>
# **apiCapabilitiesGet**
> PaginatedListCapabilities apiCapabilitiesGet(opts)

Retrieve all current capabilities in a paged list

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

var apiInstance = new CatalogueApi.CapabilitiesApi();

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiCapabilitiesGet(opts).then(function(data) {
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

[**PaginatedListCapabilities**](PaginatedListCapabilities.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

