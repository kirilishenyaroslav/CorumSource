use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Product_Picture_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Picture_Mapping_Picture_PictureId] FOREIGN KEY([PictureId])
REFERENCES [dbo].[Picture] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Picture_Mapping] NOCHECK CONSTRAINT [FK_Product_Picture_Mapping_Picture_PictureId]
GO
ALTER TABLE [dbo].[Product_Picture_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Picture_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Picture_Mapping] NOCHECK CONSTRAINT [FK_Product_Picture_Mapping_Product_ProductId]
GO
select*from dbo.Product_Picture_Mapping;