use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ProductReview_ReviewType_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_ReviewType_Mapping_ProductReview_ProductReviewId] FOREIGN KEY([ProductReviewId])
REFERENCES [dbo].[ProductReview] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview_ReviewType_Mapping] NOCHECK CONSTRAINT [FK_ProductReview_ReviewType_Mapping_ProductReview_ProductReviewId]
GO
ALTER TABLE [dbo].[ProductReview_ReviewType_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_ReviewType_Mapping_ReviewType_ReviewTypeId] FOREIGN KEY([ReviewTypeId])
REFERENCES [dbo].[ReviewType] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview_ReviewType_Mapping] NOCHECK CONSTRAINT [FK_ProductReview_ReviewType_Mapping_ReviewType_ReviewTypeId]
GO
select*from dbo.ProductReview_ReviewType_Mapping;