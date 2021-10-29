use [uh417455_db2]
go
alter table [dbo].[OrderUsedCars] add drivingLicenseNumber nvarchar(350) null;
go
select*from [dbo].[OrderUsedCars];