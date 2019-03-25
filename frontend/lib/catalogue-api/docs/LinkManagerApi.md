# CatalogueApi.LinkManagerApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiLinkManagerFrameworkSolutionCreateByFrameworkIdBySolutionIdPost**](LinkManagerApi.md#apiLinkManagerFrameworkSolutionCreateByFrameworkIdBySolutionIdPost) | **POST** /api/LinkManager/FrameworkSolution/Create/{frameworkId}/{solutionId} | Create a link between a Framework and a Solution


<a name="apiLinkManagerFrameworkSolutionCreateByFrameworkIdBySolutionIdPost"></a>
# **apiLinkManagerFrameworkSolutionCreateByFrameworkIdBySolutionIdPost**
> apiLinkManagerFrameworkSolutionCreateByFrameworkIdBySolutionIdPost(frameworkId, solutionId)

Create a link between a Framework and a Solution

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

var apiInstance = new CatalogueApi.LinkManagerApi();

var frameworkId = "frameworkId_example"; // String | CRM identifier of Framework

var solutionId = "solutionId_example"; // String | CRM identifier of Solution

apiInstance.apiLinkManagerFrameworkSolutionCreateByFrameworkIdBySolutionIdPost(frameworkId, solutionId).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **frameworkId** | **String**| CRM identifier of Framework | 
 **solutionId** | **String**| CRM identifier of Solution | 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

