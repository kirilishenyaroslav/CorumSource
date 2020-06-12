/****** Object:  StoredProcedure [dbo].[GetFinalReport]    Script Date: 14.03.2017 21:21:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetFinalReport] @userId NVARCHAR(128),
@isAdmin BIT,
@UseOrderClientFilter BIT,
@UseOrderTypeFilter BIT,
@FilterOrderClientId NVARCHAR(128),
@FilterOrderTypeId NVARCHAR(128),
@FilterOrderDateBeg DATETIME2,
@FilterOrderDateEnd DATETIME2,
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
 
	--AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd)

	AND ((SELECT top 1 CAST(h.ChangeDateTime AS DATE)
	FROM dbo.OrderPipelineSteps p 
	INNER JOIN dbo.OrderStatusHistory h
	ON h.OldStatus = p.FromStatus  and  h.OrderId = ob.Id
	WHERE  p.OrderTypeId = ob.OrderType and p.StartDateForClient = 1) >= @FilterOrderDateBeg  
	AND    	
	(SELECT top 1 CAST(h.ChangeDateTime AS DATE)
	FROM dbo.OrderPipelineSteps p 
	INNER JOIN dbo.OrderStatusHistory h
	ON h.OldStatus = p.FromStatus  and h.OrderId = ob.Id
	WHERE  p.OrderTypeId = ob.OrderType and p.StartDateForClient = 1) <= @FilterOrderDateEnd
	OR ((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd))
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
--    AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd)
    AND ((SELECT top 1 CAST(h.ChangeDateTime AS DATE)
	FROM dbo.OrderPipelineSteps p 
	INNER JOIN dbo.OrderStatusHistory h
	ON h.OldStatus = p.FromStatus  and  h.OrderId = ob.Id
	WHERE  p.OrderTypeId = ob.OrderType and p.StartDateForClient = 1) >= @FilterOrderDateBeg  
	AND    	
	(SELECT top 1 CAST(h.ChangeDateTime AS DATE)
	FROM dbo.OrderPipelineSteps p 
	INNER JOIN dbo.OrderStatusHistory h
	ON h.OldStatus = p.FromStatus  and h.OrderId = ob.Id
	WHERE  p.OrderTypeId = ob.OrderType and p.StartDateForClient = 1) <= @FilterOrderDateEnd
	OR ((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd))
    group by os.OrderStatusName, ops.FinishOfTheProcess            
  END


SET ANSI_NULLS ON
