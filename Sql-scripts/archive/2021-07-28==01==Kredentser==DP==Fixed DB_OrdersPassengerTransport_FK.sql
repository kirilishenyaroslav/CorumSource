use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_Countries] FOREIGN KEY([FromCountry])
REFERENCES [dbo].[Countries] ([Ñode])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_OrdersPassengerTransport_Countries]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_Countries1] FOREIGN KEY([ToCountry])
REFERENCES [dbo].[Countries] ([Ñode])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_OrdersPassengerTransport_Countries1]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_OrdersPassengerTransport_OrdersBase]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_RouteTypes] FOREIGN KEY([TripType])
REFERENCES [dbo].[RouteTypes] ([Id])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_OrdersPassengerTransport_RouteTypes]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_Passenger_OrgFrom] FOREIGN KEY([OrgFromId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_Passenger_OrgFrom]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_Passenger_OrgTo] FOREIGN KEY([OrgToId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_Passenger_OrgTo]
GO
select*from dbo.OrdersPassengerTransport;