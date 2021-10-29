use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[LocaleStringResource]  WITH NOCHECK ADD  CONSTRAINT [FK_LocaleStringResource_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LocaleStringResource] NOCHECK CONSTRAINT [FK_LocaleStringResource_Language_LanguageId]
GO
select*from dbo.LocaleStringResource;