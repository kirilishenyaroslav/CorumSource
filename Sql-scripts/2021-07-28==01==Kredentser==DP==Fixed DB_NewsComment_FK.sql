use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[NewsComment]  WITH NOCHECK ADD  CONSTRAINT [FK_NewsComment_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsComment] NOCHECK CONSTRAINT [FK_NewsComment_Customer_CustomerId]
GO
ALTER TABLE [dbo].[NewsComment]  WITH NOCHECK ADD  CONSTRAINT [FK_NewsComment_News_NewsItemId] FOREIGN KEY([NewsItemId])
REFERENCES [dbo].[News] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsComment] NOCHECK CONSTRAINT [FK_NewsComment_News_NewsItemId]
GO
ALTER TABLE [dbo].[NewsComment]  WITH NOCHECK ADD  CONSTRAINT [FK_NewsComment_Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsComment] NOCHECK CONSTRAINT [FK_NewsComment_Store_StoreId]
GO
select*from dbo.NewsComment;