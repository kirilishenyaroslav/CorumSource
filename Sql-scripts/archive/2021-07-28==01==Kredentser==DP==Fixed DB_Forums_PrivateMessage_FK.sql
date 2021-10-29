use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Forums_PrivateMessage]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_PrivateMessage_Customer_FromCustomerId] FOREIGN KEY([FromCustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_PrivateMessage] NOCHECK CONSTRAINT [FK_Forums_PrivateMessage_Customer_FromCustomerId]
GO
ALTER TABLE [dbo].[Forums_PrivateMessage]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_PrivateMessage_Customer_ToCustomerId] FOREIGN KEY([ToCustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_PrivateMessage] NOCHECK CONSTRAINT [FK_Forums_PrivateMessage_Customer_ToCustomerId]
GO

select*from dbo.Forums_PrivateMessage;