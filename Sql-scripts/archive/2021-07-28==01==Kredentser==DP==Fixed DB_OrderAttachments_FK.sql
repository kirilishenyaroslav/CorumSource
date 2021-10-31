use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderAttachments] ADD  CONSTRAINT [DF_OrderAttachments_AddedDateTime]  DEFAULT (getdate()) FOR [AddedDateTime]
GO
ALTER TABLE [dbo].[OrderAttachments]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderAttachments_AspNetUsers] FOREIGN KEY([AddedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderAttachments] NOCHECK CONSTRAINT [FK_OrderAttachments_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderAttachments]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderAttachments_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderAttachments] NOCHECK CONSTRAINT [FK_OrderAttachments_OrdersBase]
GO
ALTER TABLE [dbo].[OrderAttachments]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderAttachments_OrdersDocTypes] FOREIGN KEY([DocType])
REFERENCES [dbo].[OrdersDocTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderAttachments] NOCHECK CONSTRAINT [FK_OrderAttachments_OrdersDocTypes]
GO
select*from dbo.OrderAttachments;