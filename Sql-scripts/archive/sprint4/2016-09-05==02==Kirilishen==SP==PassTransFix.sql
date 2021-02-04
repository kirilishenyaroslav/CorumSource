

/****** Object:  Table [dbo].[Countries]    Script Date: 05.09.2016 15:45:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Countries](
	[Сode] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Fullname] [varchar](255) NOT NULL,
	[alpha2] [varchar](2) NOT NULL,
	[alpha3] [varchar](3) NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[Сode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


