

/****** Object:  Table [dbo].[RoutePointType]    Script Date: 27.09.2017 1:14:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoutePointType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullNamePointType] [nvarchar](500) NULL,
	[ShortNamePointType] [nvarchar](50) NULL,
 CONSTRAINT [PK_RoutePointType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


