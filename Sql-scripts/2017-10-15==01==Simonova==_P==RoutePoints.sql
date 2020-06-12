

/****** Object:  Table [dbo].[RoutePoints]    Script Date: 15.10.2017 23:43:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RoutePoints](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoutePointId] [bigint] NULL,
	[RoutePointTypeId] [int] NULL,
	[ContactPerson] [varchar](255) NULL,
	[ContactPersonPhone] [varchar](50) NULL,
	[NumberPoint] [int] NULL,
 CONSTRAINT [PK_RoutePoints] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RoutePoints]  WITH CHECK ADD  CONSTRAINT [FK_RoutePoints_RoutePointType] FOREIGN KEY([RoutePointTypeId])
REFERENCES [dbo].[RoutePointType] ([Id])
GO

ALTER TABLE [dbo].[RoutePoints] CHECK CONSTRAINT [FK_RoutePoints_RoutePointType]
GO

ALTER TABLE [dbo].[RoutePoints]  WITH CHECK ADD  CONSTRAINT [FK_RoutePoints_Routes] FOREIGN KEY([RoutePointId])
REFERENCES [dbo].[Routes] ([Id])
GO

ALTER TABLE [dbo].[RoutePoints] CHECK CONSTRAINT [FK_RoutePoints_Routes]
GO


