# CatalogueApi.CapabilitiesImplementedReviewsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiCapabilitiesImplementedReviewsByEvidenceByEvidenceIdGet**](CapabilitiesImplementedReviewsApi.md#apiCapabilitiesImplementedReviewsByEvidenceByEvidenceIdGet) | **GET** /api/CapabilitiesImplementedReviews/ByEvidence/{evidenceId} | Get all Reviews for a CapabilitiesImplemented  Each list is a distinct &#39;chain&#39; of Review ie original Review with all subsequent Review  The first item in each &#39;chain&#39; is the most current Review.  The last item in each &#39;chain&#39; is the original Review.
[**apiCapabilitiesImplementedReviewsPost**](CapabilitiesImplementedReviewsApi.md#apiCapabilitiesImplementedReviewsPost) | **POST** /api/CapabilitiesImplementedReviews | Create a new Review for a CapabilitiesImplemented


<a name="apiCapabilitiesImplementedReviewsByEvidenceByEvidenceIdGet"></a>
# **apiCapabilitiesImplementedReviewsByEvidenceByEvidenceIdGet**
> PaginatedListIEnumerableCapabilitiesImplementedReviews apiCapabilitiesImplementedReviewsByEvidenceByEvidenceIdGet(evidenceId, opts)

Get all Reviews for a CapabilitiesImplemented  Each list is a distinct &#39;chain&#39; of Review ie original Review with all subsequent Review  The first item in each &#39;chain&#39; is the most current Review.  The last item in each &#39;chain&#39; is the original Review.

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedReviewsApi();

var evidenceId = "evidenceId_example"; // String | CRM identifier of CapabilitiesImplementedEvidence

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiCapabilitiesImplementedReviewsByEvidenceByEvidenceIdGet(evidenceId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **evidenceId** | **String**| CRM identifier of CapabilitiesImplementedEvidence | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListIEnumerableCapabilitiesImplementedReviews**](PaginatedListIEnumerableCapabilitiesImplementedReviews.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiCapabilitiesImplementedReviewsPost"></a>
# **apiCapabilitiesImplementedReviewsPost**
> CapabilitiesImplementedReviews apiCapabilitiesImplementedReviewsPost(opts)

Create a new Review for a CapabilitiesImplemented

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedReviewsApi();

var opts = { 
  'review': new CatalogueApi.CapabilitiesImplementedReviews() // CapabilitiesImplementedReviews | new Review information
};
apiInstance.apiCapabilitiesImplementedReviewsPost(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **review** | [**CapabilitiesImplementedReviews**](CapabilitiesImplementedReviews.md)| new Review information | [optional] 

### Return type

[**CapabilitiesImplementedReviews**](CapabilitiesImplementedReviews.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: application/json

