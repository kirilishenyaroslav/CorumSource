use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ShipmentItem]  WITH NOCHECK ADD  CONSTRAINT [FK_ShipmentItem_Shipment_ShipmentId] FOREIGN KEY([ShipmentId])
REFERENCES [dbo].[Shipment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShipmentItem] NOCHECK CONSTRAINT [FK_ShipmentItem_Shipment_ShipmentId]
GO
select*from dbo.ShipmentItem;