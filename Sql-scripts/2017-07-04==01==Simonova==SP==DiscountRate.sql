

/****** Object:  Table [dbo].[ConcursDiscountRate]    Script Date: 05.07.2017 13:23:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ConcursDiscountRate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DiscountRate] [decimal](16, 2) NULL,
	[DateBeg] [datetime2](7) NULL,
	[DateEnd] [datetime2](7) NULL,
 CONSTRAINT [PK_ConcursDiscountRate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


