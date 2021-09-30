use [uh417455_db2_2021-08-18]
go
CREATE TABLE RegisterFormFromContragents (
 Id int not null identity,
 RegisterMessageToContragentId int not null,
 carBrand NVARCHAR(350) not null,
 stateNumberCar nvarchar(100) not null,
 trailerNumber nvarchar(100) not null,
 loadCapacity float not null,
 distance float not null,
 fullNameOfDriver NVARCHAR(350) not null,
 phoneNumber NVARCHAR(350) not null,
 drivingLicenseNumber NVARCHAR(350) not null,
 contragentName NVARCHAR(350) not null,
 note text null,
 stateBorderCrossingPoint text null,
 seriesPassportNumber NVARCHAR(350) not null,
 scannedCopyOfSignedOrder bit null,
 scannedCopyOfRegistrationCertificate bit null,
 scanCopyOfPassport bit null,
 scannedCopyOfAdmissionToTransportation bit null,
 scannedCopyOfCivilPassport bit null,
 IsUpdate bit null,
 dateCreate DATETIME2 not null,
 dateUpdate DATETIME2 not null,
 tenderItemUuid UNIQUEIDENTIFIER not null
 PRIMARY KEY (Id)
);

go
alter table RegisterFormFromContragents
with check add constraint FK_RegisterFormFromContragents_RegisterMessageToContragents foreign key(RegisterMessageToContragentId)
references RegisterMessageToContragents (Id)
on update cascade
on delete cascade

go
select*from RegisterFormFromContragents;