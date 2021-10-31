use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[GiftCardUsageHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_GiftCardUsageHistory_GiftCard_GiftCardId] FOREIGN KEY([GiftCardId])
REFERENCES [dbo].[GiftCard] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GiftCardUsageHistory] NOCHECK CONSTRAINT [FK_GiftCardUsageHistory_GiftCard_GiftCardId]
GO
ALTER TABLE [dbo].[GiftCardUsageHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_GiftCardUsageHistory_Order_UsedWithOrderId] FOREIGN KEY([UsedWithOrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GiftCardUsageHistory] NOCHECK CONSTRAINT [FK_GiftCardUsageHistory_Order_UsedWithOrderId]
GO
select*from dbo.GiftCardUsageHistory;