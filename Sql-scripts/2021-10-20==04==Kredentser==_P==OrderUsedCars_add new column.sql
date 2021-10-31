use [uh417455_db2]
go
alter table [dbo].[OrderUsedCars] add IsUpdate bit null;
go
select*from [dbo].[OrderUsedCars];