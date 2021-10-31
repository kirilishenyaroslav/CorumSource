use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_AspNetUsers] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_AspNetUsersEx] FOREIGN KEY([ExecuterId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_AspNetUsersEx]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_OrderClients]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_OrderFilterSettings2] FOREIGN KEY([OrderFilterSetId])
REFERENCES [dbo].[OrderFilterSettings2] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_OrderFilterSettings2]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_OrderStatuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_OrderStatuses]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_OrderTypesBase] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_OrderTypesBase]
GO
select*from dbo.OrderFilters;