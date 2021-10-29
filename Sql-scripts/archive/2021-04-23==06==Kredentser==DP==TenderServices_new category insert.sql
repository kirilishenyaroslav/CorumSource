use [Corum.Prod-2021-01-30]
go

INSERT INTO dbo.TenderServices ([Id], [industryName], [industryId], [industryId_Test])  
VALUES (5, N'Перевозки ЖД', 67, 57);

go 
select*from dbo.TenderServices;