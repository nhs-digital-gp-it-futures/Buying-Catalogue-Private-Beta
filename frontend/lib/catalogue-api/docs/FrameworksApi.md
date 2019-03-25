# CatalogueApi.FrameworksApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiFrameworksByCapabilityByCapabilityIdGet**](FrameworksApi.md#apiFrameworksByCapabilityByCapabilityIdGet) | **GET** /api/Frameworks/ByCapability/{capabilityId} | Get existing framework/s which have the given capability
[**apiFrameworksByIdByIdGet**](FrameworksApi.md#apiFrameworksByIdByIdGet) | **GET** /api/Frameworks/ById/{id} | Get an existing framework given its CRM identifier  Typically used to retrieve previous version
[**apiFrameworksBySolutionBySolutionIdGet**](FrameworksApi.md#apiFrameworksBySolutionBySolutionIdGet) | **GET** /api/Frameworks/BySolution/{solutionId} | Get existing framework/s on which a solution was onboarded, given the CRM identifier of the solution
[**apiFrameworksByStandardByStandardIdGet**](FrameworksApi.md#apiFrameworksByStandardByStandardIdGet) | **GET** /api/Frameworks/ByStandard/{standardId} | Get existing framework/s which have the given standard
[**apiFrameworksGet**](FrameworksApi.md#apiFrameworksGet) | **GET** /api/Frameworks | Retrieve all current frameworks in a paged list


<a name="apiFrameworksByCapabilityByCapabilityIdGet"></a>
# **apiFrameworksByCapabilityByCapabilityIdGet**
> PaginatedListFrameworks apiFrameworksByCapabilityByCapabilityIdGet(capabilityId, opts)

Get existing framework/s which have the given capability

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

var apiInstance = new CatalogueApi.FrameworksApi();

var capabilityId = "capabilityId_example"; // String | CRM identifier of Capability

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiFrameworksByCapabilityByCapabilityIdGet(capabilityId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **capabilityId** | **String**| CRM identifier of Capability | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListFrameworks**](PaginatedListFrameworks.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiFrameworksByIdByIdGet"></a>
# **apiFrameworksByIdByIdGet**
> Frameworks apiFrameworksByIdByIdGet(id)

Get an existing framework given its CRM identifier  Typically used to retrieve previous version

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

var apiInstance = new CatalogueApi.FrameworksApi();

var id = "id_example"; // String | CRM identifier of framework to find

apiInstance.apiFrameworksByIdByIdGet(id).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **String**| CRM identifier of framework to find | 

### Return type

[**Frameworks**](Frameworks.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiFrameworksBySolutionBySolutionIdGet"></a>
# **apiFrameworksBySolutionBySolutionIdGet**
> PaginatedListFrameworks apiFrameworksBySolutionBySolutionIdGet(solutionId, opts)

Get existing framework/s on which a solution was onboarded, given the CRM identifier of the solution

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

var apiInstance = new CatalogueApi.FrameworksApi();

var solutionId = "solutionId_example"; // String | CRM identifier of solution

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiFrameworksBySolutionBySolutionIdGet(solutionId, opts).then(function(data) {
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

[**PaginatedListFrameworks**](PaginatedListFrameworks.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiFrameworksByStandardByStandardIdGet"></a>
# **apiFrameworksByStandardByStandardIdGet**
> PaginatedListFrameworks apiFrameworksByStandardByStandardIdGet(standardId, opts)

Get existing framework/s which have the given standard

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

var apiInstance = new CatalogueApi.FrameworksApi();

var standardId = "standardId_example"; // String | CRM identifier of Standard

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiFrameworksByStandardByStandardIdGet(standardId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **standardId** | **String**| CRM identifier of Standard | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListFrameworks**](PaginatedListFrameworks.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiFrameworksGet"></a>
# **apiFrameworksGet**
> PaginatedListFrameworks apiFrameworksGet(opts)

Retrieve all current frameworks in a paged list

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

var apiInstance = new CatalogueApi.FrameworksApi();

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiFrameworksGet(opts).then(function(data) {
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

[**PaginatedListFrameworks**](PaginatedListFrameworks.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

