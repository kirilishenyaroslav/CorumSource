use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Forums_Subscription]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Subscription_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_Subscription] NOCHECK CONSTRAINT [FK_Forums_Subscription_Customer_CustomerId]
GO
select*from dbo.Forums_Subscription;