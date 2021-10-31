use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[ProductReviewHelpfulness]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReviewHelpfulness_ProductReview_ProductReviewId] FOREIGN KEY([ProductReviewId])
REFERENCES [dbo].[ProductReview] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReviewHelpfulness] NOCHECK CONSTRAINT [FK_ProductReviewHelpfulness_ProductReview_ProductReviewId]
GO
select*from dbo.ProductReviewHelpfulness;