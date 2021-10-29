use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Customer_CustomerRole_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Customer_CustomerRole_Mapping_Customer_Customer_Id] FOREIGN KEY([Customer_Id])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Customer_CustomerRole_Mapping] NOCHECK CONSTRAINT [FK_Customer_CustomerRole_Mapping_Customer_Customer_Id]
GO
ALTER TABLE [dbo].[Customer_CustomerRole_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Customer_CustomerRole_Mapping_CustomerRole_CustomerRole_Id] FOREIGN KEY([CustomerRole_Id])
REFERENCES [dbo].[CustomerRole] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Customer_CustomerRole_Mapping] NOCHECK CONSTRAINT [FK_Customer_CustomerRole_Mapping_CustomerRole_CustomerRole_Id]
GO
select*from dbo.Customer_CustomerRole_Mapping;