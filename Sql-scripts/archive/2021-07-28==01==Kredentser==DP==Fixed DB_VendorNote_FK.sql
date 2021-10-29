use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[VendorNote]  WITH NOCHECK ADD  CONSTRAINT [FK_VendorNote_Vendor_VendorId] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VendorNote] NOCHECK CONSTRAINT [FK_VendorNote_Vendor_VendorId]
GO
select*from dbo.VendorNote;