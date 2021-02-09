

ALTER TABLE [dbo].[OrderTruckTransport]  WITH CHECK ADD  CONSTRAINT [FK_OrderTruckTransport_ConsigneeCountries] FOREIGN KEY([ConsigneeCountryId])
REFERENCES [dbo].[Countries] ([?ode])
GO

ALTER TABLE [dbo].[OrderTruckTransport] CHECK CONSTRAINT [FK_OrderTruckTransport_ConsigneeCountries]
GO


ALTER TABLE [dbo].[OrderTruckTransport]  WITH CHECK ADD  CONSTRAINT [FK_OrderTruckTransport_ShipperCountries] FOREIGN KEY([ShipperCountryId])
REFERENCES [dbo].[Countries] ([?ode])
GO

ALTER TABLE [dbo].[OrderTruckTransport] CHECK CONSTRAINT [FK_OrderTruckTransport_ShipperCountries]
GO
