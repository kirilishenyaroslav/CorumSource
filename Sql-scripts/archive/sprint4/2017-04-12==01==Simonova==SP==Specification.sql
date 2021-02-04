ALTER TABLE dbo.OrderCompetitiveList
ADD SpecificationId int
go


ALTER TABLE [dbo].[OrderCompetitiveList]  WITH CHECK ADD  CONSTRAINT [FK_OrderCompetitiveList_ContractSpecifications] FOREIGN KEY([SpecificationId])
REFERENCES [dbo].[ContractSpecifications] ([Id])
GO

ALTER TABLE [dbo].[OrderCompetitiveList] CHECK CONSTRAINT [FK_OrderCompetitiveList_ContractSpecifications]
GO

ALTER TABLE dbo.OrderCompetitiveList
ADD IsChange bit
go
