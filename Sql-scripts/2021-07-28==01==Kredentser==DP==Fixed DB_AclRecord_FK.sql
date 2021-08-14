use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[AclRecord]  WITH CHECK ADD  CONSTRAINT [FK_AclRecord_CustomerRole_CustomerRoleId] FOREIGN KEY([CustomerRoleId])
REFERENCES [dbo].[CustomerRole] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AclRecord] CHECK CONSTRAINT [FK_AclRecord_CustomerRole_CustomerRoleId]
GO

