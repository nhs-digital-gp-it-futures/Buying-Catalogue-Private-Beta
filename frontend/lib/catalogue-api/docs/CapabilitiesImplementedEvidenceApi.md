# CatalogueApi.CapabilitiesImplementedEvidenceApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiCapabilitiesImplementedEvidenceByClaimByClaimIdGet**](CapabilitiesImplementedEvidenceApi.md#apiCapabilitiesImplementedEvidenceByClaimByClaimIdGet) | **GET** /api/CapabilitiesImplementedEvidence/ByClaim/{claimId} | Get all Evidence for the given Claim  Each list is a distinct &#39;chain&#39; of Evidence ie original Evidence with all subsequent Evidence  The first item in each &#39;chain&#39; is the most current Evidence.  The last item in each &#39;chain&#39; is the original Evidence.
[**apiCapabilitiesImplementedEvidencePost**](CapabilitiesImplementedEvidenceApi.md#apiCapabilitiesImplementedEvidencePost) | **POST** /api/CapabilitiesImplementedEvidence | Create a new evidence


<a name="apiCapabilitiesImplementedEvidenceByClaimByClaimIdGet"></a>
# **apiCapabilitiesImplementedEvidenceByClaimByClaimIdGet**
> PaginatedListIEnumerableCapabilitiesImplementedEvidence apiCapabilitiesImplementedEvidenceByClaimByClaimIdGet(claimId, opts)

Get all Evidence for the given Claim  Each list is a distinct &#39;chain&#39; of Evidence ie original Evidence with all subsequent Evidence  The first item in each &#39;chain&#39; is the most current Evidence.  The last item in each &#39;chain&#39; is the original Evidence.

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedEvidenceApi();

var claimId = "claimId_example"; // String | CRM identifier of Claim

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiCapabilitiesImplementedEvidenceByClaimByClaimIdGet(claimId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimId** | **String**| CRM identifier of Claim | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListIEnumerableCapabilitiesImplementedEvidence**](PaginatedListIEnumerableCapabilitiesImplementedEvidence.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiCapabilitiesImplementedEvidencePost"></a>
# **apiCapabilitiesImplementedEvidencePost**
> CapabilitiesImplementedEvidence apiCapabilitiesImplementedEvidencePost(opts)

Create a new evidence

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedEvidenceApi();

var opts = { 
  'evidence': new CatalogueApi.CapabilitiesImplementedEvidence() // CapabilitiesImplementedEvidence | new evidence information
};
apiInstance.apiCapabilitiesImplementedEvidencePost(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **evidence** | [**CapabilitiesImplementedEvidence**](CapabilitiesImplementedEvidence.md)| new evidence information | [optional] 

### Return type

[**CapabilitiesImplementedEvidence**](CapabilitiesImplementedEvidence.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: application/json

