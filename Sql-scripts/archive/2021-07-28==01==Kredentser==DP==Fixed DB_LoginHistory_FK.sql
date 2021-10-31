use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[LoginHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_LoginHistory_AspNetUsers1] FOREIGN KEY([userId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoginHistory] NOCHECK CONSTRAINT [FK_LoginHistory_AspNetUsers1]
GO
select*from dbo.LoginHistory;