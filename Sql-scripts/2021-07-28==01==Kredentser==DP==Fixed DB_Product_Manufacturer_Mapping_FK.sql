use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Product_Manufacturer_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Manufacturer_Mapping_Manufacturer_ManufacturerId] FOREIGN KEY([ManufacturerId])
REFERENCES [dbo].[Manufacturer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Manufacturer_Mapping] NOCHECK CONSTRAINT [FK_Product_Manufacturer_Mapping_Manufacturer_ManufacturerId]
GO
ALTER TABLE [dbo].[Product_Manufacturer_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Manufacturer_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Manufacturer_Mapping] NOCHECK CONSTRAINT [FK_Product_Manufacturer_Mapping_Product_ProductId]
GO
select*from dbo.Product_Manufacturer_Mapping;