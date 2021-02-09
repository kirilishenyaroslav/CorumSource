alter table [dbo].Contracts
add IsActive bit;

go

alter table [dbo].Contracts
add DaysDelay int;

go

alter table [dbo].Contracts
add ReceiveDateReal datetime;

go


CREATE TABLE [dbo].RouteTypes(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameRouteType] [nvarchar](256) NOT NULL
 CONSTRAINT [PK_RouteTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].CarCarryCapacity(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CarryCapacity] int NOT NULL
 CONSTRAINT [PK_CarCarryCapacity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].RouteIntervalType(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameIntervalType] [nvarchar](256) NOT NULL
 CONSTRAINT [PK_RouteIntervalType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].ContractSpecifications(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL, 
	[DateBeg] [datetime] NOT NULL, 
	[DateEnd] [datetime] NOT NULL, 
	[CreatedByUser] [nvarchar](128) NOT NULL, 
	[NameSpecification] [nvarchar](500) NOT NULL,
	[CarryCapacityId] [int],
	[IsFreight] bit not null,
	[DeparturePoint] [nvarchar](500),
	[ArrivalPoint] [nvarchar](500),
	[RouteLength] [decimal](16,2),
	[MovingType] [int],
	[RouteTypeId] [int] not null,
	[IntervalTypeId] [int] not null,
	[RateKm]  [decimal](16,2),
	[RateHour]  [decimal](16,2),
	[RateMachineHour]  [decimal](16,2),
	[RateTotalFreight]  [decimal](16,2),
	[NDSTax]  [decimal](16,2),
 CONSTRAINT [PK_ContractSpecifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_RouteIntervalType] FOREIGN KEY([IntervalTypeId])
REFERENCES [dbo].[RouteIntervalType] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_RouteIntervalType]
GO

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_CarCarryCapacity] FOREIGN KEY([CarryCapacityId])
REFERENCES [dbo].[CarCarryCapacity] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_CarCarryCapacity]
GO

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_RouteTypes] FOREIGN KEY([RouteTypeId])
REFERENCES [dbo].[RouteTypes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_RouteTypes]
GO

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_AspNetUsers] FOREIGN KEY([CreatedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_AspNetUsers]
GO

SET IDENTITY_INSERT [dbo].[RouteTypes] ON 
GO

INSERT [dbo].[RouteTypes] ([Id], [NameRouteType]) VALUES (1, N'Городской')
GO

INSERT [dbo].[RouteTypes] ([Id], [NameRouteType]) VALUES (2, N'Междугородний')
GO

INSERT [dbo].[RouteTypes] ([Id], [NameRouteType]) VALUES (3, N'Международный')
GO

SET IDENTITY_INSERT [dbo].[CarCarryCapacity] OFF
GO

Alter table [dbo].[CarCarryCapacity]
Alter Column [CarryCapacity] decimal(16,2) not null;

GO

alter table [dbo].ContractSpecifications
add ContractId int not null;

go

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Contracts] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Contracts]
GO
