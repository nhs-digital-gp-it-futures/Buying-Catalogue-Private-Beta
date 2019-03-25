# CatalogueApi.StandardsApplicableReviewsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiStandardsApplicableReviewsByEvidenceByEvidenceIdGet**](StandardsApplicableReviewsApi.md#apiStandardsApplicableReviewsByEvidenceByEvidenceIdGet) | **GET** /api/StandardsApplicableReviews/ByEvidence/{evidenceId} | Get all Reviews for a StandardsApplicable  Each list is a distinct &#39;chain&#39; of Review ie original Review with all subsequent Review  The first item in each &#39;chain&#39; is the most current Review.  The last item in each &#39;chain&#39; is the original Review.
[**apiStandardsApplicableReviewsPost**](StandardsApplicableReviewsApi.md#apiStandardsApplicableReviewsPost) | **POST** /api/StandardsApplicableReviews | Create a new Review for a StandardsApplicable


<a name="apiStandardsApplicableReviewsByEvidenceByEvidenceIdGet"></a>
# **apiStandardsApplicableReviewsByEvidenceByEvidenceIdGet**
> PaginatedListIEnumerableStandardsApplicableReviews apiStandardsApplicableReviewsByEvidenceByEvidenceIdGet(evidenceId, opts)

Get all Reviews for a StandardsApplicable  Each list is a distinct &#39;chain&#39; of Review ie original Review with all subsequent Review  The first item in each &#39;chain&#39; is the most current Review.  The last item in each &#39;chain&#39; is the original Review.

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

var apiInstance = new CatalogueApi.StandardsApplicableReviewsApi();

var evidenceId = "evidenceId_example"; // String | CRM identifier of StandardsApplicableEvidence

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiStandardsApplicableReviewsByEvidenceByEvidenceIdGet(evidenceId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **evidenceId** | **String**| CRM identifier of StandardsApplicableEvidence | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListIEnumerableStandardsApplicableReviews**](PaginatedListIEnumerableStandardsApplicableReviews.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiStandardsApplicableReviewsPost"></a>
# **apiStandardsApplicableReviewsPost**
> StandardsApplicableReviews apiStandardsApplicableReviewsPost(opts)

Create a new Review for a StandardsApplicable

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

var apiInstance = new CatalogueApi.StandardsApplicableReviewsApi();

var opts = { 
  'review': new CatalogueApi.StandardsApplicableReviews() // StandardsApplicableReviews | new Review information
};
apiInstance.apiStandardsApplicableReviewsPost(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **review** | [**StandardsApplicableReviews**](StandardsApplicableReviews.md)| new Review information | [optional] 

### Return type

[**StandardsApplicableReviews**](StandardsApplicableReviews.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: application/json

