use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[RecurringPaymentHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_RecurringPaymentHistory_RecurringPayment_RecurringPaymentId] FOREIGN KEY([RecurringPaymentId])
REFERENCES [dbo].[RecurringPayment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecurringPaymentHistory] NOCHECK CONSTRAINT [FK_RecurringPaymentHistory_RecurringPayment_RecurringPaymentId]
GO
select*from dbo.RecurringPaymentHistory;