use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AccessFK] FOREIGN KEY([TypeAccessGroupId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AccessFK]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetAnnonymousUsers] FOREIGN KEY([UserForAnnonymousForm])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetAnnonymousUsers]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetRoles] FOREIGN KEY([UserRoleIdForClientData])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetRoles]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetRoles1] FOREIGN KEY([UserRoleIdForExecuterData])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetRoles1]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetRoles2] FOREIGN KEY([UserRoleIdForCompetitiveList])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetRoles2]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetUsers] FOREIGN KEY([DefaultExecuterId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetUsers]
GO
select*from dbo.OrderTypesBase;