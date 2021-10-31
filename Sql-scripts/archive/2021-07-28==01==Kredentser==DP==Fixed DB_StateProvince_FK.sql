use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[StateProvince]  WITH NOCHECK ADD  CONSTRAINT [FK_StateProvince_Country_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StateProvince] NOCHECK CONSTRAINT [FK_StateProvince_Country_CountryId]
GO
select*from dbo.StateProvince;