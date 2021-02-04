SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cars](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Model] [nvarchar](500) NULL,
	[Number] [nvarchar](20) NULL,
	[Driver] [nvarchar](500) NULL,
	[DriverLicense] [nvarchar](50) NULL,
	[FuelType] [nvarchar](20) NULL,
	[ConsumptionCity] [int] NULL,
	[ConsumptionHighway] [int] NULL,
	[PassNumber] [int] NULL,
 CONSTRAINT [PK_Cars] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Марка' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'Model'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Госномер' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'Number'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ФИО водителя' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'Driver'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'серия и номер прав' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'DriverLicense'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Вид топлива' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'FuelType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Расход на 100км в режиме город' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'ConsumptionCity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Расход на 100км в режиме за город' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'ConsumptionHighway'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество пассажиров' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'PassNumber'
GO



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CarOwners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CarrierName] [varchar](100) NULL,
	[OwnerAddress] [nvarchar](500) NULL,
	[Phone] [nvarchar](20) NULL,
	[ContactPerson] [varchar](100) NULL,
 CONSTRAINT [PK_Owners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


