use [Corum.Prod-2021-01-30]
go

INSERT INTO dbo.MenuStructure([menuName], [menuId], [parentId])  
VALUES (N'������ ��������', 37, NULL);

go 
select*from dbo.MenuStructure;