alter table [dbo].[Contracts]
add ContractRevision [varchar](max);

go

alter table [dbo].[ContractGroupesSpecifications]
add FuelPrice [decimal](16,2);
go

alter table [dbo].[ContractGroupesSpecifications]
add ExchangeRateUahRub [decimal](16,4);

go