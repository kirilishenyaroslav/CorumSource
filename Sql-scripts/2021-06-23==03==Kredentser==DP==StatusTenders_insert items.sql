use [Corum.Prod-2021-01-30]
go
SET IDENTITY_INSERT dbo.StatusTenders ON
INSERT INTO dbo.StatusTenders([processValue], [statusID])  VALUES (N'(не создан)', 10);
go
INSERT INTO dbo.StatusTenders([processValue], [statusID])  VALUES (N'черновик', 11);
go
INSERT INTO dbo.StatusTenders([processValue], [statusID])  VALUES (N'выгружен', 12);
go
INSERT INTO dbo.StatusTenders([processValue], [statusID])  VALUES (N'Аннулирован', 13);
go
SET IDENTITY_INSERT dbo.StatusTenders OFF
go
select*from StatusTenders;