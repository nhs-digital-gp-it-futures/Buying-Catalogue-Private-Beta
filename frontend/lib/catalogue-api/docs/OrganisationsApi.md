# CatalogueApi.OrganisationsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiOrganisationsByContactByContactIdGet**](OrganisationsApi.md#apiOrganisationsByContactByContactIdGet) | **GET** /api/Organisations/ByContact/{contactId} | Retrieve an Organisation for the given Contact


<a name="apiOrganisationsByContactByContactIdGet"></a>
# **apiOrganisationsByContactByContactIdGet**
> Organisations apiOrganisationsByContactByContactIdGet(contactId)

Retrieve an Organisation for the given Contact

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

var apiInstance = new CatalogueApi.OrganisationsApi();

var contactId = "contactId_example"; // String | CRM identifier of Contact

apiInstance.apiOrganisationsByContactByContactIdGet(contactId).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **contactId** | **String**| CRM identifier of Contact | 

### Return type

[**Organisations**](Organisations.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

