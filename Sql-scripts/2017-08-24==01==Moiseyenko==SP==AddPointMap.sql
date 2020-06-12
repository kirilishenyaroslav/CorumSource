alter table [dbo].[Organization]
add Latitude [decimal](9,6);
go

alter table [dbo].[Organization]
add Longitude [decimal](9,6);
go

alter table [dbo].[OrdersPassengerTransport]
add OrgFromId bigint;
go

alter table [dbo].[OrdersPassengerTransport]
add OrgToId bigint;
go

ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH CHECK ADD  CONSTRAINT [FK_Passenger_OrgFrom] FOREIGN KEY([OrgFromId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] CHECK CONSTRAINT [FK_Passenger_OrgFrom]
GO

ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH CHECK ADD  CONSTRAINT [FK_Passenger_OrgTo] FOREIGN KEY([OrgToId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] CHECK CONSTRAINT [FK_Passenger_OrgTo]
GO

alter table [dbo].[OrderTruckTransport]
add ShipperId bigint;
go

alter table [dbo].[OrderTruckTransport]
add ConsigneeId bigint;
go

ALTER TABLE [dbo].[OrderTruckTransport]  WITH CHECK ADD  CONSTRAINT [FK_Passenger_Shipper] FOREIGN KEY([ShipperId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] CHECK CONSTRAINT [FK_Passenger_Shipper]
GO

ALTER TABLE [dbo].[OrderTruckTransport]  WITH CHECK ADD  CONSTRAINT [FK_Passenger_Consignee] FOREIGN KEY([ConsigneeId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] CHECK CONSTRAINT [FK_Passenger_Consignee]
GO

alter table [dbo].[AdditionalRoutePoints]
add NumberPoint int;
go