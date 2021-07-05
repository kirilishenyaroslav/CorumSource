use [Corum.Prod-2021-01-30]
go

update BalanceKeepers set BalanceKeeper = N'ООО «Корум Шахтспецстрой»' where Id = 10;
select*from BalanceKeepers;