use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[AspNetMenuRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetMenuRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetMenuRoles] CHECK CONSTRAINT [FK_dbo.AspNetMenuRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetMenuRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetMenuRoles_dbo.MenuStructure_Id] FOREIGN KEY([MenuId])
REFERENCES [dbo].[MenuStructure] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetMenuRoles] CHECK CONSTRAINT [FK_dbo.AspNetMenuRoles_dbo.MenuStructure_Id]
GO
select*from dbo.AspNetMenuRoles;