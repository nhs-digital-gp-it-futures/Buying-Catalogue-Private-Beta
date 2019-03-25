# CatalogueApi.Solutions

## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **String** | Unique identifier of entity | 
**previousId** | **String** | Unique identifier of previous version of entity | [optional] 
**organisationId** | **String** | Unique identifier of organisation | 
**version** | **String** | Version of solution | [optional] 
**status** | **String** | Current status of this solution | [optional] 
**createdById** | **String** | Unique identifier of Contact who created this entity  Derived from calling context  SET ON SERVER | 
**createdOn** | **Date** | UTC date and time at which record created  Set by server when creating record  SET ON SERVER | [optional] 
**modifiedById** | **String** | Unique identifier of Contact who last modified this entity  Derived from calling context  SET ON SERVER | 
**modifiedOn** | **Date** | UTC date and time at which record last modified  Set by server when creating record  SET ON SERVER | [optional] 
**name** | **String** | Name of solution, as displayed to a user | [optional] 
**description** | **String** | Description of solution, as displayed to a user | [optional] 
**productPage** | **String** | Serialised product page | [optional] 


<a name="StatusEnum"></a>
## Enum: StatusEnum


* `Failed` (value: `"Failed"`)

* `Draft` (value: `"Draft"`)

* `Registered` (value: `"Registered"`)

* `CapabilitiesAssessment` (value: `"CapabilitiesAssessment"`)

* `StandardsCompliance` (value: `"StandardsCompliance"`)

* `FinalApproval` (value: `"FinalApproval"`)

* `SolutionPage` (value: `"SolutionPage"`)

* `Approved` (value: `"Approved"`)




