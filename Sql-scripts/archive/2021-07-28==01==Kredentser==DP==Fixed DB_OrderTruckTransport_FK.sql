use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport] FOREIGN KEY([TruckTypeId])
REFERENCES [dbo].[OrderTruckTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport_ConsigneeCountries] FOREIGN KEY([ConsigneeCountryId])
REFERENCES [dbo].[Countries] ([Ñode])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport_ConsigneeCountries]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport_RouteTypes] FOREIGN KEY([TripType])
REFERENCES [dbo].[RouteTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport_RouteTypes]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport_ShipperCountries] FOREIGN KEY([ShipperCountryId])
REFERENCES [dbo].[Countries] ([Ñode])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport_ShipperCountries]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport2] FOREIGN KEY([VehicleTypeId])
REFERENCES [dbo].[OrderVehicleTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport2]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport3] FOREIGN KEY([LoadingTypeId])
REFERENCES [dbo].[OrderLoadingTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport3]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport4] FOREIGN KEY([UnloadingTypeId])
REFERENCES [dbo].[OrderUnloadingTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport4]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport5] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport5]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_Passenger_Consignee] FOREIGN KEY([ConsigneeId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_Passenger_Consignee]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_Passenger_Shipper] FOREIGN KEY([ShipperId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_Passenger_Shipper]
GO
select*from dbo.OrderTruckTransport;