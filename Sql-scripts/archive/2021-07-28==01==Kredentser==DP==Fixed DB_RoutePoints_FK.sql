use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[RoutePoints]  WITH NOCHECK ADD  CONSTRAINT [FK_RoutePoints_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[RoutePoints] NOCHECK CONSTRAINT [FK_RoutePoints_Organization]
GO
ALTER TABLE [dbo].[RoutePoints]  WITH NOCHECK ADD  CONSTRAINT [FK_RoutePoints_RoutePointType] FOREIGN KEY([RoutePointTypeId])
REFERENCES [dbo].[RoutePointType] ([Id])
GO
ALTER TABLE [dbo].[RoutePoints] NOCHECK CONSTRAINT [FK_RoutePoints_RoutePointType]
GO
ALTER TABLE [dbo].[RoutePoints]  WITH NOCHECK ADD  CONSTRAINT [FK_RoutePoints_Routes] FOREIGN KEY([RoutePointId])
REFERENCES [dbo].[Routes] ([Id])
GO
ALTER TABLE [dbo].[RoutePoints] NOCHECK CONSTRAINT [FK_RoutePoints_Routes]
GO
select*from dbo.RoutePoints;