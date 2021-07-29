use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[SpecificationAttributeOption]  WITH NOCHECK ADD  CONSTRAINT [FK_SpecificationAttributeOption_SpecificationAttribute_SpecificationAttributeId] FOREIGN KEY([SpecificationAttributeId])
REFERENCES [dbo].[SpecificationAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SpecificationAttributeOption] NOCHECK CONSTRAINT [FK_SpecificationAttributeOption_SpecificationAttribute_SpecificationAttributeId]
GO
select*from dbo.SpecificationAttributeOption;