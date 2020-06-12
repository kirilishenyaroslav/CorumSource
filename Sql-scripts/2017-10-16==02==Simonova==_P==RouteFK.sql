
ALTER TABLE [dbo].[RoutePoints]  WITH CHECK ADD  CONSTRAINT [FK_RoutePoints_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization] ([Id])
GO

ALTER TABLE [dbo].[RoutePoints] CHECK CONSTRAINT [FK_RoutePoints_Organization]
GO
