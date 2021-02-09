CREATE TABLE [dbo].[SpecificationNames](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SpecCode] [int] NOT NULL,
	[SpecName] [nvarchar](500) NOT NULL
 CONSTRAINT [PK_SpecificationNames] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[MenuStructure] ON 
GO

INSERT [dbo].[MenuStructure] ([Id], [menuName], [menuId], [parentId]) VALUES (34, N'Реестр названий услуг', N'34', 16)
GO

SET IDENTITY_INSERT [dbo].[MenuStructure] OFF
GO

alter table [dbo].[ContractSpecifications]
add NameId [int] NOT NULL 

go

alter table [dbo].[ContractSpecifications]
add TypeVehicleId [int] NOT NULL

go

alter table [dbo].[ContractSpecifications]
add TypeSpecId [int] NOT NULL 

go

alter table [dbo].[ContractSpecifications]
add RouteName [nvarchar](256)

go

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Vehicles] FOREIGN KEY([TypeVehicleId])
REFERENCES [dbo].[OrderVehicleTypes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Vehicles]
GO

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Types] FOREIGN KEY([TypeSpecId])
REFERENCES [dbo].[SpecificationTypes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Types]
GO

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Names] FOREIGN KEY([NameId])
REFERENCES [dbo].[SpecificationNames] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Names]
GO

update [dbo].[ContractSpecifications]
set  TypeSpecId = 2
where IsFreight = 0;

Go

ALTER TABLE [dbo].[ContractSpecifications]   
DROP Column [IsFreight];   
GO  

Alter table [dbo].[ContractSpecifications]
Alter Column [NameSpecification] [nvarchar](500) null;

GO


