alter table [dbo].[Cars]
add CarOwnersId int null;

GO

ALTER TABLE [dbo].[CarOwners] DROP constraint FKCarOwners_Cars
GO

ALTER TABLE [dbo].[CarOwners] DROP COLUMN CarsId;  

GO

ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FKCars_CarOwners] FOREIGN KEY([CarOwnersId])
REFERENCES [dbo].[CarOwners] ([Id])
GO

ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FKCars_CarOwners]
GO
