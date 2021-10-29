use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderFilterSettings2]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings2_AspNetUsersCur] FOREIGN KEY([IdCurrentUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings2] NOCHECK CONSTRAINT [FK_OrderFilterSettings2_AspNetUsersCur]
GO
select*from dbo.OrderFilterSettings2;