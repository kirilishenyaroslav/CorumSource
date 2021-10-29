use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrdersBase] ADD  CONSTRAINT [DF_OrdersBase_CreateDatetime]  DEFAULT (getdate()) FOR [CreateDatetime]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBase_Routes] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Routes] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrderBase_Routes]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_AspNetUsers] FOREIGN KEY([CreatedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_AspNetUsers]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_AspNetUsers1] FOREIGN KEY([OrderExecuter])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_AspNetUsers1]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_BalanceKeepers] FOREIGN KEY([PayerId])
REFERENCES [dbo].[BalanceKeepers] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_BalanceKeepers]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_OrderClients]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_OrderDogs] FOREIGN KEY([ClientDogId])
REFERENCES [dbo].[OrderDogs] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_OrderDogs]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_OrderStatuses] FOREIGN KEY([CurrentOrderStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_OrderStatuses]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_OrderTypesBase] FOREIGN KEY([OrderType])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_OrderTypesBase]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_SpecificationTypes] FOREIGN KEY([TypeSpecId])
REFERENCES [dbo].[SpecificationTypes] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_SpecificationTypes]
GO
select*from dbo.OrdersBase;