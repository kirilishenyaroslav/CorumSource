use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[RoleGroupsRole]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.RoleGroupsRole_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleGroupsRole] NOCHECK CONSTRAINT [FK_dbo.RoleGroupsRole_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[RoleGroupsRole]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.RoleGroupsRole_dbo.RoleGroups_RoleGroupsId] FOREIGN KEY([RoleGroupsId])
REFERENCES [dbo].[RoleGroups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleGroupsRole] NOCHECK CONSTRAINT [FK_dbo.RoleGroupsRole_dbo.RoleGroups_RoleGroupsId]
GO
select*from dbo.RoleGroupsRole;