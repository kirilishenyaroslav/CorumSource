use [uh417455_db2]
go
alter table [dbo].[OrderUsedCars] add Summ_ float null;
go
select*from [dbo].[OrderUsedCars];