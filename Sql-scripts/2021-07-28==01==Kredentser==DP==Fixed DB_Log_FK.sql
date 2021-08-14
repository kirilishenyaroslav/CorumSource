use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Log]  WITH NOCHECK ADD  CONSTRAINT [FK_Log_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Log] NOCHECK CONSTRAINT [FK_Log_Customer_CustomerId]
GO
select*from dbo.Log;