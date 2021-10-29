use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Forums_Forum]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Forum_Forums_Group_ForumGroupId] FOREIGN KEY([ForumGroupId])
REFERENCES [dbo].[Forums_Group] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forums_Forum] NOCHECK CONSTRAINT [FK_Forums_Forum_Forums_Group_ForumGroupId]
GO
select*from dbo.Forums_Forum;