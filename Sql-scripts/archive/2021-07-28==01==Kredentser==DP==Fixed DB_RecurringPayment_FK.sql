use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[RecurringPayment]  WITH NOCHECK ADD  CONSTRAINT [FK_RecurringPayment_Order_InitialOrderId] FOREIGN KEY([InitialOrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[RecurringPayment] NOCHECK CONSTRAINT [FK_RecurringPayment_Order_InitialOrderId]
GO
select*from dbo.RecurringPayment;