alter table [dbo].OrderUsedCars
add ExpeditorId int;

GO

ALTER TABLE [dbo].[OrderUsedCars]  WITH CHECK ADD  CONSTRAINT [FK_OrderUsedCars_CarOwners] FOREIGN KEY([ExpeditorId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] CHECK CONSTRAINT [FK_OrderUsedCars_CarOwners]
GO

alter table [dbo].Contracts
add ExpeditorId int;

GO

ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_CarOwners] FOREIGN KEY([ExpeditorId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FK_Contracts_CarOwners]
GO

alter table [dbo].OrderUsedCars
add ContractExpBkId int;

GO

ALTER TABLE [dbo].[OrderUsedCars]  WITH CHECK ADD  CONSTRAINT [FK_OrderUsedCarsContracts1] FOREIGN KEY([ContractExpBkId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] CHECK CONSTRAINT [FK_OrderUsedCarsContracts1]
GO