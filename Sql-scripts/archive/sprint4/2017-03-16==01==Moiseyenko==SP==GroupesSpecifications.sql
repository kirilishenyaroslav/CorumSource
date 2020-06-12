ALTER TABLE [dbo].[ContractSpecifications]   
DROP CONSTRAINT [FK_Specifications_Contracts];   
GO  

ALTER TABLE [dbo].[ContractSpecifications]   
DROP Column [ContractId];   
GO  

CREATE TABLE [dbo].ContractGroupesSpecifications(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateBeg] [datetime] NOT NULL, 
	[DateEnd] [datetime] NOT NULL, 
	[NameGroupSpec] [nvarchar](500) NOT NULL,
	[DaysDelay] [int],
	[IsActive] bit not null,
	[ContractId] int not null,
 CONSTRAINT [PK_ContractGroupesSpecifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ContractGroupesSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_GroupesSpec] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[ContractGroupesSpecifications] CHECK CONSTRAINT [FK_Contracts_GroupesSpec]
GO

TRUNCATE TABLE [dbo].[ContractSpecifications];
GO

alter table [dbo].[ContractSpecifications]
add GroupeSpecId int not null;

go

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_GroupesSpec_Specifications] FOREIGN KEY([GroupeSpecId])
REFERENCES [dbo].[ContractGroupesSpecifications] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_GroupesSpec_Specifications]
GO

alter table [dbo].[ContractGroupesSpecifications]
add CreateDate datetime2(7) not null;

go

alter table [dbo].[ContractGroupesSpecifications]
add CreatedByUser nvarchar(128) not null;

go

ALTER TABLE [dbo].[ContractGroupesSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_GroupesSpec_AspNetUsers] FOREIGN KEY([CreatedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ContractGroupesSpecifications] CHECK CONSTRAINT [FK_GroupesSpec_AspNetUsers]
GO

Alter table [dbo].[ContractSpecifications]
Alter Column [DateBeg] datetime;

GO

Alter table [dbo].[ContractSpecifications]
Alter Column [DateEnd] datetime;

GO
