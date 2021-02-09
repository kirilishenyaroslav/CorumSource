alter table SpecificationTypes drop column MovingType
go

alter table SpecificationTypes 
alter column SpecificationType nvarchar(256)
go

SET IDENTITY_INSERT [dbo].[SpecificationTypes] ON
GO


INSERT INTO [dbo].[SpecificationTypes](Id, SpecificationType) VALUES(1,'фрахт/фиксированный')
GO

INSERT INTO [dbo].[SpecificationTypes](Id, SpecificationType) VALUES(2,'тариф/не фиксированный')
GO

SET IDENTITY_INSERT [dbo].[SpecificationTypes] OFF
  GO


