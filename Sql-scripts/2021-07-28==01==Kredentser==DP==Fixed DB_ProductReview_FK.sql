use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ProductReview]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview] NOCHECK CONSTRAINT [FK_ProductReview_Customer_CustomerId]
GO
ALTER TABLE [dbo].[ProductReview]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview] NOCHECK CONSTRAINT [FK_ProductReview_Product_ProductId]
GO
ALTER TABLE [dbo].[ProductReview]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview] NOCHECK CONSTRAINT [FK_ProductReview_Store_StoreId]
GO
select*from dbo.ProductReview;