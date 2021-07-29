use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[UserProfile]  WITH NOCHECK ADD  CONSTRAINT [FK_UserProfile_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserProfile] NOCHECK CONSTRAINT [FK_UserProfile_AspNetUsers]
GO
ALTER TABLE [dbo].[UserProfile]  WITH NOCHECK ADD  CONSTRAINT [FK_UserProfile_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Ñode])
GO
ALTER TABLE [dbo].[UserProfile] NOCHECK CONSTRAINT [FK_UserProfile_Countries]
GO
select*from dbo.[UserProfile];