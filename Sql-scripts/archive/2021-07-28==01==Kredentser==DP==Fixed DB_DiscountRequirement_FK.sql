use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[DiscountRequirement]  WITH NOCHECK ADD  CONSTRAINT [FK_DiscountRequirement_Discount_DiscountId] FOREIGN KEY([DiscountId])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiscountRequirement] NOCHECK CONSTRAINT [FK_DiscountRequirement_Discount_DiscountId]
GO
ALTER TABLE [dbo].[DiscountRequirement]  WITH NOCHECK ADD  CONSTRAINT [FK_DiscountRequirement_DiscountRequirement_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[DiscountRequirement] ([Id])
GO
ALTER TABLE [dbo].[DiscountRequirement] NOCHECK CONSTRAINT [FK_DiscountRequirement_DiscountRequirement_ParentId]
GO
select*from dbo.DiscountRequirement;