ALTER TABLE Organization
ADD IsAuto bit NOT NULL DEFAULT(0)
GO

update Organization
set IsAuto = 0
go