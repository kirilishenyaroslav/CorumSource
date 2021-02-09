ALTER TABLE dbo.OrderTypesBase ADD UserRoleIdForCompetitiveList NVARCHAR(128) NULL;
GO


ALTER TABLE [dbo].[OrderTypesBase]  WITH CHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetRoles2] FOREIGN KEY([UserRoleIdForCompetitiveList])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO