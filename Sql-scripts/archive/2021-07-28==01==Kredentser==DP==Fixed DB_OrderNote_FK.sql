use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderNote]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderNote_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderNote] NOCHECK CONSTRAINT [FK_OrderNote_Order_OrderId]
GO
select*from dbo.OrderNote;