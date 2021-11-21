use [uh417455_db2]
go
alter table [dbo].[OrderUsedCars] add tenderTureNumber int null;
go
select*from [dbo].[OrderUsedCars];