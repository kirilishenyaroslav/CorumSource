use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Product_Category_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Category_Mapping_Category_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Category_Mapping] NOCHECK CONSTRAINT [FK_Product_Category_Mapping_Category_CategoryId]
GO
ALTER TABLE [dbo].[Product_Category_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Category_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Category_Mapping] NOCHECK CONSTRAINT [FK_Product_Category_Mapping_Product_ProductId]
GO
select*from dbo.Product_Category_Mapping;