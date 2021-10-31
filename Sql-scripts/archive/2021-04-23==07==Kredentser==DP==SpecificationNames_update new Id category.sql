use [Corum.Prod-2021-01-30]
go

update dbo.SpecificationNames
set industryId = 67, industryId_Test = 57 where Id >= 54 and Id <= 59
go

select*from dbo.SpecificationNames;
