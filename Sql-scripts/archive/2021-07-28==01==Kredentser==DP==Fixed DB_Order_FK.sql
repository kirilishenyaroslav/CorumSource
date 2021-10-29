use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Address_BillingAddressId] FOREIGN KEY([BillingAddressId])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_Address_BillingAddressId]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Address_PickupAddressId] FOREIGN KEY([PickupAddressId])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_Address_PickupAddressId]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Address_ShippingAddressId] FOREIGN KEY([ShippingAddressId])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_Address_ShippingAddressId]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_Customer_CustomerId]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_RewardPointsHistory_RewardPointsHistoryEntryId] FOREIGN KEY([RewardPointsHistoryEntryId])
REFERENCES [dbo].[RewardPointsHistory] ([Id])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_RewardPointsHistory_RewardPointsHistoryEntryId]
GO
select*from dbo.[Order];