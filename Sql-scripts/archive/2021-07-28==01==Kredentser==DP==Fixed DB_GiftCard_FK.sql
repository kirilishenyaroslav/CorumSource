use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[GiftCard]  WITH NOCHECK ADD  CONSTRAINT [FK_GiftCard_OrderItem_PurchasedWithOrderItemId] FOREIGN KEY([PurchasedWithOrderItemId])
REFERENCES [dbo].[OrderItem] ([Id])
GO
ALTER TABLE [dbo].[GiftCard] NOCHECK CONSTRAINT [FK_GiftCard_OrderItem_PurchasedWithOrderItemId]
GO
select*from dbo.GiftCard;