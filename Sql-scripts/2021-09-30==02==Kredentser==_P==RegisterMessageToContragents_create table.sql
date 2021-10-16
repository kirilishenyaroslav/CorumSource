use [uh417455_db2]
go
CREATE TABLE RegisterMessageToContragents (
 Id int not null identity,
 orderId bigint not null,
 formUuid UNIQUEIDENTIFIER not null,
 tenderNumber int not null,
 contragentName NVARCHAR(350) not null,
 emailOperacionist VARCHAR(50) not null,
 emailContragent VARCHAR(50) not null,
 dateCreate DATETIME2 not null,
 dateUpdate DATETIME2 not null,
 industryId int not null,
 descriptionTender text null,
 acceptedTransportUnits int null,
 cost float null,
 tenderItemUuid UNIQUEIDENTIFIER not null
 PRIMARY KEY (Id)
);
go
select*from RegisterMessageToContragents;