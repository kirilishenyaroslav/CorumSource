 ALTER TABLE dbo.CarOwners
  ADD IsForwarder bit;

GO


alter table dbo.Cars
alter column FuelType int null
go

EXEC sp_RENAME '[dbo].[Cars].FuelType', 'FuelTypeId', 'COLUMN'
GO

ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FKCars_CarsFuelType] FOREIGN KEY([FuelTypeId])
REFERENCES [dbo].[CarsFuelType] ([Id])
GO

ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FKCars_CarsFuelType]
GO