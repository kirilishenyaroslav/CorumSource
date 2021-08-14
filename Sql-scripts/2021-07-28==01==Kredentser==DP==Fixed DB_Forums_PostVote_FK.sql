use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Forums_PostVote]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_PostVote_Forums_Post_ForumPostId] FOREIGN KEY([ForumPostId])
REFERENCES [dbo].[Forums_Post] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forums_PostVote] NOCHECK CONSTRAINT [FK_Forums_PostVote_Forums_Post_ForumPostId]
GO
select*from dbo.Forums_PostVote;