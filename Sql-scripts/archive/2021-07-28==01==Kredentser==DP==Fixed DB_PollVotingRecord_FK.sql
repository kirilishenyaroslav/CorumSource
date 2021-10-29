use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[PollVotingRecord]  WITH NOCHECK ADD  CONSTRAINT [FK_PollVotingRecord_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PollVotingRecord] NOCHECK CONSTRAINT [FK_PollVotingRecord_Customer_CustomerId]
GO
ALTER TABLE [dbo].[PollVotingRecord]  WITH NOCHECK ADD  CONSTRAINT [FK_PollVotingRecord_PollAnswer_PollAnswerId] FOREIGN KEY([PollAnswerId])
REFERENCES [dbo].[PollAnswer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PollVotingRecord] NOCHECK CONSTRAINT [FK_PollVotingRecord_PollAnswer_PollAnswerId]
GO
select*from dbo.PollVotingRecord;