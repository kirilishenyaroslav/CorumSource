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
DECLARE @IsFreight int
DECLARE @RateTotalFreight decimal(16, 2) 
DECLARE @IsPriceNegotiated bit

select @RateKm = 0;
select @RateHour = 0;
select @RateMachineHour = 0;
select @IsFreight = 0;
select @RateTotalFreight = 0;
select @IsPriceNegotiated = 0;

 SET @IsFreight = (select cs.TypeSpecId  FROM [dbo].[ContractSpecifications] cs
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
