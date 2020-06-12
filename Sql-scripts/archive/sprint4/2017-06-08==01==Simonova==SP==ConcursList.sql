SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetSpecificationById]( 
    @SpecificationsId int
)
RETURNS @SpecificationsTable TABLE
   (
       UsedRateName     nvarchar(100),
	   UsedRateId       int,
	   RateValue        nvarchar(100)	   
   )
AS
BEGIN
DECLARE @RateKm decimal(16, 2)
DECLARE @RateHour decimal(16, 2)
DECLARE @RateMachineHour decimal(16, 2) 
DECLARE @IsFreight bit
DECLARE @RateTotalFreight decimal(16, 2) 
DECLARE @IsPriceNegotiated bit

select @RateKm = 0;
select @RateHour = 0;
select @RateMachineHour = 0;
select @IsFreight = 0;
select @RateTotalFreight = 0;
select @IsPriceNegotiated = 0;

 SELECT @IsFreight = (select cs.TypeSpecId  FROM [dbo].[ContractSpecifications] cs
         where cs.Id = @SpecificationsId)
   
  if (@IsFreight = 1)
  begin
   SELECT @RateTotalFreight = (select cs.RateTotalFreight  FROM [dbo].[ContractSpecifications] cs
         where cs.Id = @SpecificationsId)
  
    INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES (CAST(@RateTotalFreight as varchar) + ' грн', 1, @RateTotalFreight)
		end
		else
		begin
   SELECT @RateKm = (select cs.RateKm  FROM [dbo].[ContractSpecifications] cs
         where cs.Id = @SpecificationsId)
   
   SELECT @RateHour = (select cs.RateHour  FROM [dbo].[ContractSpecifications] cs
         where cs.Id = @SpecificationsId)

	SELECT @RateMachineHour = (select cs.RateMachineHour  FROM [dbo].[ContractSpecifications] cs
         where cs.Id = @SpecificationsId)

	 if (@RateKm > 0)		    
         INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES (CAST(@RateKm as varchar) + ' грн/км', 2, @RateKm)
   
	  if (@RateHour > 0)		    
         INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES (CAST(@RateHour as varchar) + ' грн/час', 3, @RateHour)
      
	    if (@RateMachineHour > 0)		    
         INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES (CAST(@RateMachineHour as varchar) + '  грн маш./час', 4, @RateMachineHour)
                   
		if ((@RateKm = 0) and (@RateHour = 0) and (@RateMachineHour = 0))		    
		    INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES (CAST(@RateKm as varchar) + ' грн/км', 2, @RateKm)
		end


       SELECT @IsPriceNegotiated = (select cs.IsPriceNegotiated  FROM [dbo].[ContractSpecifications] cs
         where cs.Id = @SpecificationsId)
 
  if (@IsPriceNegotiated = 1)
  
    INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES ('Дог. цена', 5, @IsPriceNegotiated)

   RETURN
END


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
cs.NameSpecification, sn.SpecName,
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
 on cs.TypeVehicleId = v.Id
 left join SpecificationNames sn
 on sn.Id = cs.NameId
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
AND (v.Id = @VehicleTypeId)
SET ANSI_NULLS ON
