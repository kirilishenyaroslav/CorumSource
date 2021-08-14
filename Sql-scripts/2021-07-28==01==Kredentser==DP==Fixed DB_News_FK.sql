use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[News]  WITH NOCHECK ADD  CONSTRAINT [FK_News_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[News] NOCHECK CONSTRAINT [FK_News_Language_LanguageId]
GO
select*from dbo.News;