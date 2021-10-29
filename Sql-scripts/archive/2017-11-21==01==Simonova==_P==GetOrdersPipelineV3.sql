SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetOrdersPipelineV3] @userId NVARCHAR(128),
@isAdmin BIT,
@IsTransport BIT,

@UseStatusesFilter BIT,
@UseOrderCreatorFilter BIT,
@UseOrderExecuterFilter BIT,
@UseOrderTypeFilter BIT,
@UseOrderClientFilter BIT,
@UseOrderPriorityFilter BIT,
@UseOrderDateFilter BIT,
@UseOrderExDateFilter BIT,
@UseFinalStatusFilter BIT,
@UseProjectFilter BIT,
@UseOrderPayerFilter BIT,
@UseOrderOrgFromFilter BIT,
@UseOrderOrgToFilter BIT,

@FilterStatusId NVARCHAR(128),
@FilterOrderCreatorId NVARCHAR(MAX),
@FilterOrderExecuterId NVARCHAR(MAX),
@FilterOrderTypeId NVARCHAR(128),
@FilterOrderClientId NVARCHAR(128),
@FilterOrderPriority INT,
@FilterOrderDateBeg DATETIME2,
@FilterOrderDateEnd DATETIME2,
@FilterOrderExDateBeg DATETIME2,
@FilterOrderExDateEnd DATETIME2,
@FilterFinalStatus BIT,
@FilterProjectId NVARCHAR(128),
@FilterOrderPayerId NVARCHAR(MAX),
@FilterOrderOrgFromId NVARCHAR(MAX),
@FilterOrderOrgToId NVARCHAR(MAX)

AS


  IF (@isAdmin = 1)
  BEGIN
    SELECT
      ob.Id
     ,ob.OrderDate
     ,ob.CreatedByUser
     ,ob.CreateDatetime
     ,ob.OrderType
     ,ob.CurrentOrderStatus
     ,ob.OrderDescription
     ,ob.ClientId
     ,ob.ClientDogId
     ,ob.Summ
     ,ob.UseNotifications
     ,ob.CreatorPosition
     ,ob.CreatorContact
     ,ob.PriotityType
     ,ob.OrderServiceDateTime
     ,ob.OrderExecuter
     ,ob.PayerId
     ,ob.ProjectNum
     ,ob.CarNumber
     ,ob.DistanceDescription
     ,ob.TotalPrice
     ,ob.TotalDistanceLength
     ,ob.IsPrivateOrder
     ,os.Color
     ,os.OrderStatusName
     ,os.ShortName AS OrderStatusShortName
     ,os.FontColor
     ,os.BackgroundColor
     ,os.IconFile
     ,os.IconDescription
     ,ob.ExecuterNotes
     ,oc.ClientName
     ,cc.Center AS CenterName
     ,anuc.DisplayName AS CreatorDispalyName
     ,anue.DisplayName AS ExecutorDisplayName
     ,otb.TypeName
     ,otb.ShortName AS TypeShortName
     ,otb.IsTransportType
     ,bk.BalanceKeeper AS PayerName
     ,p.Id AS ProjectId
     ,p.Code AS ProjectCode
     ,p.Description AS ProjectDescription
     ,(SELECT TOP 1
          ops.FinishOfTheProcess
        FROM OrderPipelineSteps ops
        WHERE ops.OrderTypeId = ob.OrderType
        AND ops.ToStatus = ob.CurrentOrderStatus)
      AS isFinishOfTheProcess

    FROM dbo.OrdersBase ob
    LEFT JOIN dbo.Projects p
      ON ob.ProjectId = p.Id
    LEFT JOIN dbo.OrderClients oc
      ON ob.ClientId = oc.Id
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
    LEFT JOIN dbo.AspNetRoles ro
      ON oc.AccessRoleId = ro.Id
    LEFT JOIN dbo.AspNetRoles rt
      ON otb.TypeAccessGroupId = rt.Id
    LEFT JOIN dbo.OrderStatuses os
      ON ob.CurrentOrderStatus = os.Id
    LEFT JOIN dbo.Centers cc
      ON oc.ClientCFOId = cc.Id
    LEFT JOIN dbo.AspNetUsers anuc
      ON ob.CreatedByUser = anuc.Id
    LEFT JOIN dbo.AspNetUsers anue
      ON ob.OrderExecuter = anue.Id
    LEFT JOIN dbo.BalanceKeepers bk
      ON ob.PayerId = bk.Id
	LEFT JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
    LEFT JOIN dbo.OrdersPassengerTransport ps
      ON ob.Id = ps.OrderId
    WHERE (otb.IsTransportType = @IsTransport)
    AND ((ob.IsPrivateOrder = 0)
    OR ((ob.IsPrivateOrder = 1)
    AND (ob.CreatedByUser = @userId)))
    AND ((@UseStatusesFilter = 0)
    OR ((@UseStatusesFilter = 1)
	AND (ob.CurrentOrderStatus in (SELECT *  
                                   FROM dbo.SplitString(@FilterStatusId)))))
    AND ((@UseOrderCreatorFilter = 0)
    OR ((@UseOrderCreatorFilter = 1)   
	AND (ob.CreatedByUser in (SELECT *  
                                   FROM dbo.SplitStringV2(@FilterOrderCreatorId,',')))))
    AND ((@UseOrderExecuterFilter = 0)
    OR ((@UseOrderExecuterFilter = 1)
	AND (ob.OrderExecuter in (SELECT *  
                                   FROM dbo.SplitStringV2(@FilterOrderExecuterId,',')))))
    AND ((@UseOrderTypeFilter = 0)
    OR ((@UseOrderTypeFilter = 1)   
    AND (ob.OrderType in (SELECT *  
                                   FROM dbo.SplitString(@FilterOrderTypeId)))))
    AND ((@UseOrderClientFilter = 0)
    OR ((@UseOrderClientFilter = 1)    
    AND (ob.ClientId in (SELECT *  
                         FROM dbo.SplitString(@FilterOrderClientId)))))

    AND ((@UseOrderOrgFromFilter = 0)
    OR ((@UseOrderOrgFromFilter = 1)    
    AND (
	((ob.OrderType in (4,5,7)) AND
	(ott.Shipper in (SELECT *  FROM dbo.SplitStringV2(@FilterOrderOrgFromId,','))))
	OR
	((ob.OrderType in (1,3,6)) AND
	(ps.OrgFrom in (SELECT *  FROM dbo.SplitStringV2(@FilterOrderOrgFromId,','))))
	)))     						 
    AND ((@UseOrderOrgToFilter = 0)
    OR ((@UseOrderOrgToFilter = 1)    
    AND (
	((ob.OrderType in (4,5,7)) AND
	(ott.Consignee in (SELECT *  FROM dbo.SplitStringV2(@FilterOrderOrgToId,','))))
	OR
	((ob.OrderType in (1,3,6)) AND
	(ps.OrgTo in (SELECT *  FROM dbo.SplitStringV2(@FilterOrderOrgToId,','))))
	)))
			
    AND ((@UseOrderPayerFilter = 0)
    OR ((@UseOrderPayerFilter = 1)    
    AND (ob.PayerId in (SELECT *  
                         FROM dbo.SplitString(@FilterOrderPayerId)))))

    AND ((@UseOrderPriorityFilter = 0)
    OR ((@UseOrderPriorityFilter = 1)
    AND (ob.PriotityType = @FilterOrderPriority)))
    
     AND ((@UseProjectFilter = 0)
    OR ((@UseProjectFilter = 1)
    AND (exists (SELECT *  
                            FROM dbo.SplitString(@FilterProjectId) AS filterIds
                      INNER JOIN dbo.OrderBaseProjects obp ON CAST(filterIds.Name AS BIGINT) = obp.ProjectId
                           WHERE obp.OrderId = ob.Id)  )))

    AND ((@UseOrderDateFilter = 0)
    OR ((@UseOrderDateFilter = 1)
    AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd)))
    AND ((@UseOrderExDateFilter = 0)
    OR ((@UseOrderExDateFilter = 1)
    --AND (ob.OrderServiceDateTime BETWEEN @FilterOrderExDateBeg AND @FilterOrderExDateEnd)))
	AND ((select convert(date, ob.OrderServiceDateTime)) BETWEEN @FilterOrderExDateBeg AND @FilterOrderExDateEnd)))
    AND ((@UseFinalStatusFilter = 0)
     OR (((@UseFinalStatusFilter = 1)) AND (exists(SELECT *  
                                                     FROM OrderPipelineSteps ops
                                                    WHERE ops.OrderTypeId = ob.OrderType
                                                      AND ops.ToStatus = ob.CurrentOrderStatus
                                                      AND ops.FinishOfTheProcess=@FilterFinalStatus))))
  END
  ELSE
  BEGIN

    SELECT
      ob.Id
     ,ob.OrderDate
     ,ob.CreatedByUser
     ,ob.CreateDatetime
     ,ob.OrderType
     ,ob.CurrentOrderStatus
     ,ob.OrderDescription
     ,ob.ClientId
     ,ob.ClientDogId
     ,ob.Summ
     ,ob.UseNotifications
     ,ob.CreatorPosition
     ,ob.CreatorContact
     ,ob.PriotityType
     ,ob.OrderServiceDateTime
     ,ob.OrderExecuter
     ,ob.ExecuterNotes
     ,ob.PayerId
     ,ob.ProjectNum
     ,ob.CarNumber
     ,ob.DistanceDescription
     ,ob.TotalPrice
     ,ob.TotalDistanceLength
     ,ob.IsPrivateOrder
     ,os.Color
     ,os.OrderStatusName
     ,os.ShortName AS OrderStatusShortName
     ,os.FontColor
     ,os.BackgroundColor
     ,os.IconFile
     ,os.IconDescription
     ,oc.ClientName
     ,cc.Center AS CenterName
     ,anuc.DisplayName AS CreatorDispalyName
     ,anue.DisplayName AS ExecutorDisplayName
     ,otb.TypeName
     ,otb.ShortName AS TypeShortName
     ,otb.IsTransportType
     ,bk.BalanceKeeper AS PayerName
     ,p.Id AS ProjectId
     ,p.Code AS ProjectCode
     ,p.Description AS ProjectDescription
     ,(SELECT TOP 1
          ops.FinishOfTheProcess
        FROM OrderPipelineSteps ops
        WHERE ops.OrderTypeId = ob.OrderType
        AND ops.ToStatus = ob.CurrentOrderStatus)
      AS isFinishOfTheProcess

    FROM dbo.OrdersBase ob
    LEFT JOIN dbo.Projects p
      ON ob.ProjectId = p.Id
    LEFT JOIN dbo.OrderClients oc
      ON ob.ClientId = oc.Id
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
    LEFT JOIN dbo.AspNetRoles ro
      ON oc.AccessRoleId = ro.Id
    LEFT JOIN dbo.AspNetRoles rt
      ON otb.TypeAccessGroupId = rt.Id
    LEFT JOIN dbo.OrderStatuses os
      ON ob.CurrentOrderStatus = os.Id
    LEFT JOIN dbo.Centers cc
      ON oc.ClientCFOId = cc.Id
    LEFT JOIN dbo.AspNetUsers anuc
      ON ob.CreatedByUser = anuc.Id
    LEFT JOIN dbo.AspNetUsers anue
      ON ob.OrderExecuter = anue.Id
    LEFT JOIN dbo.BalanceKeepers bk
      ON ob.PayerId = bk.Id
	LEFT JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
    LEFT JOIN dbo.OrdersPassengerTransport ps
      ON ob.Id = ps.OrderId
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
    AND (otb.IsTransportType = @IsTransport)
    AND ((ob.IsPrivateOrder = 0)
    OR ((ob.IsPrivateOrder = 1)
    AND (ob.CreatedByUser = @userId)))
    AND ((@UseStatusesFilter = 0)
    OR ((@UseStatusesFilter = 1)
    AND (ob.CurrentOrderStatus = @FilterStatusId)))
    AND ((@UseOrderCreatorFilter = 0)
    OR ((@UseOrderCreatorFilter = 1)    
	AND (ob.CreatedByUser in (SELECT *  
                                   FROM dbo.SplitStringV2(@FilterOrderCreatorId,',')))))    
    AND ((@UseOrderExecuterFilter = 0)
    OR ((@UseOrderExecuterFilter = 1)   
	AND (ob.OrderExecuter in (SELECT *  
                                   FROM dbo.SplitStringV2(@FilterOrderExecuterId,',')))))    

    AND ((@UseOrderTypeFilter = 0)
    OR ((@UseOrderTypeFilter = 1)    
	AND (ob.OrderType in (SELECT *  
                                   FROM dbo.SplitString(@FilterOrderTypeId)))))
      AND ((@UseOrderOrgFromFilter = 0)
    OR ((@UseOrderOrgFromFilter = 1)    
	 AND (
	((ob.OrderType in (4,5,7)) AND
	(ott.Shipper in (SELECT *  FROM dbo.SplitStringV2(@FilterOrderOrgFromId,','))))
	OR
	((ob.OrderType in (1,3,6)) AND
	(ps.OrgFrom in (SELECT *  FROM dbo.SplitStringV2(@FilterOrderOrgFromId,','))))
	)))     					 
    AND ((@UseOrderOrgToFilter = 0)
    OR ((@UseOrderOrgToFilter = 1)    
     AND (
	((ob.OrderType in (4,5,7)) AND
	(ott.Consignee in (SELECT *  FROM dbo.SplitStringV2(@FilterOrderOrgToId,','))))
	OR
	((ob.OrderType in (1,3,6)) AND
	(ps.OrgTo in (SELECT *  FROM dbo.SplitStringV2(@FilterOrderOrgToId,','))))
	)))	
    AND ((@UseOrderPayerFilter = 0)
    OR ((@UseOrderPayerFilter = 1)    
    AND (ob.PayerId in (SELECT *  
                         FROM dbo.SplitString(@FilterOrderPayerId)))))

    AND ((@UseOrderClientFilter = 0)
    OR ((@UseOrderClientFilter = 1)
    AND (ob.ClientId = @FilterOrderClientId)))
    AND ((@UseOrderPriorityFilter = 0)
    OR ((@UseOrderPriorityFilter = 1)
    AND (ob.PriotityType = @FilterOrderPriority)))

    AND ((@UseProjectFilter = 0)
    OR ((@UseProjectFilter = 1)
    AND (exists (SELECT *  
                            FROM dbo.SplitString(@FilterProjectId) AS filterIds
                      INNER JOIN dbo.OrderBaseProjects obp ON CAST(filterIds.Name AS BIGINT) = obp.ProjectId
                           WHERE obp.OrderId = ob.Id)  )))


    AND ((@UseOrderDateFilter = 0)
    OR ((@UseOrderDateFilter = 1)
    AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd)))
    AND ((@UseOrderExDateFilter = 0)
    OR ((@UseOrderExDateFilter = 1)
    AND ((select convert(date, ob.OrderServiceDateTime)) BETWEEN @FilterOrderExDateBeg AND @FilterOrderExDateEnd)))
    AND ((@UseFinalStatusFilter = 0)
     OR (((@UseFinalStatusFilter = 1)) AND (exists(SELECT *  
                                                     FROM OrderPipelineSteps ops
                                                    WHERE ops.OrderTypeId = ob.OrderType
                                                      AND ops.ToStatus = ob.CurrentOrderStatus
                                                      AND ops.FinishOfTheProcess=@FilterFinalStatus))))
  END
