use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Discount_AppliedToManufacturers]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToManufacturers_Discount_Discount_Id] FOREIGN KEY([Discount_Id])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToManufacturers] NOCHECK CONSTRAINT [FK_Discount_AppliedToManufacturers_Discount_Discount_Id]
GO
ALTER TABLE [dbo].[Discount_AppliedToManufacturers]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToManufacturers_Manufacturer_Manufacturer_Id] FOREIGN KEY([Manufacturer_Id])
REFERENCES [dbo].[Manufacturer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToManufacturers] NOCHECK CONSTRAINT [FK_Discount_AppliedToManufacturers_Manufacturer_Manufacturer_Id]
GO
select*from dbo.Discount_AppliedToManufacturers;