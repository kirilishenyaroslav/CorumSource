use [uh417455_db2]
go
alter table [dbo].[OrderUsedCars] add formUuid uniqueidentifier null;
go
select*from [dbo].[OrderUsedCars];