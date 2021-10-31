use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[TierPrice]  WITH NOCHECK ADD  CONSTRAINT [FK_TierPrice_CustomerRole_CustomerRoleId] FOREIGN KEY([CustomerRoleId])
REFERENCES [dbo].[CustomerRole] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TierPrice] NOCHECK CONSTRAINT [FK_TierPrice_CustomerRole_CustomerRoleId]
GO
ALTER TABLE [dbo].[TierPrice]  WITH NOCHECK ADD  CONSTRAINT [FK_TierPrice_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TierPrice] NOCHECK CONSTRAINT [FK_TierPrice_Product_ProductId]
GO
select*from dbo.TierPrice;