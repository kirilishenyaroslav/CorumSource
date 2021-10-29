use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ContractGroupesSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_GroupesSpec] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[ContractGroupesSpecifications] CHECK CONSTRAINT [FK_Contracts_GroupesSpec]
GO
ALTER TABLE [dbo].[ContractGroupesSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_GroupesSpec_AspNetUsers] FOREIGN KEY([CreatedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ContractGroupesSpecifications] CHECK CONSTRAINT [FK_GroupesSpec_AspNetUsers]
GO
select*from dbo.ContractGroupesSpecifications;