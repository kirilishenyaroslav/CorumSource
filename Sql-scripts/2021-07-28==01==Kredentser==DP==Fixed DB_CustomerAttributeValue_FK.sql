use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[CustomerAttributeValue]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerAttributeValue_CustomerAttribute_CustomerAttributeId] FOREIGN KEY([CustomerAttributeId])
REFERENCES [dbo].[CustomerAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerAttributeValue] NOCHECK CONSTRAINT [FK_CustomerAttributeValue_CustomerAttribute_CustomerAttributeId]
GO
select*from dbo.CustomerAttributeValue;