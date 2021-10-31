use [uh417455_db2]
go
alter table [dbo].[OrderUsedCars] add tenderNumber int null;
go
select*from [dbo].[OrderUsedCars];