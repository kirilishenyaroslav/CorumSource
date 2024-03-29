/****** Object:  StoredProcedure [dbo].[GetSpecifications]    Script Date: 12.08.2017 22:44:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[GetConcursHistory] 
@AcceptDate DATETIME2,
@SpecificationId int,
@RouteTypeId int,
@IntervalTypeId int
AS
SELECT ocl.[Id],
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
  where ocl.SpecificationId = @SpecificationId 
  and cs.RouteTypeId = @RouteTypeId
  and cs.IntervalTypeId = @IntervalTypeId
  and ob.OrderDate BETWEEN @AcceptDate AND dateadd(m,3,@AcceptDate) 
SET ANSI_NULLS ON
