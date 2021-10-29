use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[DiscountUsageHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_DiscountUsageHistory_Discount_DiscountId] FOREIGN KEY([DiscountId])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiscountUsageHistory] NOCHECK CONSTRAINT [FK_DiscountUsageHistory_Discount_DiscountId]
GO
ALTER TABLE [dbo].[DiscountUsageHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_DiscountUsageHistory_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiscountUsageHistory] NOCHECK CONSTRAINT [FK_DiscountUsageHistory_Order_OrderId]
GO
select*from dbo.DiscountUsageHistory;