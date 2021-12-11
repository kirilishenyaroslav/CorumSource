use [uh417455_db2]
go
alter table [dbo].RegisterMessageToContragents alter column emailOperacionist text;
go
alter table [dbo].RegisterMessageToContragents alter column emailContragent text;
go
select*from [dbo].RegisterMessageToContragents;