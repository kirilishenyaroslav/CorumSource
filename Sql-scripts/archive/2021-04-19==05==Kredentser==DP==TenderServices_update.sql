USE [uh417455_db2]
GO
/****** Object:  Table [dbo].[TenderServices]    Script Date: 19.04.2021 19:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TenderServices](
	[Id] [int] NOT NULL,
	[industryName] [nchar](150) NOT NULL,
	[industryId] [int] NOT NULL,
	[industryId_Test] [int] NOT NULL,
 CONSTRAINT [PK_TenderServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[TenderServices] ([Id], [industryName], [industryId], [industryId_Test]) VALUES (1, N'Услуги грузового транспорта (региональные)                                                                                                            ', 383, 42)
GO
INSERT [dbo].[TenderServices] ([Id], [industryName], [industryId], [industryId_Test]) VALUES (2, N'Услуги легкового и пассажирского транспорта                                                                                                           ', 384, 384)
GO
INSERT [dbo].[TenderServices] ([Id], [industryName], [industryId], [industryId_Test]) VALUES (3, N'Услуги спец.транспорта и спец.техники                                                                                                                 ', 385, 385)
GO
INSERT [dbo].[TenderServices] ([Id], [industryName], [industryId], [industryId_Test]) VALUES (4, N'Услуги грузового транспорта (международные)                                                                                                           ', 54, 383)
GO
