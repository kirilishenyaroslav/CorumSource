alter table [dbo].[Contracts]
add NDSTax [decimal](16,2);

go

alter table [dbo].[ContractGroupesSpecifications]
add NDSTax [decimal](16,2);
go
