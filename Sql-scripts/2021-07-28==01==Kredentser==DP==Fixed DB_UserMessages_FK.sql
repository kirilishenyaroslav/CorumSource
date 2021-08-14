use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[UserMessages]  WITH NOCHECK ADD  CONSTRAINT [FK_PK_UserMessages_AspNetUsers] FOREIGN KEY([CreatedFromUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserMessages] NOCHECK CONSTRAINT [FK_PK_UserMessages_AspNetUsers]
GO
ALTER TABLE [dbo].[UserMessages]  WITH NOCHECK ADD  CONSTRAINT [FK_PK_UserMessages_AspNetUsers2] FOREIGN KEY([CreatedToUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserMessages] NOCHECK CONSTRAINT [FK_PK_UserMessages_AspNetUsers2]
GO
select*from dbo.UserMessages;