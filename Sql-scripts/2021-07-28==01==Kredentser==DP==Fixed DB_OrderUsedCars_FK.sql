use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCars] FOREIGN KEY([CarId])
REFERENCES [dbo].[Cars] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCars]
GO
ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCars_CarOwners] FOREIGN KEY([ExpeditorId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCars_CarOwners]
GO
ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCarsContract] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCarsContract]
GO
ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCarsContracts1] FOREIGN KEY([ContractExpBkId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCarsContracts1]
GO
ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCarsOrder] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCarsOrder]
GO
select*from dbo.OrderUsedCars;