use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[PollAnswer]  WITH NOCHECK ADD  CONSTRAINT [FK_PollAnswer_Poll_PollId] FOREIGN KEY([PollId])
REFERENCES [dbo].[Poll] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PollAnswer] NOCHECK CONSTRAINT [FK_PollAnswer_Poll_PollId]
GO
select*from dbo.PollAnswer;