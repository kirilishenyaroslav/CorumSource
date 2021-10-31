use [Corum.Prod-2021-07-27_remote server]
go
ALTER TABLE [dbo].[ActivityLog]  WITH CHECK ADD  CONSTRAINT [FK_ActivityLog_ActivityLogType_ActivityLogTypeId] FOREIGN KEY([ActivityLogTypeId])
REFERENCES [dbo].[ActivityLogType] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_ActivityLogType_ActivityLogTypeId]
GO
ALTER TABLE [dbo].[ActivityLog]  WITH CHECK ADD  CONSTRAINT [FK_ActivityLog_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_Customer_CustomerId]
GO
select*from dbo.ActivityLog;