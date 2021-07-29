use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[CustomerPassword]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerPassword_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerPassword] NOCHECK CONSTRAINT [FK_CustomerPassword_Customer_CustomerId]
GO
select*from dbo.CustomerPassword;