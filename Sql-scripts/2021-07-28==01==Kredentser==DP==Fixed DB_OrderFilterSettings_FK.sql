use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_AspNetUsers] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_AspNetUsersCur] FOREIGN KEY([IdCurrentUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_AspNetUsersCur]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_AspNetUsersEx] FOREIGN KEY([ExecuterId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_AspNetUsersEx]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_OrderClients]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_OrderStatuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_OrderStatuses]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_OrderTypesBase] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_OrderTypesBase]
GO
select*from dbo.OrderFilterSettings;