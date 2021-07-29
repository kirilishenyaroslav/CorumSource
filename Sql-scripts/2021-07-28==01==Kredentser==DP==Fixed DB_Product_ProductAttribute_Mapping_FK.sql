use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Product_ProductAttribute_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_ProductAttribute_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_ProductAttribute_Mapping] NOCHECK CONSTRAINT [FK_Product_ProductAttribute_Mapping_Product_ProductId]
GO
ALTER TABLE [dbo].[Product_ProductAttribute_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_ProductAttribute_Mapping_ProductAttribute_ProductAttributeId] FOREIGN KEY([ProductAttributeId])
REFERENCES [dbo].[ProductAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_ProductAttribute_Mapping] NOCHECK CONSTRAINT [FK_Product_ProductAttribute_Mapping_ProductAttribute_ProductAttributeId]
GO
select*from dbo.Product_ProductAttribute_Mapping;