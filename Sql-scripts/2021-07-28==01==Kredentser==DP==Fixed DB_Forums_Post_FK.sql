use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Forums_Post]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Post_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_Post] NOCHECK CONSTRAINT [FK_Forums_Post_Customer_CustomerId]
GO
ALTER TABLE [dbo].[Forums_Post]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Post_Forums_Topic_TopicId] FOREIGN KEY([TopicId])
REFERENCES [dbo].[Forums_Topic] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forums_Post] NOCHECK CONSTRAINT [FK_Forums_Post_Forums_Topic_TopicId]
GO
select*from dbo.Forums_Post;