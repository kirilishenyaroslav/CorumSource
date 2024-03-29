

/****** Object:  Table [dbo].[OrderBaseProjects]    Script Date: 29.05.2017 21:18:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderBaseProjects](
	[Id] [int] NOT NULL,
	[ProjectId] [int] NULL,
 CONSTRAINT [PK_OrderBaseProjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrderBaseProjects]  WITH CHECK ADD  CONSTRAINT [FK_OrderBaseProjects_OrderBaseProjects] FOREIGN KEY([Id])
REFERENCES [dbo].[OrderBaseProjects] ([Id])
GO

ALTER TABLE [dbo].[OrderBaseProjects] CHECK CONSTRAINT [FK_OrderBaseProjects_OrderBaseProjects]
GO

ALTER TABLE [dbo].[OrderBaseProjects]  WITH CHECK ADD  CONSTRAINT [FK_OrderBaseProjects_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO

ALTER TABLE [dbo].[OrderBaseProjects] CHECK CONSTRAINT [FK_OrderBaseProjects_Projects]
GO


alter table [dbo].[OrdersBase]
add IdProject int

go



ALTER TABLE [dbo].[OrdersBase]  WITH CHECK ADD  CONSTRAINT [FK_OrdersBase_Projects1] FOREIGN KEY([IdProject])
REFERENCES [dbo].[Projects] ([Id])
GO

ALTER TABLE [dbo].[OrdersBase] CHECK CONSTRAINT [FK_OrdersBase_Projects1]
GO
