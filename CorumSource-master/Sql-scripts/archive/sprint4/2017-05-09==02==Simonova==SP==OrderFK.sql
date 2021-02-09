ALTER TABLE [dbo].[OrderTruckTransport]  WITH CHECK ADD  CONSTRAINT [FK_OrderTruckTransport_RouteTypes] FOREIGN KEY([TripType])
REFERENCES [dbo].[RouteTypes] ([Id])
GO

ALTER TABLE [dbo].[OrderTruckTransport] CHECK CONSTRAINT [FK_OrderTruckTransport_RouteTypes]
GO


ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH CHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_RouteTypes] FOREIGN KEY([TripType])
REFERENCES [dbo].[RouteTypes] ([Id])
GO

ALTER TABLE [dbo].[OrdersPassengerTransport] CHECK CONSTRAINT [FK_OrdersPassengerTransport_RouteTypes]
GO


