/****** Object:  StoredProcedure [dbo].[GetBaseReport]    Script Date: 30.01.2017 22:59:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetBaseReport] @userId NVARCHAR(128),
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
     ,oc.ClientName
     ,cc.Center AS CenterName
     ,anuc.DisplayName AS CreatorDispalyName
     ,anue.DisplayName AS ExecutorDisplayName
     ,otb.TypeName
     ,otb.ShortName AS TypeShortName
     ,otb.IsTransportType
     ,bk.BalanceKeeper AS PayerName
     ,(SELECT TOP 1
          ops.FinishOfTheProcess
        FROM OrderPipelineSteps ops
        WHERE ops.OrderTypeId = ob.OrderType
        AND ops.ToStatus = ob.CurrentOrderStatus)
      AS isFinishOfTheProcess,
	  ott.TruckTypeId,
	  ott.TruckDescription,
	  ott.Weight,
      ott.Volume,
      ott.BoxingDescription,
      ott.DimenssionL,
      ott.DimenssionW,
      ott.DimenssionH,
      ott.VehicleTypeId,
      ott.LoadingTypeId,
      ott.Shipper,
      ott.Consignee,
      ott.FromShipperDatetime,
      ott.ToConsigneeDatetime,
      ott.UnloadingTypeId,
      ott.ShipperCountryId,
      ott.ConsigneeCountryId,
      ott.ShipperCity,
      ott.ConsigneeCity,
      ott.ShipperAdress,
      ott.ConsigneeAdress,
      ott.TripType,
      ott.ShipperContactPerson,
      ott.ShipperContactPersonPhone,
      ott.ConsigneeContactPerson,
      ott.ConsigneeContactPersonPhone,
	  ob.ExecuterNotes

    FROM dbo.OrdersBase ob
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
	INNER JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
    
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
 
    AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd)
	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.FromStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1))
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
     ,(SELECT TOP 1
          ops.FinishOfTheProcess
        FROM OrderPipelineSteps ops
        WHERE ops.OrderTypeId = ob.OrderType
        AND ops.ToStatus = ob.CurrentOrderStatus)
      AS isFinishOfTheProcess,
	  ott.TruckTypeId,
	  ott.TruckDescription,
	  ott.Weight,
      ott.Volume,
      ott.BoxingDescription,
      ott.DimenssionL,
      ott.DimenssionW,
      ott.DimenssionH,
      ott.VehicleTypeId,
      ott.LoadingTypeId,
      ott.Shipper,
      ott.Consignee,
      ott.FromShipperDatetime,
      ott.ToConsigneeDatetime,
      ott.UnloadingTypeId,
      ott.ShipperCountryId,
      ott.ConsigneeCountryId,
      ott.ShipperCity,
      ott.ConsigneeCity,
      ott.ShipperAdress,
      ott.ConsigneeAdress,
      ott.TripType,
      ott.ShipperContactPerson,
      ott.ShipperContactPersonPhone,
      ott.ConsigneeContactPerson,
      ott.ConsigneeContactPersonPhone,
	  ob.ExecuterNotes
    FROM dbo.OrdersBase ob
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
	 INNER JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId

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
    AND (ob.OrderDate BETWEEN @FilterOrderDateBeg AND @FilterOrderDateEnd)
	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.FromStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1))
    
  END
