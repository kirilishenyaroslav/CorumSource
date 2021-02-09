
/****** Object:  Table [dbo].[Contracts]    Script Date: 13.09.2016 20:39:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contracts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CarOwnersId] [int] NULL,
	[BalanceKeeperId] [int] NULL,
	[ContractNumber] [nvarchar](20) NULL,
	[ContractDate] [datetime] NULL,
	[DateBeg] [datetime] NULL,
	[DateEnd] [datetime] NULL,
 CONSTRAINT [PK_Contracts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_BalanceKeepers] FOREIGN KEY([BalanceKeeperId])
REFERENCES [dbo].[BalanceKeepers] ([Id])
GO

ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FK_Contracts_BalanceKeepers]
GO

ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FKContracts_CarOwners] FOREIGN KEY([CarOwnersId])
REFERENCES [dbo].[CarOwners] ([Id])
GO

ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FKContracts_CarOwners]
GO


