CREATE TABLE [dbo].UserMessages(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateTimeCreate] [datetime2](7) NOT NULL,
	[MessageText] [varchar](max) NOT NULL,
	[CreatedFromUser] [nvarchar](128) NOT NULL,
	[CreatedToUser] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_UserMessages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserMessages]  WITH CHECK ADD  CONSTRAINT [FK_PK_UserMessages_AspNetUsers] FOREIGN KEY([CreatedFromUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserMessages] CHECK CONSTRAINT [FK_PK_UserMessages_AspNetUsers]
GO

ALTER TABLE [dbo].[UserMessages]  WITH CHECK ADD  CONSTRAINT [FK_PK_UserMessages_AspNetUsers2] FOREIGN KEY([CreatedToUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserMessages] CHECK CONSTRAINT [FK_PK_UserMessages_AspNetUsers2]
GO

alter table [dbo].[UserMessages]
add MessageSubject [nvarchar](256);

GO

alter table [dbo].[UserMessages]
add DateTimeOpen [datetime2](7);

GO