# CatalogueApi.SearchApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiPorcelainSearchByKeywordByKeywordGet**](SearchApi.md#apiPorcelainSearchByKeywordByKeywordGet) | **GET** /api/porcelain/Search/ByKeyword/{keyword} | Get existing solution/s which are related to the given keyword &lt;br /&gt;  Keyword is not case sensitive &lt;br /&gt;  Capabilities are searched for capabilities which contain  the keyword in the capability name or descriptions.  This  forms a set of desired capabilities. &lt;br /&gt;  These desired capabilities are then matched to solution/s which  implement at least one of the desired capabilities. &lt;br /&gt;  An &#39;ideal&#39; solution would be one which only implements all  of the desired capabilities. &lt;br /&gt;  Each solution is given a &#39;distance&#39; which represents how many  different capabilites the solution implements, compared to the  set of desired capabilities: &lt;br /&gt;    zero     &#x3D;&#x3D; solution has exactly capabilities desired &lt;br /&gt;    positive &#x3D;&#x3D; solution has more capabilities than desired &lt;br /&gt;    negative &#x3D;&#x3D; solution has less capabilities than desired &lt;br /&gt;


<a name="apiPorcelainSearchByKeywordByKeywordGet"></a>
# **apiPorcelainSearchByKeywordByKeywordGet**
> PaginatedListSearchResult apiPorcelainSearchByKeywordByKeywordGet(keyword, opts)

Get existing solution/s which are related to the given keyword &lt;br /&gt;  Keyword is not case sensitive &lt;br /&gt;  Capabilities are searched for capabilities which contain  the keyword in the capability name or descriptions.  This  forms a set of desired capabilities. &lt;br /&gt;  These desired capabilities are then matched to solution/s which  implement at least one of the desired capabilities. &lt;br /&gt;  An &#39;ideal&#39; solution would be one which only implements all  of the desired capabilities. &lt;br /&gt;  Each solution is given a &#39;distance&#39; which represents how many  different capabilites the solution implements, compared to the  set of desired capabilities: &lt;br /&gt;    zero     &#x3D;&#x3D; solution has exactly capabilities desired &lt;br /&gt;    positive &#x3D;&#x3D; solution has more capabilities than desired &lt;br /&gt;    negative &#x3D;&#x3D; solution has less capabilities than desired &lt;br /&gt;

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

var apiInstance = new CatalogueApi.SearchApi();

var keyword = "keyword_example"; // String | keyword describing a solution or capability

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiPorcelainSearchByKeywordByKeywordGet(keyword, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **keyword** | **String**| keyword describing a solution or capability | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListSearchResult**](PaginatedListSearchResult.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

