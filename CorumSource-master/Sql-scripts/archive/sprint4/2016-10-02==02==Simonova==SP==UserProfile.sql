
/****** Object:  Table [dbo].[UserProfile]    Script Date: 02.10.2016 23:48:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserProfile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[CountryId] [int] NULL,
	[City] [varchar](100) NULL,
	[Photo] [varchar](max) NULL,
	[LanguageId] [int] NULL,
 CONSTRAINT [PK_UsedProfile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_AspNetUsers]
GO

ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([?ode])
GO

ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_Countries]
GO


