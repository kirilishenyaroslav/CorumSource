use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[AdditionalRoutePoints]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalRoutePoints_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO
ALTER TABLE [dbo].[AdditionalRoutePoints] CHECK CONSTRAINT [FK_AdditionalRoutePoints_Order]
GO
ALTER TABLE [dbo].[AdditionalRoutePoints]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalRoutePoints_Organization] FOREIGN KEY([RoutePointId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[AdditionalRoutePoints] CHECK CONSTRAINT [FK_AdditionalRoutePoints_Organization]
GO
select*from dbo.AdditionalRoutePoints;