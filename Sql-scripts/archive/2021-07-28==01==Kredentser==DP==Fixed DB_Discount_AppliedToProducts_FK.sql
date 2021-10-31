use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Discount_AppliedToProducts]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToProducts_Discount_Discount_Id] FOREIGN KEY([Discount_Id])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToProducts] NOCHECK CONSTRAINT [FK_Discount_AppliedToProducts_Discount_Discount_Id]
GO
ALTER TABLE [dbo].[Discount_AppliedToProducts]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToProducts_Product_Product_Id] FOREIGN KEY([Product_Id])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToProducts] NOCHECK CONSTRAINT [FK_Discount_AppliedToProducts_Product_Product_Id]
GO
select*from dbo.Discount_AppliedToProducts;