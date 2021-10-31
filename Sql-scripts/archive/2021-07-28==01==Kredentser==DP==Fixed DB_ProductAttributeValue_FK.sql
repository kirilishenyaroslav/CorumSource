use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ProductAttributeValue]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductAttributeValue_Product_ProductAttribute_Mapping_ProductAttributeMappingId] FOREIGN KEY([ProductAttributeMappingId])
REFERENCES [dbo].[Product_ProductAttribute_Mapping] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductAttributeValue] NOCHECK CONSTRAINT [FK_ProductAttributeValue_Product_ProductAttribute_Mapping_ProductAttributeMappingId]
GO
select*from dbo.ProductAttributeValue;