
CREATE TABLE [dbo].OrderFilterSettings(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameFilter] [nvarchar](128) NOT NULL,
	[StatusId] [int],
	[CreatorId] [nvarchar](128),
	[ExecuterId] [nvarchar](128),
	[TypeId] [int],
	[ClientId] [bigint],
	[PriorityType] [int],
	[DeltaDateBeg] [int],
	[DeltaDateEnd] [int],
	[DeltaDateBegEx] [int],
	[DeltaDateEndEx] [int]
 CONSTRAINT [PK_OrderFilterSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].OrderFilterSettings  WITH CHECK ADD  CONSTRAINT [FK_OrderFilterSettings_OrderStatuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].OrderFilterSettings CHECK CONSTRAINT [FK_OrderFilterSettings_OrderStatuses]
GO

ALTER TABLE [dbo].OrderFilterSettings  WITH CHECK ADD  CONSTRAINT [FK_OrderFilterSettings_AspNetUsers] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].OrderFilterSettings CHECK CONSTRAINT [FK_OrderFilterSettings_AspNetUsers]
GO

ALTER TABLE [dbo].OrderFilterSettings  WITH CHECK ADD  CONSTRAINT [FK_OrderFilterSettings_AspNetUsersEx] FOREIGN KEY([ExecuterId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].OrderFilterSettings CHECK CONSTRAINT [FK_OrderFilterSettings_AspNetUsersEx]
GO

ALTER TABLE [dbo].OrderFilterSettings  WITH CHECK ADD  CONSTRAINT [FK_OrderFilterSettings_OrderTypesBase] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO
ALTER TABLE [dbo].OrderFilterSettings CHECK CONSTRAINT [FK_OrderFilterSettings_OrderTypesBase]
GO

ALTER TABLE [dbo].OrderFilterSettings  WITH CHECK ADD  CONSTRAINT [FK_OrderFilterSettings_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
GO
ALTER TABLE [dbo].OrderFilterSettings CHECK CONSTRAINT [FK_OrderFilterSettings_OrderClients]
GO



