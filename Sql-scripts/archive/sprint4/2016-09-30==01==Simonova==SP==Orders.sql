/****** Object:  UserDefinedFunction [dbo].[GetOrdersForPeriod]    Script Date: 30.09.2016 22:54:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[GetOrdersForPeriod]( 
    @PeriodDateBeg datetime,
	@PeriodDateEnd datetime
)
RETURNS @OrderBaseTable TABLE
   (
       NumerType     varchar(150),
	   PriorityType   varchar(10),
	   AddressFrom varchar(MAX),
	   AddressTo varchar(MAX),
	   PassInfo varchar(MAX),
	   Department varchar(1500),
	   Executor varchar(512)
   )
AS
BEGIN
INSERT @OrderBaseTable

select 
Convert(nvarchar(50),B.Id) +' '+ Coalesce(Bt.ShortName,'') as NumerType,
'PriorityType' =
CASE
WHEN B.PriotityType = 0 THEN 'Плановая'
ELSE 'Срочная'
END,
Coalesce(Countr.Name,'') + ', ' + Coalesce(P.FromCity,'') + ', ' + Coalesce(P.AdressFrom,'') + ' ' + Coalesce(P.OrgFrom,'') + ' ' + Convert(nvarchar(20),(convert(varchar, P.StartDateTimeOfTrip, 104))) + ' ' + Convert(nvarchar(5),(convert(varchar, P.StartDateTimeOfTrip, 108))) as AddressFrom,
'AddressTo' =
CASE
WHEN P.NeedReturn = 0 THEN Coalesce(Countr1.Name,'') + ', ' + Coalesce(P.ToCity,'') + ', ' + Coalesce(P.AdressTo,'') + ' ' + Coalesce(P.OrgTo,'') + ' ' + Convert(nvarchar(20),(convert(varchar, P.FinishDateTimeOfTrip, 104))) + ' ' + Convert(nvarchar(5),(convert(varchar, P.FinishDateTimeOfTrip, 108)))
ELSE  Coalesce(Countr1.Name,'') + ', ' + Coalesce(P.ToCity,'') + ', ' + Coalesce(P.AdressTo,'') + ' ' + Coalesce(P.OrgTo,'') + ' ' + Convert(nvarchar(20),(convert(varchar, P.ReturnFinishDateTimeOfTrip, 104)))  + ' ' + Convert(nvarchar(5),(convert(varchar, P.ReturnFinishDateTimeOfTrip, 108)))
END,
Coalesce(P.PassInfo,'') as PassInfo,
Coalesce(C.Center,'') +' '+ Coalesce(Oc.ClientName,'') + ' ' + Coalesce(Bal.BalanceKeeper,'') as Department,
Coalesce(U.DisplayName,'') + ' ' + Coalesce(U1.DisplayName,'') as Executor
from [dbo].[OrdersBase] B, [dbo].[OrderTypesBase] Bt,
[dbo].[Centers] C, [dbo].[OrderClients] Oc,
[dbo].[BalanceKeepers] Bal, [dbo].[AspNetUsers] U, [dbo].[AspNetUsers] U1,
[dbo].[OrdersPassengerTransport] P, [dbo].[Countries] Countr1,
[dbo].[Countries] Countr
where B.OrderType = Bt.Id
and B.ClientId = Oc.Id
and Oc.ClientCFOId = C.Id
and B.PayerId = Bal.Id
and B.CreatedByUser = U.Id
and B.OrderExecuter = U1.Id
and P.OrderId = B.Id
and P.ToCountry = Countr1.Сode
and P.FromCountry = Countr.Сode
and (B.OrderDate >= @PeriodDateBeg) And (B.OrderDate <= @PeriodDateEnd)
   
   RETURN
END

