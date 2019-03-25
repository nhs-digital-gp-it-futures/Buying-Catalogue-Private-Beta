# CatalogueApi.ContactsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**apiContactsByEmailByEmailGet**](ContactsApi.md#apiContactsByEmailByEmailGet) | **GET** /api/Contacts/ByEmail/{email} | Retrieve a contacts for an organisation, given the contact’s email address  Email address is case insensitive
[**apiContactsByIdByIdGet**](ContactsApi.md#apiContactsByIdByIdGet) | **GET** /api/Contacts/ById/{id} | Retrieve a contact for an organisation, given the contact’s CRM identifier
[**apiContactsByOrganisationByOrganisationIdGet**](ContactsApi.md#apiContactsByOrganisationByOrganisationIdGet) | **GET** /api/Contacts/ByOrganisation/{organisationId} | Retrieve all contacts for an organisation in a paged list, given the organisation’s CRM identifier


<a name="apiContactsByEmailByEmailGet"></a>
# **apiContactsByEmailByEmailGet**
> Contacts apiContactsByEmailByEmailGet(email)

Retrieve a contacts for an organisation, given the contact’s email address  Email address is case insensitive

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

var apiInstance = new CatalogueApi.ContactsApi();

var email = "email_example"; // String | email address to search for

apiInstance.apiContactsByEmailByEmailGet(email).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **email** | **String**| email address to search for | 

### Return type

[**Contacts**](Contacts.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiContactsByIdByIdGet"></a>
# **apiContactsByIdByIdGet**
> Contacts apiContactsByIdByIdGet(id)

Retrieve a contact for an organisation, given the contact’s CRM identifier

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

var apiInstance = new CatalogueApi.ContactsApi();

var id = "id_example"; // String | CRM identifier of contact

apiInstance.apiContactsByIdByIdGet(id).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **String**| CRM identifier of contact | 

### Return type

[**Contacts**](Contacts.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="apiContactsByOrganisationByOrganisationIdGet"></a>
# **apiContactsByOrganisationByOrganisationIdGet**
> PaginatedListContacts apiContactsByOrganisationByOrganisationIdGet(organisationId, opts)

Retrieve all contacts for an organisation in a paged list, given the organisation’s CRM identifier

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

var apiInstance = new CatalogueApi.ContactsApi();

var organisationId = "organisationId_example"; // String | CRM identifier of organisation

var opts = { 
  'pageIndex': 56, // Number | 1-based index of page to return.  Defaults to 1
  'pageSize': 56 // Number | number of items per page.  Defaults to 20
};
apiInstance.apiContactsByOrganisationByOrganisationIdGet(organisationId, opts).then(function(data) {
  console.log('API called successfully. Returned data: ' + data);
}, function(error) {
  console.error(error);
});

```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **organisationId** | **String**| CRM identifier of organisation | 
 **pageIndex** | **Number**| 1-based index of page to return.  Defaults to 1 | [optional] 
 **pageSize** | **Number**| number of items per page.  Defaults to 20 | [optional] 

### Return type

[**PaginatedListContacts**](PaginatedListContacts.md)

### Authorization

[basic](../README.md#basic), [oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

