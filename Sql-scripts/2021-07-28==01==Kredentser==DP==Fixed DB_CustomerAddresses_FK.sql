use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[CustomerAddresses]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerAddresses_Address_Address_Id] FOREIGN KEY([Address_Id])
REFERENCES [dbo].[Address] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerAddresses] NOCHECK CONSTRAINT [FK_CustomerAddresses_Address_Address_Id]
GO
ALTER TABLE [dbo].[CustomerAddresses]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerAddresses_Customer_Customer_Id] FOREIGN KEY([Customer_Id])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerAddresses] NOCHECK CONSTRAINT [FK_CustomerAddresses_Customer_Customer_Id]
GO
select*from dbo.CustomerAddresses;