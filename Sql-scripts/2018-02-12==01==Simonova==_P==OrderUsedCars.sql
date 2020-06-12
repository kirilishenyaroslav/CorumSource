alter table [dbo].[OrderUsedCars]
add FactShipperDateTime datetime2(7);

alter table [dbo].[OrderUsedCars]
add FactConsigneeDateTime datetime2(7);

go