use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderCompetitiveList]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderCompetitiveList_ContractSpecifications] FOREIGN KEY([SpecificationId])
REFERENCES [dbo].[ContractSpecifications] ([Id])
GO
ALTER TABLE [dbo].[OrderCompetitiveList] NOCHECK CONSTRAINT [FK_OrderCompetitiveList_ContractSpecifications]
GO
ALTER TABLE [dbo].[OrderCompetitiveList]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderCompetitiveList_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderCompetitiveList] NOCHECK CONSTRAINT [FK_OrderCompetitiveList_OrdersBase]
GO
select*from dbo.OrderCompetitiveList;