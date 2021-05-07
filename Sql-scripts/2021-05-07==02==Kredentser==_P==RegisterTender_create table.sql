use [uh417455_db2]
go
CREATE TABLE RegisterTenders (
 Id int not null identity,
 OrderId bigint not null,
 TenderUuid uniqueidentifier not null,
 PRIMARY KEY (Id)
);
go
select*from RegisterTenders;