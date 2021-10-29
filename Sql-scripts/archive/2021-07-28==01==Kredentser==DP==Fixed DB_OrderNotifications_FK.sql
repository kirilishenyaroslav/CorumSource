use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderNotifications]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderNotifications] FOREIGN KEY([Reciever])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderNotifications] NOCHECK CONSTRAINT [FK_OrderNotifications]
GO
ALTER TABLE [dbo].[OrderNotifications]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderNotificationsCreator] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderNotifications] NOCHECK CONSTRAINT [FK_OrderNotificationsCreator]
GO
ALTER TABLE [dbo].[OrderNotifications]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderNotificationsTypes] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrderNotificationTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderNotifications] NOCHECK CONSTRAINT [FK_OrderNotificationsTypes]
GO
select*from dbo.OrderNotifications;