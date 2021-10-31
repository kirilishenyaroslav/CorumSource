use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Permissions]  WITH NOCHECK ADD  CONSTRAINT [FK_Permissions_AspNetRoles] FOREIGN KEY([roleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[Permissions] NOCHECK CONSTRAINT [FK_Permissions_AspNetRoles]
GO
ALTER TABLE [dbo].[Permissions]  WITH NOCHECK ADD  CONSTRAINT [FK_Permissions_MenuStructure] FOREIGN KEY([menuId])
REFERENCES [dbo].[MenuStructure] ([Id])
GO
ALTER TABLE [dbo].[Permissions] NOCHECK CONSTRAINT [FK_Permissions_MenuStructure]
GO
select*from dbo.[Permissions];