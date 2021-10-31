use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderDogs]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderDogs_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDogs] NOCHECK CONSTRAINT [FK_OrderDogs_OrderClients]
GO
select*from dbo.OrderDogs;