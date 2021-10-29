use [Corum.Prod-2021-01-30]
go
CREATE TABLE RegisterTenders (
 Id int not null identity,
 OrderId bigint not null,
 tenderNumber int not null,
 subCompanyId int not null,
 subCompanyName NVARCHAR(150) not null,
 industryId int not null,
 industryName NVARCHAR(100) not null,
 dateStart DATETIME2 not null,
 dateEnd DATETIME2 not null,
 mode TINYINT not null,
 stageMode NVARCHAR(100) not null,
 stageNumber TINYINT not null,
 TenderUuid uniqueidentifier not null,
 process TINYINT not null,
 downloadAddress NVARCHAR(400) not null,
 unloadAddress NVARCHAR(400) not null,
 downloadDataRequired DATETIME2 not null,
 unloadDataRequired DATETIME2 not null,
 routeOrder NVARCHAR(500) not null,
 cargoName NVARCHAR(500) not null,
 lotState int not null,
 PRIMARY KEY (Id)
);
go
select*from RegisterTenders;