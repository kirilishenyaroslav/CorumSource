use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[CheckoutAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_CheckoutAttributeValue_CheckoutAttribute_CheckoutAttributeId] FOREIGN KEY([CheckoutAttributeId])
REFERENCES [dbo].[CheckoutAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CheckoutAttributeValue] CHECK CONSTRAINT [FK_CheckoutAttributeValue_CheckoutAttribute_CheckoutAttributeId]
GO
select*from dbo.CheckoutAttributeValue;