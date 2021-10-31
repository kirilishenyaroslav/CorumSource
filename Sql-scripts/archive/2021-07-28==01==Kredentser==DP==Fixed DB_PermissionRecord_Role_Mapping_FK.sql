use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[PermissionRecord_Role_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionRecord_Role_Mapping_CustomerRole_CustomerRole_Id] FOREIGN KEY([CustomerRole_Id])
REFERENCES [dbo].[CustomerRole] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermissionRecord_Role_Mapping] NOCHECK CONSTRAINT [FK_PermissionRecord_Role_Mapping_CustomerRole_CustomerRole_Id]
GO
ALTER TABLE [dbo].[PermissionRecord_Role_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionRecord_Role_Mapping_PermissionRecord_PermissionRecord_Id] FOREIGN KEY([PermissionRecord_Id])
REFERENCES [dbo].[PermissionRecord] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermissionRecord_Role_Mapping] NOCHECK CONSTRAINT [FK_PermissionRecord_Role_Mapping_PermissionRecord_PermissionRecord_Id]
GO
select*from dbo.PermissionRecord_Role_Mapping;