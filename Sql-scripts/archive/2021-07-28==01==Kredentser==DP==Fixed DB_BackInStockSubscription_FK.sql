use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[BackInStockSubscription]  WITH CHECK ADD  CONSTRAINT [FK_BackInStockSubscription_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BackInStockSubscription] CHECK CONSTRAINT [FK_BackInStockSubscription_Customer_CustomerId]
GO
ALTER TABLE [dbo].[BackInStockSubscription]  WITH CHECK ADD  CONSTRAINT [FK_BackInStockSubscription_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BackInStockSubscription] CHECK CONSTRAINT [FK_BackInStockSubscription_Product_ProductId]
GO
select*from dbo.BackInStockSubscription;