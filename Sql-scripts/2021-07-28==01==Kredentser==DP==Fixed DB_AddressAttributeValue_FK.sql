use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[AddressAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_AddressAttributeValue_AddressAttribute_AddressAttributeId] FOREIGN KEY([AddressAttributeId])
REFERENCES [dbo].[AddressAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AddressAttributeValue] CHECK CONSTRAINT [FK_AddressAttributeValue_AddressAttribute_AddressAttributeId]
GO
select*from dbo.AddressAttributeValue;