use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderConcursListsSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderConcursListsSteps] FOREIGN KEY([StepId])
REFERENCES [dbo].[OrderConcursSteps] ([Id])
GO
ALTER TABLE [dbo].[OrderConcursListsSteps] NOCHECK CONSTRAINT [FK_OrderConcursListsSteps]
GO
ALTER TABLE [dbo].[OrderConcursListsSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderConcursListsSteps2] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderConcursListsSteps] NOCHECK CONSTRAINT [FK_OrderConcursListsSteps2]
GO
ALTER TABLE [dbo].[OrderConcursListsSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderConcursListsUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderConcursListsSteps] NOCHECK CONSTRAINT [FK_OrderConcursListsUsers]
GO

select*from dbo.OrderConcursListsSteps;