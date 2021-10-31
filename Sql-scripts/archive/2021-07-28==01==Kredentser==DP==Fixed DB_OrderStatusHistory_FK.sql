use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderStatusHistory] ADD  CONSTRAINT [DF_OrderStatusHistory_ChangeDateTime]  DEFAULT (getdate()) FOR [ChangeDateTime]
GO
ALTER TABLE [dbo].[OrderStatusHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderStatusHistory_AspNetUsers] FOREIGN KEY([ChangedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderStatusHistory] NOCHECK CONSTRAINT [FK_OrderStatusHistory_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderStatusHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderStatusHistory_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderStatusHistory] NOCHECK CONSTRAINT [FK_OrderStatusHistory_OrdersBase]
GO
ALTER TABLE [dbo].[OrderStatusHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderStatusHistory_OrderStatuses] FOREIGN KEY([OldStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderStatusHistory] NOCHECK CONSTRAINT [FK_OrderStatusHistory_OrderStatuses]
GO
ALTER TABLE [dbo].[OrderStatusHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderStatusHistory_OrderStatuses1] FOREIGN KEY([NewStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderStatusHistory] NOCHECK CONSTRAINT [FK_OrderStatusHistory_OrderStatuses1]
GO
select*from dbo.OrderStatusHistory;