use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ProductAttributeCombination]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductAttributeCombination_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductAttributeCombination] NOCHECK CONSTRAINT [FK_ProductAttributeCombination_Product_ProductId]
GO
select*from dbo.ProductAttributeCombination;