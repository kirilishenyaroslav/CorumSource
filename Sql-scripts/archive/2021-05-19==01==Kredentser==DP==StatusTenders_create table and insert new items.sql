use [Corum.Prod-2021-01-30]
go
CREATE TABLE StatusTenders (
 statusID int not null identity,
 processValue NVARCHAR(100) not null,
 PRIMARY KEY (StatusID)
);
go
INSERT dbo.StatusTenders (processValue) VALUES (N'���������� ������������')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'������������� ���������')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'������������ ������������')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'����� �����������')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'��������� ������')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'������������ ���������')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'����� �������')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'����������� �������')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'��������')
go
select*from StatusTenders;