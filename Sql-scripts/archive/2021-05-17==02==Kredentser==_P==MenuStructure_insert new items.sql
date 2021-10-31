use [uh417455_db2]
go

INSERT INTO dbo.MenuStructure([menuName], [menuId], [parentId])  
VALUES (N'Реестр тендеров', 37, NULL);

go 
select*from dbo.MenuStructure;