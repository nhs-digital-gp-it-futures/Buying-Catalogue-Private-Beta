# CatalogueApi.KeywordSearchHistoryApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiPorcelainKeywordSearchHistoryGet**](KeywordSearchHistoryApi.md#apiPorcelainKeywordSearchHistoryGet) | **GET** /api/porcelain/KeywordSearchHistory | Get keywords and how many times they occurred in a specified UTC date range


<a name="apiPorcelainKeywordSearchHistoryGet"></a>
# **apiPorcelainKeywordSearchHistoryGet**
> PaginatedListKeywordCount apiPorcelainKeywordSearchHistoryGet(opts)

Get keywords and how many times they occurred in a specified UTC date range

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

var apiInstance = new CatalogueApi.KeywordSearchHistoryApi();

var opts = { 
  'startDate': new Date("2013-10-20T19:20:30+01:00"), // Date | start of UTC date range eg 1965-05-15              Defaults to 10 years ago
  'endDate': new Date("2013-10-20T19:20:30+01:00"), // Date | end of UTC date range eg 2006-02-20              Defaults to 10 years from now
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiPorcelainKeywordSearchHistoryGet(opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **startDate** | **Date**| start of UTC date range eg 1965-05-15              Defaults to 10 years ago | [optional] 
 **endDate** | **Date**| end of UTC date range eg 2006-02-20              Defaults to 10 years from now | [optional] 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListKeywordCount**](PaginatedListKeywordCount.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

