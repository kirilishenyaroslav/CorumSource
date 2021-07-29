use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderPipelineSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderPipelineSteps_AspNetRoles] FOREIGN KEY([AccessRoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderPipelineSteps] NOCHECK CONSTRAINT [FK_OrderPipelineSteps_AspNetRoles]
GO
ALTER TABLE [dbo].[OrderPipelineSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderPipelineSteps_OrderStatuses] FOREIGN KEY([FromStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderPipelineSteps] NOCHECK CONSTRAINT [FK_OrderPipelineSteps_OrderStatuses]
GO
ALTER TABLE [dbo].[OrderPipelineSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderPipelineSteps_OrderStatuses1] FOREIGN KEY([ToStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderPipelineSteps] NOCHECK CONSTRAINT [FK_OrderPipelineSteps_OrderStatuses1]
GO
ALTER TABLE [dbo].[OrderPipelineSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderPipelineSteps_OrderTypesBase] FOREIGN KEY([OrderTypeId])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO
ALTER TABLE [dbo].[OrderPipelineSteps] NOCHECK CONSTRAINT [FK_OrderPipelineSteps_OrderTypesBase]
GO
select*from dbo.OrderPipelineSteps;