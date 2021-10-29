use [uh417455_db2]
go
CREATE TABLE RegisterTenderContragents (
 Id int not null identity,
 OrderId bigint not null,
 tenderNumber int not null,
 itemExternalNumber bigint not null,
 ContragentName NVARCHAR(350) not null,
 ContragentIdAps int not null,
 DateUpdateInfo DATETIME2 not null,
 IsWinner bit not null,
 EDRPOUContragent bigint not null,
 emailContragent VARCHAR(50) not null,
 transportUnitsProposed int not null,
 acceptedTransportUnits int null,
 costOfCarWithoutNDS float not null,
 costOfCarWithNDS float not null,
 PaymentDelay int not null,
 tenderItemUuid UNIQUEIDENTIFIER not null,
 nmcName NVARCHAR(500) not null,
 PRIMARY KEY (Id)
);
go
select*from RegisterTenderContragents;