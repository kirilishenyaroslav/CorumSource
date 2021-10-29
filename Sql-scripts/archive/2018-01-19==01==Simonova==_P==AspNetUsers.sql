ALTER TABLE dbo.AspNetUsers
  ADD IsDeleted bit NOT NULL DEFAULT(0) 
  go 