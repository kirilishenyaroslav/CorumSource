USE [uh417455_db]
GO
/****** Object:  StoredProcedure [dbo].[GetConcursHistory]    Script Date: 06.09.2017 22:17:31 ******/
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
	  ob.OrderDate
  FROM [dbo].[OrderCompetitiveList] ocl
 inner join dbo.ContractSpecifications cs
 on cs.Id = ocl.SpecificationId
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
