use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Poll]  WITH NOCHECK ADD  CONSTRAINT [FK_Poll_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Poll] NOCHECK CONSTRAINT [FK_Poll_Language_LanguageId]
GO
select*from dbo.Poll;