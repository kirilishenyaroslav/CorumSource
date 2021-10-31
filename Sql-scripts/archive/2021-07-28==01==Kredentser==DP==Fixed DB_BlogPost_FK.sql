use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[BlogPost]  WITH CHECK ADD  CONSTRAINT [FK_BlogPost_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogPost] CHECK CONSTRAINT [FK_BlogPost_Language_LanguageId]
GO
select*from dbo.BlogPost;