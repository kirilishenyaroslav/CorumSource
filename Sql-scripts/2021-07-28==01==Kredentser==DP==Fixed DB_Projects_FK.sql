use [Corum.Prod-2021-07-27_remote server]
go
ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD FOREIGN KEY([ProjectCFOId])
REFERENCES [dbo].[Centers] ([Id])
GO
ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD FOREIGN KEY([ProjectTypeId])
REFERENCES [dbo].[ProjectTypes] ([Id])
GO
ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD  CONSTRAINT [FK_Projects_Organization] FOREIGN KEY([Shipper])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Projects] NOCHECK CONSTRAINT [FK_Projects_Organization]
GO
ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD  CONSTRAINT [FK_Projects_Organization1] FOREIGN KEY([Consignee])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Projects] NOCHECK CONSTRAINT [FK_Projects_Organization1]
GO
ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD  CONSTRAINT [FK_Projects_Projects] FOREIGN KEY([Id])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[Projects] NOCHECK CONSTRAINT [FK_Projects_Projects]
GO
select*from dbo.Projects;