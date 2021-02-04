
SET IDENTITY_INSERT [dbo].[RouteTypes] ON
GO


INSERT INTO [dbo].[RouteTypes](Id, NameRouteType) VALUES(0,'Городская')
GO

update [dbo].[ContractSpecifications]
set [RouteTypeId] = 0
go

DELETE FROM [dbo].[RouteTypes] WHERE ID=1
  go

INSERT INTO [dbo].[RouteTypes](Id, NameRouteType) VALUES(1,'Междугородняя')
GO

DELETE FROM [dbo].[RouteTypes] WHERE ID in (2,3)
  go

INSERT INTO [dbo].[RouteTypes](Id, NameRouteType) VALUES(2,'Международная')
GO

SET IDENTITY_INSERT [dbo].[RouteTypes] OFF
  GO


