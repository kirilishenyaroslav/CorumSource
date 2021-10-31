use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_Country_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_Country_CountryId]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_StateProvince_StateProvinceId] FOREIGN KEY([StateProvinceId])
REFERENCES [dbo].[StateProvince] ([Id])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_StateProvince_StateProvinceId]
GO
select*from dbo.Address;