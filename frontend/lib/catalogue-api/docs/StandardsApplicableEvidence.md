# CatalogueApi.StandardsApplicableEvidence

## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **String** | Unique identifier of entity | 
**previousId** | **String** | Unique identifier of previous version of entity | [optional] 
**claimId** | **String** | Unique identifier of Claim | 
**createdById** | **String** | Unique identifier of Contact who created record  Derived from calling context  SET ON SERVER | 
**createdOn** | **Date** | UTC date and time at which record created  Set by server when creating record  SET ON SERVER | [optional] 
**originalDate** | **Date** | UTC date and time at which record was originally created  Set by server when first creating record  SET ON SERVER | [optional] 
**evidence** | **String** | Serialised evidence data | [optional] 
**hasRequestedLiveDemo** | **Boolean** | true if supplier has requested to do a &#39;live demo&#39;  instead of submitting a file | [optional] 
**blobId** | **String** | unique identifier of binary file in blob storage system  NOTE:  this may not be a GUID eg it may be a URL  NOTE:  this is a GUID for SharePoint | [optional] 


