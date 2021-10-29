use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Routes]  WITH NOCHECK ADD  CONSTRAINT [FK_Routes_FromOrganization] FOREIGN KEY([OrgFromId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Routes] NOCHECK CONSTRAINT [FK_Routes_FromOrganization]
GO
ALTER TABLE [dbo].[Routes]  WITH NOCHECK ADD  CONSTRAINT [FK_Routes_ToOrganization] FOREIGN KEY([OrgToId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Routes] NOCHECK CONSTRAINT [FK_Routes_ToOrganization]
GO
select*from dbo.[Routes];