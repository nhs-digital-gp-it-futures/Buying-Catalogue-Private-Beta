# CatalogueApi.EvidenceBlobStoreApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiEvidenceBlobStorePrepareForSolutionBySolutionIdPut**](EvidenceBlobStoreApi.md#apiEvidenceBlobStorePrepareForSolutionBySolutionIdPut) | **PUT** /api/EvidenceBlobStore/PrepareForSolution/{solutionId} | Create server side folder structure for specified solution


<a name="apiEvidenceBlobStorePrepareForSolutionBySolutionIdPut"></a>
# **apiEvidenceBlobStorePrepareForSolutionBySolutionIdPut**
> apiEvidenceBlobStorePrepareForSolutionBySolutionIdPut(solutionId)

Create server side folder structure for specified solution

Server side folder structure is something like:  --Organisation  ----Solution  ------Capability Evidence  --------Appointment Management - Citizen  --------Appointment Management - GP  --------Clinical Decision Support  --------[all other claimed capabilities]    Will be done automagically when solution status changes to SolutionStatus.CapabilitiesAssessment

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

var apiInstance = new CatalogueApi.EvidenceBlobStoreApi();

var solutionId = "solutionId_example"; // String | unique identifier of solution

apiInstance.apiEvidenceBlobStorePrepareForSolutionBySolutionIdPut(solutionId).then(function() {
  console.log('API called successfully.');
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **solutionId** | **String**| unique identifier of solution | 

### Return type

null (empty response body)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

