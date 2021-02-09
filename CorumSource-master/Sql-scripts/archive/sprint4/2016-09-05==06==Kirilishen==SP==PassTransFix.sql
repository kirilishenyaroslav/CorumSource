alter table [dbo].[OrdersPassengerTransport]
add FromCountry int null;

GO

alter table [dbo].[OrdersPassengerTransport]
add ToCountry int null;

GO

alter table [dbo].[OrdersPassengerTransport]
add FromCity varchar(100) null;

GO

alter table [dbo].[OrdersPassengerTransport]
add ToCity varchar(100) null;

GO



ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH CHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_Countries] FOREIGN KEY([FromCountry])
REFERENCES [dbo].[Countries] ([Сode])
GO

ALTER TABLE [dbo].[OrdersPassengerTransport] CHECK CONSTRAINT [FK_OrdersPassengerTransport_Countries]
GO



ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH CHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_Countries1] FOREIGN KEY([ToCountry])
REFERENCES [dbo].[Countries] ([Сode])
GO

ALTER TABLE [dbo].[OrdersPassengerTransport] CHECK CONSTRAINT [FK_OrdersPassengerTransport_Countries1]
GO

