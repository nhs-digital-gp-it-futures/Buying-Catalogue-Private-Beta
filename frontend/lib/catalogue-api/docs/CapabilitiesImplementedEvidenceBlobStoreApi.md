# CatalogueApi.CapabilitiesImplementedEvidenceBlobStoreApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiCapabilitiesImplementedEvidenceBlobStoreAddEvidenceForClaimPost**](CapabilitiesImplementedEvidenceBlobStoreApi.md#apiCapabilitiesImplementedEvidenceBlobStoreAddEvidenceForClaimPost) | **POST** /api/CapabilitiesImplementedEvidenceBlobStore/AddEvidenceForClaim | Upload a file to support a claim  If the file already exists on the server, then a new version is created
[**apiCapabilitiesImplementedEvidenceBlobStoreDownloadByClaimIdPost**](CapabilitiesImplementedEvidenceBlobStoreApi.md#apiCapabilitiesImplementedEvidenceBlobStoreDownloadByClaimIdPost) | **POST** /api/CapabilitiesImplementedEvidenceBlobStore/Download/{claimId} | Download a file which is supporting a claim
[**apiCapabilitiesImplementedEvidenceBlobStoreEnumerateClaimFolderTreeBySolutionIdGet**](CapabilitiesImplementedEvidenceBlobStoreApi.md#apiCapabilitiesImplementedEvidenceBlobStoreEnumerateClaimFolderTreeBySolutionIdGet) | **GET** /api/CapabilitiesImplementedEvidenceBlobStore/EnumerateClaimFolderTree/{solutionId} | List all claim files and sub-folders for a solution
[**apiCapabilitiesImplementedEvidenceBlobStoreEnumerateFolderByClaimIdGet**](CapabilitiesImplementedEvidenceBlobStoreApi.md#apiCapabilitiesImplementedEvidenceBlobStoreEnumerateFolderByClaimIdGet) | **GET** /api/CapabilitiesImplementedEvidenceBlobStore/EnumerateFolder/{claimId} | List all files and sub-folders for a claim including folder for claim


<a name="apiCapabilitiesImplementedEvidenceBlobStoreAddEvidenceForClaimPost"></a>
# **apiCapabilitiesImplementedEvidenceBlobStoreAddEvidenceForClaimPost**
> &#39;String&#39; apiCapabilitiesImplementedEvidenceBlobStoreAddEvidenceForClaimPost(claimId, file, filename, opts)

Upload a file to support a claim  If the file already exists on the server, then a new version is created

Server side folder structure is something like:  --Organisation  ----Solution  ------Capability Evidence  --------Appointment Management - Citizen  --------Appointment Management - GP  --------Clinical Decision Support  --------[all other claimed capabilities]  ----------Images  ----------PDF  ----------Videos  ----------RTM  ----------Misc                where subFolder is an optional folder under a claimed capability ie Images, PDF, et al

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedEvidenceBlobStoreApi();

var claimId = "claimId_example"; // String | Unique identifier of claim

var file = "/path/to/file.txt"; // File | Client file path

var filename = "filename_example"; // String | Server file name

var opts = { 
  'subFolder': "subFolder_example" // String | optional sub-folder
};
apiInstance.apiCapabilitiesImplementedEvidenceBlobStoreAddEvidenceForClaimPost(claimId, file, filename, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimId** | **String**| Unique identifier of claim | 
 **file** | **File**| Client file path | 
 **filename** | **String**| Server file name | 
 **subFolder** | **String**| optional sub-folder | [optional] 

### Return type

**&#39;String&#39;**

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/json

<a name="apiCapabilitiesImplementedEvidenceBlobStoreDownloadByClaimIdPost"></a>
# **apiCapabilitiesImplementedEvidenceBlobStoreDownloadByClaimIdPost**
> FileResult apiCapabilitiesImplementedEvidenceBlobStoreDownloadByClaimIdPost(claimId, opts)

Download a file which is supporting a claim

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedEvidenceBlobStoreApi();

var claimId = "claimId_example"; // String | unique identifier of solution claim

var opts = { 
  'uniqueId': "uniqueId_example" // String | unique identifier of file
};
apiInstance.apiCapabilitiesImplementedEvidenceBlobStoreDownloadByClaimIdPost(claimId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimId** | **String**| unique identifier of solution claim | 
 **uniqueId** | **String**| unique identifier of file | [optional] 

### Return type

[**FileResult**](FileResult.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

<a name="apiCapabilitiesImplementedEvidenceBlobStoreEnumerateClaimFolderTreeBySolutionIdGet"></a>
# **apiCapabilitiesImplementedEvidenceBlobStoreEnumerateClaimFolderTreeBySolutionIdGet**
> PaginatedListClaimBlobInfoMap apiCapabilitiesImplementedEvidenceBlobStoreEnumerateClaimFolderTreeBySolutionIdGet(solutionId, opts)

List all claim files and sub-folders for a solution

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedEvidenceBlobStoreApi();

var solutionId = "solutionId_example"; // String | unique identifier of solution

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiCapabilitiesImplementedEvidenceBlobStoreEnumerateClaimFolderTreeBySolutionIdGet(solutionId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **solutionId** | **String**| unique identifier of solution | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListClaimBlobInfoMap**](PaginatedListClaimBlobInfoMap.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiCapabilitiesImplementedEvidenceBlobStoreEnumerateFolderByClaimIdGet"></a>
# **apiCapabilitiesImplementedEvidenceBlobStoreEnumerateFolderByClaimIdGet**
> PaginatedListBlobInfo apiCapabilitiesImplementedEvidenceBlobStoreEnumerateFolderByClaimIdGet(claimId, opts)

List all files and sub-folders for a claim including folder for claim

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

var apiInstance = new CatalogueApi.CapabilitiesImplementedEvidenceBlobStoreApi();

var claimId = "claimId_example"; // String | unique identifier of solution claim

var opts = { 
  'subFolder': "subFolder_example", // String | optional sub-folder under claim
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiCapabilitiesImplementedEvidenceBlobStoreEnumerateFolderByClaimIdGet(claimId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **claimId** | **String**| unique identifier of solution claim | 
 **subFolder** | **String**| optional sub-folder under claim | [optional] 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListBlobInfo**](PaginatedListBlobInfo.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

