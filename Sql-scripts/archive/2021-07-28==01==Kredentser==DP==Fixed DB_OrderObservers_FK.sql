use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderObservers]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderObservers_AspNetUsers] FOREIGN KEY([userId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderObservers] NOCHECK CONSTRAINT [FK_OrderObservers_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderObservers]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderObservers_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderObservers] NOCHECK CONSTRAINT [FK_OrderObservers_OrdersBase]
GO
select*from dbo.OrderObservers;