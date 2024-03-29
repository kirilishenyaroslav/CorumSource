/****** Object:  StoredProcedure [dbo].[GetStatusReport]    Script Date: 01.03.2017 22:03:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetStatusReport] @userId NVARCHAR(128),
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
      ot.TruckTypeName
     ,bk.BalanceKeeper AS PayerName
     ,COUNT(ob.Id) AS CntAll
     ,COUNT(CASE
        WHEN ob.PriotityType = 0 THEN 1
        ELSE NULL
      END) AS CntZero
     ,COUNT(CASE
        WHEN ob.PriotityType = 1 THEN 1
        ELSE NULL
      END) AS CntOne
    FROM [dbo].[OrdersBase] ob
    INNER JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
    LEFT JOIN dbo.OrderTruckTypes ot
      ON ot.Id = ott.TruckTypeId
    LEFT JOIN dbo.BalanceKeepers bk
      ON ob.PayerId = bk.Id
    WHERE (otb.IsTransportType = 1)
    AND (ob.OrderType IN (4, 5, 7))

    AND ((@UseOrderClientFilter = 0)
    OR ((@UseOrderClientFilter = 1)
    AND (ob.ClientId IN (SELECT
        *
      FROM dbo.SplitString(@FilterOrderClientId))
    )))
    AND ((@UseOrderTypeFilter = 0)
    OR ((@UseOrderTypeFilter = 1)
    AND (ob.OrderType IN (SELECT
        *
      FROM dbo.SplitString(@FilterOrderTypeId))
    )))

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
	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1)))
	--если проимпортирована, то отбираем по данным черновика
	OR ((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd))

    GROUP BY ot.TruckTypeName
            ,bk.BalanceKeeper
    ORDER BY ot.TruckTypeName, bk.BalanceKeeper

  END
  ELSE
  BEGIN
    SELECT
      ot.TruckTypeName
     ,bk.BalanceKeeper AS PayerName
     ,COUNT(ob.Id) AS CntAll
     ,COUNT(CASE
        WHEN ob.PriotityType = 0 THEN 1
        ELSE NULL
      END) AS CntZero
     ,COUNT(CASE
        WHEN ob.PriotityType = 1 THEN 1
        ELSE NULL
      END) AS CntOne
    FROM [dbo].[OrdersBase] ob
    INNER JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
    LEFT JOIN dbo.OrderClients oc
      ON ob.ClientId = oc.Id
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
    LEFT JOIN dbo.OrderTruckTypes ot
      ON ot.Id = ott.TruckTypeId
    LEFT JOIN dbo.BalanceKeepers bk
      ON ob.PayerId = bk.Id
    LEFT JOIN dbo.AspNetRoles ro
      ON oc.AccessRoleId = ro.Id
    LEFT JOIN dbo.AspNetRoles rt
      ON otb.TypeAccessGroupId = rt.Id
    WHERE (otb.IsTransportType = 1)
    AND (ob.OrderType IN (4, 5, 7))
    AND (EXISTS (SELECT
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

    AND ((@UseOrderClientFilter = 0)
    OR ((@UseOrderClientFilter = 1)
    AND (ob.ClientId = @FilterOrderClientId)))
    AND ((@UseOrderTypeFilter = 0)
    OR ((@UseOrderTypeFilter = 1)
    AND (ob.OrderType IN (SELECT
        *
      FROM dbo.SplitString(@FilterOrderTypeId))
    )))

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
	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1)))
	--если проимпортирована, то отбираем по данным черновика
	OR ((ob.CurrentOrderStatus = 17)
	AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd))
	
	GROUP BY ot.TruckTypeName
            ,bk.BalanceKeeper
    ORDER BY ot.TruckTypeName, bk.BalanceKeeper

  END

SET ANSI_NULLS ON
