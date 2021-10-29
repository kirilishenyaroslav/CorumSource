use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ShoppingCartItem]  WITH NOCHECK ADD  CONSTRAINT [FK_ShoppingCartItem_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoppingCartItem] NOCHECK CONSTRAINT [FK_ShoppingCartItem_Customer_CustomerId]
GO
ALTER TABLE [dbo].[ShoppingCartItem]  WITH NOCHECK ADD  CONSTRAINT [FK_ShoppingCartItem_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoppingCartItem] NOCHECK CONSTRAINT [FK_ShoppingCartItem_Product_ProductId]
GO
select*from dbo.ShoppingCartItem;