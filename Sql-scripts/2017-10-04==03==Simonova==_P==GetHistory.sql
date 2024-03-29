USE [uh417455_db]
GO
/****** Object:  StoredProcedure [dbo].[GetConcursHistory]    Script Date: 03.10.2017 0:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[GetConcursHistory] 
@AcceptDate DATETIME2,
@AcceptDate2 DATETIME2,
@SpecificationId int,
@RouteTypeId int,
@IntervalTypeId int,
@ShowAll bit
AS
SELECT distinct ocl.[Id],
	ocl.[ExpeditorName]
      ,ocl.[CarsAccepted]    
      ,ocl.[CarCostDog]
      ,ocl.[CarCost]
      ,ocl.[DaysDelay]     
      ,ocl.[CarCost7]
      ,ocl.[DaysDelayStep1]
      ,ocl.[DaysDelayStep2],
	  ocl.[CarsOffered],
	  ocl.[Prepayment],
	  ocl.[Prepayment2],
	  ob.OrderDate,
	  sn.[SpecName],
	  cs.[NameId]
  FROM [dbo].[OrderCompetitiveList] ocl
 inner join dbo.ContractSpecifications cs
 on cs.Id = ocl.SpecificationId
 left join SpecificationNames sn
 on cs.NameId = sn.Id
 inner join OrdersBase ob 
 on ob.Id = ocl.OrderId
 inner join OrderConcursListsSteps s
 on s.OrderId = ocl.OrderId
 where s.StepId = 4 --финальный
 and ocl.CarsAccepted > 0
  and 
   ((@ShowAll = 1)
  or ((@ShowAll = 0) and
  (ocl.SpecificationId = @SpecificationId)
  and (cs.RouteTypeId = @RouteTypeId)
  and (cs.IntervalTypeId = @IntervalTypeId)))
  and ob.OrderDate BETWEEN @AcceptDate AND @AcceptDate2

SET ANSI_NULLS ON
