use [Corum.Prod-2021-01-30]
go
SET IDENTITY_INSERT dbo.StatusTenders ON
INSERT INTO dbo.StatusTenders([processValue], [statusID])  VALUES (N'(�� ������)', 10);
go
INSERT INTO dbo.StatusTenders([processValue], [statusID])  VALUES (N'��������', 11);
go
INSERT INTO dbo.StatusTenders([processValue], [statusID])  VALUES (N'��������', 12);
go
INSERT INTO dbo.StatusTenders([processValue], [statusID])  VALUES (N'�����������', 13);
go
SET IDENTITY_INSERT dbo.StatusTenders OFF
go
select*from StatusTenders;