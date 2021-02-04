
/****** Object:  Table [dbo].[OrderCompetitiveList]    Script Date: 05.04.2017 12:49:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[OrderCompetitiveList](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[CarsAccepted] [int] NULL,
	[NDS] [int] NULL,
	[CarCostDog] [decimal](16, 2) NOT NULL,
	[CarCost] [decimal](16, 2) NOT NULL,
	[PaymentsDeferment] [int] NULL,
	[Prepayment] [int] NULL,
	[PrepaymentEffect] [int] NULL,
	[Comments] [varchar](255) NULL,
 CONSTRAINT [PK_OrderCompetitiveList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[OrderCompetitiveList]  WITH CHECK ADD  CONSTRAINT [FK_OrderCompetitiveList_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[OrderCompetitiveList] CHECK CONSTRAINT [FK_OrderCompetitiveList_OrdersBase]
GO


