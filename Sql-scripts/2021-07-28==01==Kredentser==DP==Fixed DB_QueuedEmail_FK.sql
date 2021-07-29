use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[QueuedEmail]  WITH NOCHECK ADD  CONSTRAINT [FK_QueuedEmail_EmailAccount_EmailAccountId] FOREIGN KEY([EmailAccountId])
REFERENCES [dbo].[EmailAccount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QueuedEmail] NOCHECK CONSTRAINT [FK_QueuedEmail_EmailAccount_EmailAccountId]
GO
select*from dbo.QueuedEmail;