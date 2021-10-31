use [uh417455_db2]
go

update BalanceKeepers set BalanceKeeper = N'ООО «Корум Шахтспецстрой»' where Id = 10;
select*from BalanceKeepers;