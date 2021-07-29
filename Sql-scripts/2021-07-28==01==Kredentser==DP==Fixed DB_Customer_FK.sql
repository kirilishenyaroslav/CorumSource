use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Address_BillingAddress_Id] FOREIGN KEY([BillingAddress_Id])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Address_BillingAddress_Id]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Address_ShippingAddress_Id] FOREIGN KEY([ShippingAddress_Id])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Address_ShippingAddress_Id]
GO
select*from dbo.Customer;