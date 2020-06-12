SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderFilterSettings2](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameFilter] [nvarchar](128) NULL,
	[PriorityType] [int] NULL,
	[DeltaDateBeg] [float] NOT NULL,
	[DeltaDateEnd] [float] NOT NULL,
	[DeltaDateBegEx] [float] NOT NULL,
	[DeltaDateEndEx] [float] NOT NULL,
	[UseStatusFilter] [bit] NOT NULL,
	[UseCreatorFilter] [bit] NOT NULL,
	[UseExecuterFilter] [bit] NOT NULL,
	[UseTypeFilter] [bit] NOT NULL,
	[UseClientFilter] [bit] NOT NULL,
	[UsePriorityFilter] [bit] NOT NULL,
	[UseDateFilter] [bit] NOT NULL,
	[UseExDateFilter] [bit] NOT NULL,
	[IdCurrentUser] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_OrderFilterSettings2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrderFilterSettings2]  WITH CHECK ADD  CONSTRAINT [FK_OrderFilterSettings2_AspNetUsersCur] FOREIGN KEY([IdCurrentUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[OrderFilterSettings2] CHECK CONSTRAINT [FK_OrderFilterSettings2_AspNetUsersCur]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderFilters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusId] [int] NULL,
	[CreatorId] [nvarchar](128) NULL,
	[ExecuterId] [nvarchar](128) NULL,
	[TypeId] [int] NULL,
	[ClientId] [bigint] NULL,
	[OrderFilterSetId] [int] NULL,
 CONSTRAINT [PK_OrderFilters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrderFilters]  WITH CHECK ADD  CONSTRAINT [FK_OrderFilters_AspNetUsers] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[OrderFilters] CHECK CONSTRAINT [FK_OrderFilters_AspNetUsers]
GO

ALTER TABLE [dbo].[OrderFilters]  WITH CHECK ADD  CONSTRAINT [FK_OrderFilters_AspNetUsersEx] FOREIGN KEY([ExecuterId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[OrderFilters] CHECK CONSTRAINT [FK_OrderFilters_AspNetUsersEx]
GO

ALTER TABLE [dbo].[OrderFilters]  WITH CHECK ADD  CONSTRAINT [FK_OrderFilters_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
GO

ALTER TABLE [dbo].[OrderFilters] CHECK CONSTRAINT [FK_OrderFilters_OrderClients]
GO

ALTER TABLE [dbo].[OrderFilters]  WITH CHECK ADD  CONSTRAINT [FK_OrderFilters_OrderFilterSettings2] FOREIGN KEY([OrderFilterSetId])
REFERENCES [dbo].[OrderFilterSettings2] ([Id])
GO

ALTER TABLE [dbo].[OrderFilters] CHECK CONSTRAINT [FK_OrderFilters_OrderFilterSettings2]
GO

ALTER TABLE [dbo].[OrderFilters]  WITH CHECK ADD  CONSTRAINT [FK_OrderFilters_OrderStatuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO

ALTER TABLE [dbo].[OrderFilters] CHECK CONSTRAINT [FK_OrderFilters_OrderStatuses]
GO

ALTER TABLE [dbo].[OrderFilters]  WITH CHECK ADD  CONSTRAINT [FK_OrderFilters_OrderTypesBase] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO

ALTER TABLE [dbo].[OrderFilters] CHECK CONSTRAINT [FK_OrderFilters_OrderTypesBase]
GO


