use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Product_ProductTag_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_ProductTag_Mapping_Product_Product_Id] FOREIGN KEY([Product_Id])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_ProductTag_Mapping] NOCHECK CONSTRAINT [FK_Product_ProductTag_Mapping_Product_Product_Id]
GO
ALTER TABLE [dbo].[Product_ProductTag_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_ProductTag_Mapping_ProductTag_ProductTag_Id] FOREIGN KEY([ProductTag_Id])
REFERENCES [dbo].[ProductTag] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_ProductTag_Mapping] NOCHECK CONSTRAINT [FK_Product_ProductTag_Mapping_ProductTag_ProductTag_Id]
GO
select*from dbo.Product_ProductTag_Mapping;