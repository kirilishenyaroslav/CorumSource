alter table [dbo].[OrdersBase]
add IsAdditionalRoutePoints bit;

go

CREATE TABLE [dbo].[AdditionalRoutePoints](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RoutePointId] [bigint] NOT NULL,
	[IsLoading] [bit],
	[OrderId] [bigint] not null
 CONSTRAINT [PK_AdditionalRoutePoints] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AdditionalRoutePoints]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalRoutePoints_Organization] FOREIGN KEY([RoutePointId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[AdditionalRoutePoints] CHECK CONSTRAINT [FK_AdditionalRoutePoints_Organization]
GO

ALTER TABLE [dbo].[AdditionalRoutePoints]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalRoutePoints_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO
ALTER TABLE [dbo].[AdditionalRoutePoints] CHECK CONSTRAINT [FK_AdditionalRoutePoints_Order]
GO