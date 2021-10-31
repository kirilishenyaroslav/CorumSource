use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[StockQuantityHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_StockQuantityHistory_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockQuantityHistory] NOCHECK CONSTRAINT [FK_StockQuantityHistory_Product_ProductId]
GO
select*from dbo.StockQuantityHistory;