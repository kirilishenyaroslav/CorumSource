use [uh417455_db2]
go
alter table [dbo].[OrderUsedCars] add trailerNumber nvarchar(100) null;
go
alter table [dbo].[OrderUsedCars] add transportDimensions nvarchar(350) null;
go
alter table [dbo].[OrderUsedCars] add distance float null;
go
alter table [dbo].[OrderUsedCars] add stateBorderCrossingPoint text null;
go
alter table [dbo].[OrderUsedCars] add seriesPassportNumber nvarchar(350) null;
go
select*from [dbo].[OrderUsedCars];