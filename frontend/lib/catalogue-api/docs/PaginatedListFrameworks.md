# CatalogueApi.PaginatedListFrameworks

## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**pageIndex** | **Number** | 1-based index of which page this page  Defaults to 1 | [optional] 
**totalPages** | **Number** | Total number of pages based on NHSD.GPITF.BuyingCatalog.Models.PaginatedList&#x60;1.PageSize | [optional] 
**pageSize** | **Number** | Maximum number of items in this page  Defaults to 20 | [optional] 
**items** | [**[Frameworks]**](Frameworks.md) | List of items | [optional] 
**hasPreviousPage** | **Boolean** | true if there is a page of items previous to this page | [optional] 
**hasNextPage** | **Boolean** | true if there is a page of items after this page | [optional] 


