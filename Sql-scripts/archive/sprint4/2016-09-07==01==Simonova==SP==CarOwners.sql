alter table [dbo].[CarOwners]
add parentId int null;

GO


alter table [dbo].[CarOwners]
add CarsId int null;

GO



ALTER TABLE [dbo].[CarOwners]  WITH CHECK ADD  CONSTRAINT [FKCarOwners_Cars] FOREIGN KEY([CarsId])
REFERENCES [dbo].[Cars] ([Id])
GO

ALTER TABLE [dbo].[CarOwners] CHECK CONSTRAINT [FKCarOwners_Cars]
GO
