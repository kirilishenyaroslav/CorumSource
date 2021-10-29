use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[FAQAnswers]  WITH NOCHECK ADD  CONSTRAINT [FK_PK_FAQAnswers_FAQGroupes] FOREIGN KEY([GroupId])
REFERENCES [dbo].[FAQGroupes] ([Id])
GO
ALTER TABLE [dbo].[FAQAnswers] NOCHECK CONSTRAINT [FK_PK_FAQAnswers_FAQGroupes]
GO
select*from dbo.FAQAnswers;