ALTER TABLE dbo.Projects
 ADD ProjectTypeId int NULL 

GO

ALTER TABLE dbo.Projects
 ADD ProjectCFOId int NULL      


GO 

ALTER TABLE dbo.Projects
 ADD ProjectOrderer VARCHAR(255) NULL

GO

ALTER TABLE dbo.Projects
 ADD ConstructionDesc VARCHAR(500) NULL


GO

ALTER TABLE dbo.Projects
 ADD Comments VARCHAR(Max) NULL

GO

ALTER TABLE dbo.Projects
 ADD PlanCount int NULL DEFAULT(0)

GO


ALTER TABLE dbo.Projects
 ADD isActive bit NULL DEFAULT(1)

GO


