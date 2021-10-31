use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ELMAH_Error] ADD  CONSTRAINT [DF_ELMAH_Error_ErrorId]  DEFAULT (newid()) FOR [ErrorId]
GO
select*from dbo.ELMAH_Error;