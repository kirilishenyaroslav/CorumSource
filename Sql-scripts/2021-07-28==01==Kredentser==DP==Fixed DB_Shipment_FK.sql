use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Shipment]  WITH NOCHECK ADD  CONSTRAINT [FK_Shipment_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Shipment] NOCHECK CONSTRAINT [FK_Shipment_Order_OrderId]
GO
select*from dbo.Shipment;