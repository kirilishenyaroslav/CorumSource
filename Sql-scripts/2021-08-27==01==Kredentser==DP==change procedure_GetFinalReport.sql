USE [uh417455_db2_2021-08-18]
GO
/****** Object:  StoredProcedure [dbo].[GetFinalReport]    Script Date: 27.08.2021 14:02:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetFinalReport] @userId NVARCHAR(128),
@isAdmin BIT,
@UseOrderClientFilter BIT,
@UseOrderTypeFilter BIT,
@UseTripTypeFilter BIT,
@FilterOrderClientId NVARCHAR(128),
@FilterOrderTypeId NVARCHAR(128),
@FilterTripTypeId NVARCHAR(128),
@FilterOrderDateBeg DATETIME2,
@FilterOrderDateEnd DATETIME2,
@FilterAcceptDateBeg DATETIME2,
@FilterAcceptDateEnd DATETIME2,
@UseOrderDateFilter BIT,
@UseAcceptDateFilter BIT, 
@isPassOrders BIT
AS

  IF (@isAdmin = 1)
  BEGIN
    SELECT
      os.OrderStatusName, ops.FinishOfTheProcess,
      COUNT(distinct ob.Id) AS CntAll
	   	  
    FROM dbo.OrdersBase ob
    LEFT JOIN dbo.OrderClients oc
      ON ob.ClientId = oc.Id
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
    LEFT JOIN dbo.AspNetRoles ro
      ON oc.AccessRoleId = ro.Id
    LEFT JOIN dbo.AspNetRoles rt
      ON otb.TypeAccessGroupId = rt.Id
    LEFT JOIN dbo.Centers cc
      ON oc.ClientCFOId = cc.Id
    LEFT JOIN dbo.AspNetUsers anuc
      ON ob.CreatedByUser = anuc.Id
    LEFT JOIN dbo.AspNetUsers anue
      ON ob.OrderExecuter = anue.Id
    LEFT JOIN dbo.BalanceKeepers bk
      ON ob.PayerId = bk.Id
    	
    inner join OrderPipelineSteps ops
	on ops.OrderTypeId = ob.OrderType and
    ops.ToStatus = ob.CurrentOrderStatus
	
	inner join dbo.OrderStatuses os
	on ob.CurrentOrderStatus = os.Id	
	
	LEFT JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
    LEFT JOIN dbo.OrdersPassengerTransport ps
      ON ob.Id = ps.OrderId

    WHERE (otb.IsTransportType = 1)
    AND (((@isPassOrders = 0) AND (ob.OrderType in (4,5,7)))
	OR ((@isPassOrders = 1) AND (ob.OrderType in (1,3,6))))    
    AND ((@UseOrderClientFilter = 0)
    OR ((@UseOrderClientFilter = 1)    
    AND (ob.ClientId in (SELECT *  
                         FROM dbo.SplitString(@FilterOrderClientId)))))
    AND ((@UseOrderTypeFilter = 0)
    OR ((@UseOrderTypeFilter = 1)   
    AND (ob.OrderType in (SELECT *  
                                   FROM dbo.SplitString(@FilterOrderTypeId)))))
 
 	AND ((@UseTripTypeFilter = 0)
    OR ((@UseTripTypeFilter = 1)   
    AND (
	((ob.OrderType in (4,5,7)) AND
	(ott.TripType in (SELECT *  FROM dbo.SplitString(@FilterTripTypeId))))
	OR
	((ob.OrderType in (1,3,6)) AND
	(ps.TripType in (SELECT *  FROM dbo.SplitString(@FilterTripTypeId))))
	)))

 
 AND (
	--отбираем по дате заявки
	((@UseOrderDateFilter = 1) AND 
	((ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd)
	OR
	--если проимпортирована, то отбираем по данным черновика
	((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd))))
	OR 
  --отбираем по дате фактической поставки авто
	((@UseAcceptDateFilter = 1)
	 AND (((
	((ob.OrderType in (4,5,7)) AND ((CAST(ott.FromShipperDatetime AS DATE)) >= @FilterAcceptDateBeg)
	AND ((CAST(ott.FromShipperDatetime AS DATE)) <=@FilterAcceptDateEnd))
	OR
	((ob.OrderType in (1,3,6)) AND ((CAST(ps.StartDateTimeOfTrip AS DATE)) >= @FilterAcceptDateBeg)
	AND ((CAST(ps.StartDateTimeOfTrip AS DATE)) <=@FilterAcceptDateEnd))
	)
	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1
				OR ops.FinishOfTheProcess=0)))
	--если проимпортирована, то отбираем по данным черновика
	OR ((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterAcceptDateBeg AND @FilterAcceptDateEnd)))))
    group by os.OrderStatusName, ops.FinishOfTheProcess
    END
  ELSE
  BEGIN

    SELECT
      os.OrderStatusName, ops.FinishOfTheProcess,
      COUNT(distinct ob.Id) AS CntAll
	
	FROM dbo.OrdersBase ob
    LEFT JOIN dbo.OrderClients oc
      ON ob.ClientId = oc.Id
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
    LEFT JOIN dbo.AspNetRoles ro
      ON oc.AccessRoleId = ro.Id
    LEFT JOIN dbo.AspNetRoles rt
      ON otb.TypeAccessGroupId = rt.Id
    LEFT JOIN dbo.Centers cc
      ON oc.ClientCFOId = cc.Id
    LEFT JOIN dbo.AspNetUsers anuc
      ON ob.CreatedByUser = anuc.Id
    LEFT JOIN dbo.AspNetUsers anue
      ON ob.OrderExecuter = anue.Id
    LEFT JOIN dbo.BalanceKeepers bk
      ON ob.PayerId = bk.Id

    inner join OrderPipelineSteps ops
	on ops.OrderTypeId = ob.OrderType and
    ops.ToStatus = ob.CurrentOrderStatus
	
	inner join dbo.OrderStatuses os
	on ops.ToStatus = os.Id	

	LEFT JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
    LEFT JOIN dbo.OrdersPassengerTransport ps
      ON ob.Id = ps.OrderId
	
    WHERE (EXISTS (SELECT *
      FROM dbo.AspNetUsers anu
      LEFT JOIN dbo.AspNetUserRoles anur
        ON anu.Id = anur.UserId
      WHERE anur.RoleId = ro.Id
      AND anur.UserId = @userId))
    AND (EXISTS (SELECT *
      FROM dbo.AspNetUsers anu
      LEFT JOIN dbo.AspNetUserRoles anur
        ON anu.Id = anur.UserId
      WHERE anur.RoleId = rt.Id
      AND anur.UserId = @userId))
    AND (otb.IsTransportType = 1)
	AND (((@isPassOrders = 0) AND (ob.OrderType in (4,5,7)))
	OR ((@isPassOrders = 1) AND (ob.OrderType in (1,3,6))))
    
    AND ((@UseOrderClientFilter = 0)
    OR ((@UseOrderClientFilter = 1)
    AND (ob.ClientId = @FilterOrderClientId)))
    AND ((@UseOrderTypeFilter = 0)
    OR ((@UseOrderTypeFilter = 1)    
	AND (ob.OrderType in (SELECT *  
                                   FROM dbo.SplitString(@FilterOrderTypeId)))))

	AND ((@UseTripTypeFilter = 0)
    OR ((@UseTripTypeFilter = 1)   
    AND (
	((ob.OrderType in (4,5,7)) AND
	(ott.TripType in (SELECT *  FROM dbo.SplitString(@FilterTripTypeId))))
	OR
	((ob.OrderType in (1,3,6)) AND
	(ps.TripType in (SELECT *  FROM dbo.SplitString(@FilterTripTypeId))))
	)))

AND (
	--отбираем по дате заявки
	((@UseOrderDateFilter = 1) AND 
	((ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd)
	OR
	--если проимпортирована, то отбираем по данным черновика
	((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd))))
	OR 
  --отбираем по дате фактической поставки авто
	((@UseAcceptDateFilter = 1)
	 AND (((
	((ob.OrderType in (4,5,7)) AND ((CAST(ott.FromShipperDatetime AS DATE)) >= @FilterAcceptDateBeg)
	AND ((CAST(ott.FromShipperDatetime AS DATE)) <=@FilterAcceptDateEnd))
	OR
	((ob.OrderType in (1,3,6)) AND ((CAST(ps.StartDateTimeOfTrip AS DATE)) >= @FilterAcceptDateBeg)
	AND ((CAST(ps.StartDateTimeOfTrip AS DATE)) <=@FilterAcceptDateEnd))
	)
	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1
				OR ops.FinishOfTheProcess=0)))
	--если проимпортирована, то отбираем по данным черновика
	OR ((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterAcceptDateBeg AND @FilterAcceptDateEnd)))))
        group by os.OrderStatusName, ops.FinishOfTheProcess            
  END


SET ANSI_NULLS ON

/****** Object:  StoredProcedure [dbo].[GetOrdersBKReport]    Script Date: 26.04.2017 23:47:14 ******/
SET ANSI_NULLS ON