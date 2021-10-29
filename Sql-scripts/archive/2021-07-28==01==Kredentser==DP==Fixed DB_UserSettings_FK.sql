use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[UserSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_User] FOREIGN KEY([userId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserSettings] NOCHECK CONSTRAINT [FK_User]
GO
select*from dbo.UserSettings;