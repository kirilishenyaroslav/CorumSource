use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[StoreMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_StoreMapping_Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreMapping] NOCHECK CONSTRAINT [FK_StoreMapping_Store_StoreId]
GO
select*from dbo.StoreMapping;