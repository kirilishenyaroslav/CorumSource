/****** Object:  StoredProcedure [dbo].[GetFinalReport]    Script Date: 28.02.2017 19:22:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetFinalReport] @userId NVARCHAR(128),
@isAdmin BIT,
@UseOrderClientFilter BIT,
@UseOrderTypeFilter BIT,
@FilterOrderClientId NVARCHAR(128),
@FilterOrderTypeId NVARCHAR(128),
@FilterOrderDateBeg DATETIME2,
@FilterOrderDateEnd DATETIME2
AS


  IF (@isAdmin = 1)
  BEGIN
    SELECT
      os.OrderStatusName, ops.FinishOfTheProcess,
      COUNT(ob.Id) AS CntAll
	   	  
    FROM dbo.OrdersBase ob
    LEFT JOIN dbo.OrderClients oc
      ON ob.ClientId = oc.Id
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
    LEFT JOIN dbo.AspNetRoles ro
      ON oc.AccessRoleId = ro.Id
    LEFT JOIN dbo.AspNetRoles rt
      ON otb.TypeAccessGroupId = rt.Id
   /* LEFT JOIN dbo.OrderStatuses os
      ON ob.CurrentOrderStatus = os.Id*/
    LEFT JOIN dbo.Centers cc
      ON oc.ClientCFOId = cc.Id
    LEFT JOIN dbo.AspNetUsers anuc
      ON ob.CreatedByUser = anuc.Id
    LEFT JOIN dbo.AspNetUsers anue
      ON ob.OrderExecuter = anue.Id
    LEFT JOIN dbo.BalanceKeepers bk
      ON ob.PayerId = bk.Id
	INNER JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
    	
    inner join OrderPipelineSteps ops
	on ops.OrderTypeId = ob.OrderType and
    ops.ToStatus = ob.CurrentOrderStatus
	
	inner join dbo.OrderStatuses st
	on ops.FromStatus = st.Id	
	
	inner join dbo.OrderStatuses os
	on ops.ToStatus = os.Id	
	
    WHERE (otb.IsTransportType = 1)
    AND (ob.OrderType in (4,5,7))
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
	/*AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1))*/
				)
	--если проимпортирована, то отбираем по данным черновика
	OR ((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd))
	group by os.OrderStatusName, ops.FinishOfTheProcess
    END
  ELSE
  BEGIN

    SELECT
      os.OrderStatusName, ops.FinishOfTheProcess,
      COUNT(ob.Id) AS CntAll
	
	FROM dbo.OrdersBase ob
    LEFT JOIN dbo.OrderClients oc
      ON ob.ClientId = oc.Id
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
    LEFT JOIN dbo.AspNetRoles ro
      ON oc.AccessRoleId = ro.Id
    LEFT JOIN dbo.AspNetRoles rt
      ON otb.TypeAccessGroupId = rt.Id
    /*LEFT JOIN dbo.OrderStatuses os
      ON ob.CurrentOrderStatus = os.Id*/
    LEFT JOIN dbo.Centers cc
      ON oc.ClientCFOId = cc.Id
    LEFT JOIN dbo.AspNetUsers anuc
      ON ob.CreatedByUser = anuc.Id
    LEFT JOIN dbo.AspNetUsers anue
      ON ob.OrderExecuter = anue.Id
    LEFT JOIN dbo.BalanceKeepers bk
      ON ob.PayerId = bk.Id
	 INNER JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId

    inner join OrderPipelineSteps ops
	on ops.OrderTypeId = ob.OrderType and
    ops.ToStatus = ob.CurrentOrderStatus
	
	inner join dbo.OrderStatuses st
	on ops.FromStatus = st.Id	
	
	inner join dbo.OrderStatuses os
	on ops.ToStatus = os.Id	
	

    WHERE (EXISTS (SELECT
        *
      FROM dbo.AspNetUsers anu
      LEFT JOIN dbo.AspNetUserRoles anur
        ON anu.Id = anur.UserId
      WHERE anur.RoleId = ro.Id
      AND anur.UserId = @userId)
    )
    AND (EXISTS (SELECT
        *
      FROM dbo.AspNetUsers anu
      LEFT JOIN dbo.AspNetUserRoles anur
        ON anu.Id = anur.UserId
      WHERE anur.RoleId = rt.Id
      AND anur.UserId = @userId)
    )
    AND (otb.IsTransportType = 1)
    AND (ob.OrderType in (4,5,7))
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
	/*AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1))*/)
	--если проимпортирована, то отбираем по данным черновика
	OR ((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd))
    group by os.OrderStatusName, ops.FinishOfTheProcess            
  END
