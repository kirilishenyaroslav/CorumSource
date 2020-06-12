EXEC sp_RENAME 'OrdersBase.SpecificationTypeId' , 'TypeSpecId', 'COLUMN'
go

/****** Object:  StoredProcedure [dbo].[GetSpecifications]    Script Date: 06.06.2017 20:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetSpecifications] 
@AcceptDate DATETIME2,
@isTruck bit,
@tripType int,
@VehicleTypeId int,
@TypeSpecId int
AS
select co.CarrierName, cow.CarrierName as CarrierName1, co.IsForwarder, c.BalanceKeeperId, cgs.NameGroupSpec, 
cgs.DaysDelay,
cs.NameSpecification, /*cs.IsFreight,*/  
t.Id as SpecificationTypeId, 
t.SpecificationType,
cc.CarryCapacity,
r.NameIntervalType,
cs.Id,
cs.DeparturePoint,
cs.ArrivalPoint,
cs.IsTruck,
v.VehicleTypeName,
si.UsedRateName,
si.UsedRateId,
si.RateValue
from [dbo].[Contracts] c
left join dbo.CarOwners co 
 on c.ExpeditorId = co.Id
left join dbo.CarOwners cow 
 on c.CarOwnersId = cow.Id
 inner join dbo.ContractGroupesSpecifications cgs
 on c.Id = cgs.ContractId
 inner join dbo.ContractSpecifications cs
 on cgs.Id = cs.GroupeSpecId
 left join dbo.CarCarryCapacity cc
 on cs.CarryCapacityId = cc.Id
 left join dbo.RouteIntervalType r
 on cs.IntervalTypeId = r.Id
 left join SpecificationTypes t
 on cs.TypeSpecId = t.Id
 left join OrderVehicleTypes v
 on cs.TypeVehicleId = @VehicleTypeId
 CROSS APPLY [dbo].GetSpecificationById(cs.Id) si
 where @AcceptDate BETWEEN c.DateBeg AND c.DateEnd 
AND c.IsActive = 1
AND @AcceptDate BETWEEN cgs.DateBeg AND cgs.DateEnd 
AND cgs.IsActive = 1
--AND @AcceptDate BETWEEN cs.DateBeg AND cs.DateEnd 
AND cs.isTruck = @isTruck
AND cs.RouteTypeId = @tripType
AND ((@VehicleTypeId = 0)
OR ((@VehicleTypeId > 0) AND (cs.TypeVehicleId = @VehicleTypeId)))   
AND  ((@TypeSpecId = 0)
OR ((@TypeSpecId > 0) AND (cs.TypeSpecId = @TypeSpecId)))   
SET ANSI_NULLS ON
