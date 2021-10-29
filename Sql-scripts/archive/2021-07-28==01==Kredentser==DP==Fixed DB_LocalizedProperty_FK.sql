use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[LocalizedProperty]  WITH NOCHECK ADD  CONSTRAINT [FK_LocalizedProperty_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LocalizedProperty] NOCHECK CONSTRAINT [FK_LocalizedProperty_Language_LanguageId]
GO
select*from dbo.LocalizedProperty;