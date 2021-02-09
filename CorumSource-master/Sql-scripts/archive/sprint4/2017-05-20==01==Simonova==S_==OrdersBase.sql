SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SpecificationTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SpecificationType] [int] NULL,
	[MovingType] [int] NULL,
 CONSTRAINT [PK_SpecificationTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 - фрахт, 1 - фиксированный' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SpecificationTypes', @level2type=N'COLUMN',@level2name=N'SpecificationType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 - Фиксированный, 2 -Свободный' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SpecificationTypes', @level2type=N'COLUMN',@level2name=N'MovingType'
GO


ALTER TABLE dbo.OrdersBase
ADD SpecificationTypeId int
go


ALTER TABLE dbo.OrdersBase
ADD TimeRoute int
go

ALTER TABLE dbo.OrdersBase
ADD TimeSpecialVehicles int
go


ALTER TABLE [dbo].[OrdersBase]  WITH CHECK ADD  CONSTRAINT [FK_OrdersBase_SpecificationTypes] FOREIGN KEY([SpecificationTypeId])
REFERENCES [dbo].[SpecificationTypes] ([Id])
GO

ALTER TABLE [dbo].[OrdersBase] CHECK CONSTRAINT [FK_OrdersBase_SpecificationTypes]
GO
