use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_BalanceKeepers] FOREIGN KEY([BalanceKeeperId])
REFERENCES [dbo].[BalanceKeepers] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FK_Contracts_BalanceKeepers]
GO
ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_CarOwners] FOREIGN KEY([ExpeditorId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FK_Contracts_CarOwners]
GO
ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FKContracts_CarOwners] FOREIGN KEY([CarOwnersId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FKContracts_CarOwners]
GO
select*from dbo.Contracts;