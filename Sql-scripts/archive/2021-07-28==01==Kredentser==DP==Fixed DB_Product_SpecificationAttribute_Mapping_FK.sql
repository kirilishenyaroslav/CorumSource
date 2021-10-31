use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Product_SpecificationAttribute_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_SpecificationAttribute_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_SpecificationAttribute_Mapping] NOCHECK CONSTRAINT [FK_Product_SpecificationAttribute_Mapping_Product_ProductId]
GO
ALTER TABLE [dbo].[Product_SpecificationAttribute_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_SpecificationAttribute_Mapping_SpecificationAttributeOption_SpecificationAttributeOptionId] FOREIGN KEY([SpecificationAttributeOptionId])
REFERENCES [dbo].[SpecificationAttributeOption] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_SpecificationAttribute_Mapping] NOCHECK CONSTRAINT [FK_Product_SpecificationAttribute_Mapping_SpecificationAttributeOption_SpecificationAttributeOptionId]
GO

select*from dbo.Product_SpecificationAttribute_Mapping;