USE [uh417455_db]
GO
/****** Object:  StoredProcedure [dbo].[GetFactCars]    Script Date: 22.02.2018 23:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetFactCars] @userId NVARCHAR(128),
@isAdmin BIT,
@UseOrderIdFilter BIT,
@UseExpeditorIdFilter BIT,
@UseContractExpBkInfoFilter BIT,
@UseCarrierInfoFilter BIT,
@UseContractInfoFilter BIT,
@UseCarModelInfoFilter BIT,
@UseCarRegNumFilter BIT,
@UseCarCapacityFilter BIT,
@UseCarDriverInfoFilter BIT,
@UseDriverCardInfoFilter BIT,
@UseDriverContactInfoFilter BIT,
@UseCommentsFilter BIT,
@UseFactShipperFilter BIT,
@UseFactConsigneeFilter BIT,
@UseOrderExDateFilter BIT,
@UseOrderEndDateFilter BIT,

@FilterOrderIdFilter NVARCHAR(MAX),
@FilterExpeditorIdFilter NVARCHAR(MAX), 
@FilterContractExpBkInfoFilter NVARCHAR(MAX),
@FilterCarrierInfoFilter NVARCHAR(MAX),
@FilterContractInfoFilter NVARCHAR(MAX),
@FilterCarModelInfoFilter NVARCHAR(MAX),
@FilterCarRegNumFilter NVARCHAR(MAX),
@FilterCarCapacityFilter NVARCHAR(MAX),
@FilterCarDriverInfoFilter NVARCHAR(MAX),
@FilterDriverCardInfoFilter NVARCHAR(MAX),
@FilterDriverContactInfoFilter NVARCHAR(MAX),
@FilterCommentsFilter NVARCHAR(MAX),
@FilterFactShipperBeg DATETIME2,
@FilterFactShipperEnd DATETIME2,
@FilterFactConsigneeBeg DATETIME2,
@FilterFactConsigneeEnd DATETIME2,

@FilterOrderExDateBeg DATETIME2,
@FilterOrderExDateEnd DATETIME2,
@FilterOrderEndDateBeg DATETIME2,
@FilterOrderEndDateEnd DATETIME2
AS
  IF (@isAdmin = 1)
  BEGIN
    SELECT
      u.OrderId,
	  u.BaseRate,
	  u.BaseRateHoliday,
	  u.CarCapacity,
	  u.CarDriverInfo,
	  u.CarId,
	  u.CarModelInfo,
	  u.CarOwnerInfo,
	  u.CarRegNum,
	  u.CarrierInfo,
	  u.Comments,
	  u.ContractExpBkId,
	  u.ContractId,
	  u.ContractInfo,
	  u.DelayDays,
	  u.DriverCardInfo,
	  u.ExpeditorId,
	  u.ExpeditorName,
	  u.FactConsigneeDateTime,
	  u.FactShipperDateTime,
	  u.Id,
	  u.PlanDistance,
	  u.PlanTimeHoliday,
	  u.PlanTimeWorkDay,
	  u.DriverContactInfo,
	  u.Summ,
	  u.BaseRateWorkDay,
	  co.CarrierName,
	  c.ContractNumber,
	  c.ContractDate,
	  c.DateBeg,
	  c.DateEnd,
	  t.ShortName
    FROM dbo.OrderUsedCars u
    INNER JOIN dbo.OrdersBase ob
   ON u.OrderId = ob.Id  
   LEFT JOIN dbo.CarOwners co
   ON u.ExpeditorId = co.Id
   LEFT JOIN dbo.Contracts c
   ON u.ContractExpBkId = c.Id
   LEFT JOIN dbo.OrderTruckTransport ott
   ON ob.Id = ott.OrderId
   LEFT JOIN dbo.OrdersPassengerTransport ps
   ON ob.Id = ps.OrderId
   LEFT JOIN OrderTypesBase t
   ON t.Id = ob.OrderType
   WHERE
    ((@UseOrderIdFilter = 0)
    OR ((@UseOrderIdFilter = 1)    
	AND (u.OrderId in (SELECT *  FROM dbo.SplitString(@FilterOrderIdFilter)))))

	AND ((@UseExpeditorIdFilter = 0)
    OR ((@UseExpeditorIdFilter = 1)    
	AND (u.ExpeditorId in (SELECT *  FROM dbo.SplitString(@FilterExpeditorIdFilter)))))

	AND ((@UseContractExpBkInfoFilter = 0)
    OR ((@UseContractExpBkInfoFilter = 1)    
	AND (u.ContractExpBkId in (SELECT *  FROM dbo.SplitString(@FilterContractExpBkInfoFilter)))))

	AND ((@UseCarrierInfoFilter = 0)
    OR ((@UseCarrierInfoFilter = 1)    
	AND (u.CarrierInfo in (SELECT *  FROM dbo.SplitString(@FilterCarrierInfoFilter)))))

	AND ((@UseContractInfoFilter = 0)
    OR ((@UseContractInfoFilter = 1)    
	AND (u.ContractInfo in (SELECT *  FROM dbo.SplitString(@FilterContractInfoFilter)))))

	AND ((@UseCarModelInfoFilter = 0)
    OR ((@UseCarModelInfoFilter = 1)    
	AND (u.CarModelInfo in (SELECT *  FROM dbo.SplitString(@FilterCarModelInfoFilter)))))

	AND ((@UseCarRegNumFilter = 0)
    OR ((@UseCarRegNumFilter = 1)    
	AND (u.CarRegNum in (SELECT *  FROM dbo.SplitString(@FilterCarRegNumFilter)))))

	AND ((@UseCarCapacityFilter = 0)
    OR ((@UseCarCapacityFilter = 1)    
	AND (u.CarCapacity in (SELECT *  FROM dbo.SplitString(@FilterCarCapacityFilter)))))

    AND ((@UseCarDriverInfoFilter = 0)
    OR ((@UseCarDriverInfoFilter = 1)    
	AND (u.CarDriverInfo in (SELECT *  FROM dbo.SplitString(@FilterCarDriverInfoFilter)))))

    AND ((@UseDriverCardInfoFilter = 0)
    OR ((@UseDriverCardInfoFilter = 1)    
	AND (u.DriverCardInfo in (SELECT *  FROM dbo.SplitString(@FilterDriverCardInfoFilter)))))

	AND ((@UseDriverContactInfoFilter = 0)
    OR ((@UseDriverContactInfoFilter = 1)    
	AND (u.DriverContactInfo in (SELECT *  FROM dbo.SplitString(@FilterDriverContactInfoFilter)))))

	AND ((@UseCommentsFilter = 0)
    OR ((@UseCommentsFilter = 1)    
	AND (u.Comments in (SELECT *  FROM dbo.SplitString(@FilterCommentsFilter)))))

	AND ((@UseFactShipperFilter = 0)
    OR ((@UseFactShipperFilter = 1)    
	AND ((select convert(date, u.FactShipperDateTime)) BETWEEN @FilterFactShipperBeg AND @FilterFactShipperEnd)))

	AND ((@UseFactConsigneeFilter = 0)
    OR ((@UseFactConsigneeFilter = 1)    
	AND ((select convert(date, u.FactConsigneeDateTime)) BETWEEN @FilterFactConsigneeBeg AND @FilterFactConsigneeEnd)))

	AND ((@UseOrderExDateFilter = 0)
    OR ((@UseOrderExDateFilter = 1)    
	AND ((select convert(date, ob.OrderServiceDateTime)) BETWEEN @FilterOrderExDateBeg AND @FilterOrderExDateEnd)))

	AND ((@UseOrderEndDateFilter = 0)
    OR ((@UseOrderEndDateFilter = 1)
    AND (
	((ob.OrderType in (4,5,7)) AND ((select convert(date, ott.ToConsigneeDatetime)) BETWEEN @FilterOrderEndDateBeg AND @FilterOrderEndDateEnd))	
	OR
	((ob.OrderType in (1,3,6)) AND (	
	((ps.NeedReturn = 1) AND ((select convert(date, ps.ReturnFinishDateTimeOfTrip)) BETWEEN @FilterOrderEndDateBeg AND @FilterOrderEndDateEnd))	
	OR
	((ps.NeedReturn = 0) AND ((select convert(date, ps.FinishDateTimeOfTrip)) BETWEEN @FilterOrderEndDateBeg AND @FilterOrderEndDateEnd)))))))

  END
    ELSE
  BEGIN

   SELECT
      u.OrderId,
	  u.BaseRate,
	  u.BaseRateHoliday,
	  u.CarCapacity,
	  u.CarDriverInfo,
	  u.CarId,
	  u.CarModelInfo,
	  u.CarOwnerInfo,
	  u.CarRegNum,
	  u.CarrierInfo,
	  u.Comments,
	  u.ContractExpBkId,
	  u.ContractId,
	  u.ContractInfo,
	  u.DelayDays,
	  u.DriverCardInfo,
	  u.ExpeditorId,
	  u.ExpeditorName,
	  u.FactConsigneeDateTime,
	  u.FactShipperDateTime,
	  u.Id,
	  u.PlanDistance,
	  u.PlanTimeHoliday,
	  u.PlanTimeWorkDay,
	  u.DriverContactInfo,
	  u.Summ,
	  u.BaseRateWorkDay,
	  co.CarrierName,
	  c.ContractNumber,
	  c.ContractDate,
	  c.DateBeg,
	  c.DateEnd,
	  t.ShortName 
    FROM dbo.OrderUsedCars u
    INNER JOIN dbo.OrdersBase ob
      ON u.OrderId = ob.Id    	   
	LEFT JOIN dbo.OrderClients oc
      ON ob.ClientId = oc.Id
	LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
	LEFT JOIN dbo.AspNetRoles ro
      ON oc.AccessRoleId = ro.Id
    LEFT JOIN dbo.AspNetRoles rt
      ON otb.TypeAccessGroupId = rt.Id  
	  
	LEFT JOIN dbo.CarOwners co
   ON u.ExpeditorId = co.Id
   LEFT JOIN dbo.Contracts c
   ON u.ContractExpBkId = c.Id
   
   LEFT JOIN dbo.OrderTruckTransport ott
   ON ob.Id = ott.OrderId
   LEFT JOIN dbo.OrdersPassengerTransport ps
   ON ob.Id = ps.OrderId 
   LEFT JOIN OrderTypesBase t
   ON t.Id = ob.OrderType      
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
	AND ((@UseOrderIdFilter = 0)
    OR ((@UseOrderIdFilter = 1)    
	AND (u.OrderId in (SELECT *  FROM dbo.SplitString(@FilterOrderIdFilter)))))

	AND ((@UseExpeditorIdFilter = 0)
    OR ((@UseExpeditorIdFilter = 1)    
	AND (u.ExpeditorId in (SELECT *  FROM dbo.SplitString(@FilterExpeditorIdFilter)))))

	AND ((@UseContractExpBkInfoFilter = 0)
    OR ((@UseContractExpBkInfoFilter = 1)    
	AND (u.ContractExpBkId in (SELECT *  FROM dbo.SplitString(@FilterContractExpBkInfoFilter)))))

	AND ((@UseCarrierInfoFilter = 0)
    OR ((@UseCarrierInfoFilter = 1)    
	AND (u.CarrierInfo in (SELECT *  FROM dbo.SplitString(@FilterCarrierInfoFilter)))))

	AND ((@UseContractInfoFilter = 0)
    OR ((@UseContractInfoFilter = 1)    
	AND (u.ContractInfo in (SELECT *  FROM dbo.SplitString(@FilterContractInfoFilter)))))

	AND ((@UseCarModelInfoFilter = 0)
    OR ((@UseCarModelInfoFilter = 1)    
	AND (u.CarModelInfo in (SELECT *  FROM dbo.SplitString(@FilterCarModelInfoFilter)))))

	AND ((@UseCarRegNumFilter = 0)
    OR ((@UseCarRegNumFilter = 1)    
	AND (u.CarRegNum in (SELECT *  FROM dbo.SplitString(@FilterCarRegNumFilter)))))

	AND ((@UseCarCapacityFilter = 0)
    OR ((@UseCarCapacityFilter = 1)    
	AND (u.CarCapacity in (SELECT *  FROM dbo.SplitString(@FilterCarCapacityFilter)))))

    AND ((@UseCarDriverInfoFilter = 0)
    OR ((@UseCarDriverInfoFilter = 1)    
	AND (u.CarDriverInfo in (SELECT *  FROM dbo.SplitString(@FilterCarDriverInfoFilter)))))

    AND ((@UseDriverCardInfoFilter = 0)
    OR ((@UseDriverCardInfoFilter = 1)    
	AND (u.DriverCardInfo in (SELECT *  FROM dbo.SplitString(@FilterDriverCardInfoFilter)))))

	AND ((@UseDriverContactInfoFilter = 0)
    OR ((@UseDriverContactInfoFilter = 1)    
	AND (u.DriverContactInfo in (SELECT *  FROM dbo.SplitString(@FilterDriverContactInfoFilter)))))

	AND ((@UseCommentsFilter = 0)
    OR ((@UseCommentsFilter = 1)    
	AND (u.Comments in (SELECT *  FROM dbo.SplitString(@FilterCommentsFilter)))))

	AND ((@UseFactShipperFilter = 0)
    OR ((@UseFactShipperFilter = 1)    
	AND ((select convert(date, u.FactShipperDateTime)) BETWEEN @FilterFactShipperBeg AND @FilterFactShipperEnd)))

	AND ((@UseFactConsigneeFilter = 0)
    OR ((@UseFactConsigneeFilter = 1)    
	AND ((select convert(date, u.FactConsigneeDateTime)) BETWEEN @FilterFactConsigneeBeg AND @FilterFactConsigneeEnd)))

	AND ((@UseOrderExDateFilter = 0)
    OR ((@UseOrderExDateFilter = 1)    
	AND ((select convert(date, ob.OrderServiceDateTime)) BETWEEN @FilterOrderExDateBeg AND @FilterOrderExDateEnd)))

	AND ((@UseOrderEndDateFilter = 0)
    OR ((@UseOrderEndDateFilter = 1)
    AND (
	((ob.OrderType in (4,5,7)) AND ((select convert(date, ott.ToConsigneeDatetime)) BETWEEN @FilterOrderEndDateBeg AND @FilterOrderEndDateEnd))	
	OR
	((ob.OrderType in (1,3,6)) AND (	
	((ps.NeedReturn = 1) AND ((select convert(date, ps.ReturnFinishDateTimeOfTrip)) BETWEEN @FilterOrderEndDateBeg AND @FilterOrderEndDateEnd))	
	OR
	((ps.NeedReturn = 0) AND ((select convert(date, ps.FinishDateTimeOfTrip)) BETWEEN @FilterOrderEndDateBeg AND @FilterOrderEndDateEnd)))))))

  END
