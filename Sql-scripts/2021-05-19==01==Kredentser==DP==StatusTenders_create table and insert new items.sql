use [Corum.Prod-2021-01-30]
go
CREATE TABLE StatusTenders (
 statusID int not null identity,
 processValue NVARCHAR(100) not null,
 PRIMARY KEY (StatusID)
);
go
INSERT dbo.StatusTenders (processValue) VALUES (N'Подготовка документации')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'Корректировка критериев')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'Согласование документации')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'Прием предложений')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'Обработка данных')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'Согласование обработки')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'Выбор решения')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'Утверждение решения')
go
INSERT dbo.StatusTenders (processValue) VALUES (N'Завершен')
go
select*from StatusTenders;