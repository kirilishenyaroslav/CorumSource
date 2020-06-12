ALTER TABLE [dbo].[UserProfile]
ADD isFinishStatuses bit NOT NULL default 0
go

update [dbo].[UserProfile]
set isFinishStatuses = 0
go
