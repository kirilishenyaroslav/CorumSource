use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ShippingMethodRestrictions]  WITH NOCHECK ADD  CONSTRAINT [FK_ShippingMethodRestrictions_Country_Country_Id] FOREIGN KEY([Country_Id])
REFERENCES [dbo].[Country] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShippingMethodRestrictions] NOCHECK CONSTRAINT [FK_ShippingMethodRestrictions_Country_Country_Id]
GO
ALTER TABLE [dbo].[ShippingMethodRestrictions]  WITH NOCHECK ADD  CONSTRAINT [FK_ShippingMethodRestrictions_ShippingMethod_ShippingMethod_Id] FOREIGN KEY([ShippingMethod_Id])
REFERENCES [dbo].[ShippingMethod] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShippingMethodRestrictions] NOCHECK CONSTRAINT [FK_ShippingMethodRestrictions_ShippingMethod_ShippingMethod_Id]
GO
select*from dbo.ShippingMethodRestrictions;