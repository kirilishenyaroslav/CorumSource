
/****** Object:  Table [dbo].[OrderBaseSpecification]    Script Date: 17.06.2017 22:08:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderBaseSpecification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NULL,
	[SpecificationId] [int] NULL,
 CONSTRAINT [PK_OrderBaseSpecification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrderBaseSpecification]  WITH CHECK ADD  CONSTRAINT [FK_OrderBaseSpecification_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO

ALTER TABLE [dbo].[OrderBaseSpecification] CHECK CONSTRAINT [FK_OrderBaseSpecification_OrdersBase]
GO

ALTER TABLE [dbo].[OrderBaseSpecification]  WITH CHECK ADD  CONSTRAINT [FK_OrderBaseSpecification_SpecificationTypes] FOREIGN KEY([SpecificationId])
REFERENCES [dbo].[SpecificationTypes] ([Id])
GO

ALTER TABLE [dbo].[OrderBaseSpecification] CHECK CONSTRAINT [FK_OrderBaseSpecification_SpecificationTypes]
GO


