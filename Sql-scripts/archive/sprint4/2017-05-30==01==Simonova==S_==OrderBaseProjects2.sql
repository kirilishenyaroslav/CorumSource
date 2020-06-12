alter table [dbo].[OrderBaseProjects]
add OrderId bigint

go

alter table [dbo].[OrdersBase] drop constraint [FK_OrdersBase_Projects1]

go


alter table [dbo].[OrdersBase]
DROP COLUMN IdProject

go


ALTER TABLE [dbo].[OrderBaseProjects]  WITH CHECK ADD  CONSTRAINT [FK_OrderBaseProjects_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO

ALTER TABLE [dbo].[OrderBaseProjects] CHECK CONSTRAINT [FK_OrderBaseProjects_OrdersBase]
GO

ALTER TABLE [dbo].[OrderBaseProjects] ALTER COLUMN [ProjectId] int NOT NULL
go

ALTER TABLE [dbo].[OrderBaseProjects] ALTER COLUMN [OrderId] bigint NOT NULL
go

 DROP TABLE [dbo].[OrderBaseProjects]
 go

 SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderBaseProjects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_OrderBaseProjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrderBaseProjects]  WITH CHECK ADD  CONSTRAINT [FK_OrderBaseProjects_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO

ALTER TABLE [dbo].[OrderBaseProjects] CHECK CONSTRAINT [FK_OrderBaseProjects_OrdersBase]
GO

ALTER TABLE [dbo].[OrderBaseProjects]  WITH CHECK ADD  CONSTRAINT [FK_OrderBaseProjects_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO

ALTER TABLE [dbo].[OrderBaseProjects] CHECK CONSTRAINT [FK_OrderBaseProjects_Projects]
GO

