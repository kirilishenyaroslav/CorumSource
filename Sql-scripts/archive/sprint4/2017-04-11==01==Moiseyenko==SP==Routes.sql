CREATE TABLE [dbo].[Routes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrgFromId] [bigint] NOT NULL,
	[OrgToId] [bigint] NOT NULL,
	[RouteTime] [int] NOT NULL,
	[RouteDistance] [decimal](16,2) NOT NULL,
 CONSTRAINT [PK_Routes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Routes]  WITH CHECK ADD  CONSTRAINT [FK_Routes_FromOrganization] FOREIGN KEY([OrgFromId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Routes] CHECK CONSTRAINT [FK_Routes_FromOrganization]
GO

ALTER TABLE [dbo].[Routes]  WITH CHECK ADD  CONSTRAINT [FK_Routes_ToOrganization] FOREIGN KEY([OrgToId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Routes] CHECK CONSTRAINT [FK_Routes_ToOrganization]
GO