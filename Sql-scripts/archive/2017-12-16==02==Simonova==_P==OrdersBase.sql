ALTER TABLE [dbo].[OrdersBase]
ALTER COLUMN CreatorPosition varchar(255)

go


ALTER TABLE [dbo].[OrdersPassengerTransport]
ALTER COLUMN FromCity varchar(255)

go


ALTER TABLE [dbo].[OrdersPassengerTransport]
ALTER COLUMN ToCity varchar(255)

go


ALTER TABLE [dbo].[OrderTruckTransport]
ALTER COLUMN ShipperCity varchar(255)

go

ALTER TABLE [dbo].[OrderTruckTransport]
ALTER COLUMN ConsigneeCity varchar(255)

go


ALTER TABLE [dbo].[OrderTruckTransport]
ALTER COLUMN ShipperContactPersonPhone varchar(255)

go

ALTER TABLE [dbo].[OrderTruckTransport]
ALTER COLUMN ConsigneeContactPersonPhone varchar(255)

go


