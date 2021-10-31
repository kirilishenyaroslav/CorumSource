use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ProductWarehouseInventory]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductWarehouseInventory_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductWarehouseInventory] NOCHECK CONSTRAINT [FK_ProductWarehouseInventory_Product_ProductId]
GO
ALTER TABLE [dbo].[ProductWarehouseInventory]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductWarehouseInventory_Warehouse_WarehouseId] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[Warehouse] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductWarehouseInventory] NOCHECK CONSTRAINT [FK_ProductWarehouseInventory_Warehouse_WarehouseId]
GO
select*from dbo.ProductWarehouseInventory;