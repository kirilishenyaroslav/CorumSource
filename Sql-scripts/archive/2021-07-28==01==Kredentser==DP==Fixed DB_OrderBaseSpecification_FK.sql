use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderBaseSpecification]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBaseSpecification_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO
ALTER TABLE [dbo].[OrderBaseSpecification] NOCHECK CONSTRAINT [FK_OrderBaseSpecification_OrdersBase]
GO
ALTER TABLE [dbo].[OrderBaseSpecification]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBaseSpecification_SpecificationTypes] FOREIGN KEY([SpecificationId])
REFERENCES [dbo].[SpecificationTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderBaseSpecification] NOCHECK CONSTRAINT [FK_OrderBaseSpecification_SpecificationTypes]
GO
select*from dbo.OrderBaseSpecification;