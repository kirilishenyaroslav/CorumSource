use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderBaseProjects]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBaseProjects_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO
ALTER TABLE [dbo].[OrderBaseProjects] NOCHECK CONSTRAINT [FK_OrderBaseProjects_OrdersBase]
GO
ALTER TABLE [dbo].[OrderBaseProjects]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBaseProjects_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[OrderBaseProjects] NOCHECK CONSTRAINT [FK_OrderBaseProjects_Projects]
GO
select*from dbo.OrderBaseProjects;