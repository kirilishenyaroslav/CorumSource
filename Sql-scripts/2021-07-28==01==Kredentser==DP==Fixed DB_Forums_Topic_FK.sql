use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Forums_Topic]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Topic_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_Topic] NOCHECK CONSTRAINT [FK_Forums_Topic_Customer_CustomerId]
GO
ALTER TABLE [dbo].[Forums_Topic]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Topic_Forums_Forum_ForumId] FOREIGN KEY([ForumId])
REFERENCES [dbo].[Forums_Forum] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forums_Topic] NOCHECK CONSTRAINT [FK_Forums_Topic_Forums_Forum_ForumId]
GO
select*from dbo.Forums_Topic;