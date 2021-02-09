ALTER TABLE dbo.Organization ADD IsSystemOrg  bit DEFAULT (0)
go

update dbo.Organization
set IsSystemOrg = 0 
go

ALTER TABLE dbo.Organization ALTER COLUMN IsSystemOrg bit NOT NULL
go