use [uh417455_db2]
go
alter table [dbo].[OrderUsedCars] add IsSelected bit null;
go
select*from [dbo].[OrderUsedCars];