use [uh417455_db2]
go
alter table [dbo].[OrderUsedCars] add fullMassTC int null;
go
alter table [dbo].[OrderUsedCars] add massWithoutLoadTC1 int null;
go
alter table [dbo].[OrderUsedCars] add fullMassTC2Trailer int null;
go
alter table [dbo].[OrderUsedCars] add massWithoutLoadTC2Trailer int null;
go
select*from [dbo].[OrderUsedCars];