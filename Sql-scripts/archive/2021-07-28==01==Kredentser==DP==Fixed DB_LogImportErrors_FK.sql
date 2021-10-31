use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[LogImportErrors]  WITH NOCHECK ADD  CONSTRAINT [FK_LogImportErrors_LogisticSnapshots] FOREIGN KEY([idSnapshot])
REFERENCES [dbo].[LogisticSnapshots] ([id_spanshot])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogImportErrors] NOCHECK CONSTRAINT [FK_LogImportErrors_LogisticSnapshots]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 - ошибка, 2 - комментарий' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogImportErrors', @level2type=N'COLUMN',@level2name=N'isCommentType'
GO
select*from dbo.LogImportErrors;