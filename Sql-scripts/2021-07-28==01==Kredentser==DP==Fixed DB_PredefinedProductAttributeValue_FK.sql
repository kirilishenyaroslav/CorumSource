use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[PredefinedProductAttributeValue]  WITH NOCHECK ADD  CONSTRAINT [FK_PredefinedProductAttributeValue_ProductAttribute_ProductAttributeId] FOREIGN KEY([ProductAttributeId])
REFERENCES [dbo].[ProductAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PredefinedProductAttributeValue] NOCHECK CONSTRAINT [FK_PredefinedProductAttributeValue_ProductAttribute_ProductAttributeId]
GO
select*from dbo.PredefinedProductAttributeValue;