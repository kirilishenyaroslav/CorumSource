alter table [dbo].[OrdersBase]
add RouteId bigint;

ALTER TABLE [dbo].[OrdersBase]  WITH CHECK ADD  CONSTRAINT [FK_OrderBase_Routes] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Routes] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] CHECK CONSTRAINT [FK_OrderBase_Routes]
GO