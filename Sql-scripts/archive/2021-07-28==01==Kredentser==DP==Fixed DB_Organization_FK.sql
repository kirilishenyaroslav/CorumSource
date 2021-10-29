use [Corum.Prod-2021-07-27_remote server]
go
ALTER TABLE [dbo].[Organization]  WITH NOCHECK ADD  CONSTRAINT [FK_Organization_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Ñode])
GO
ALTER TABLE [dbo].[Organization] NOCHECK CONSTRAINT [FK_Organization_Countries]
GO
select*from dbo.Organization;