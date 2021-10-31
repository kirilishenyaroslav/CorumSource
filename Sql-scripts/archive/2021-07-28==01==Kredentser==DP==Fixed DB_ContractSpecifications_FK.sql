use [uh417455_db2]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_GroupesSpec_Specifications] FOREIGN KEY([GroupeSpecId])
REFERENCES [dbo].[ContractGroupesSpecifications] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_GroupesSpec_Specifications]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Routes_Specifications] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Routes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Routes_Specifications]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_AspNetUsers] FOREIGN KEY([CreatedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_AspNetUsers]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_CarCarryCapacity] FOREIGN KEY([CarryCapacityId])
REFERENCES [dbo].[CarCarryCapacity] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_CarCarryCapacity]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Names] FOREIGN KEY([NameId])
REFERENCES [dbo].[SpecificationNames] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Names]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_RouteIntervalType] FOREIGN KEY([IntervalTypeId])
REFERENCES [dbo].[RouteIntervalType] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_RouteIntervalType]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_RouteTypes] FOREIGN KEY([RouteTypeId])
REFERENCES [dbo].[RouteTypes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_RouteTypes]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Types] FOREIGN KEY([TypeSpecId])
REFERENCES [dbo].[SpecificationTypes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Types]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Vehicles] FOREIGN KEY([TypeVehicleId])
REFERENCES [dbo].[OrderVehicleTypes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Vehicles]
GO
select*from dbo.ContractSpecifications;