use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Discount_AppliedToCategories]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToCategories_Category_Category_Id] FOREIGN KEY([Category_Id])
REFERENCES [dbo].[Category] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToCategories] NOCHECK CONSTRAINT [FK_Discount_AppliedToCategories_Category_Category_Id]
GO
ALTER TABLE [dbo].[Discount_AppliedToCategories]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToCategories_Discount_Discount_Id] FOREIGN KEY([Discount_Id])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToCategories] NOCHECK CONSTRAINT [FK_Discount_AppliedToCategories_Discount_Discount_Id]
GO
select*from dbo.Discount_AppliedToCategories;