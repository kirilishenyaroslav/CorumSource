use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderClients]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderClients_AspNetRoles] FOREIGN KEY([AccessRoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderClients] NOCHECK CONSTRAINT [FK_OrderClients_AspNetRoles]
GO
ALTER TABLE [dbo].[OrderClients]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderClients_Centers] FOREIGN KEY([ClientCFOId])
REFERENCES [dbo].[Centers] ([Id])
GO
ALTER TABLE [dbo].[OrderClients] NOCHECK CONSTRAINT [FK_OrderClients_Centers]
GO

select*from dbo.OrderClients;