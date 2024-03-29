SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery40.sql|7|0|C:\Users\LASTOC~1\AppData\Local\Temp\~vs69A4.sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[TruckOrdersInsert] 
@ProjectNum varchar(50),        @OrderDate datetime2(7),		@PayerName varchar(100),
@ClientCFO varchar(100),		@ClientName varchar(100),      
@ApplicationOwner varchar(80),  @OwnerPost varchar(20),         @CreatorContact varchar(100), 
@Shipper varchar(500),		    @Consignee varchar(500),        @FromCountry varchar(100),
@FromCity nvarchar(128),        @AdressFrom varchar(500),	    @ToCountry varchar(100),        
@ToCity nvarchar(128),          @AdressTo varchar(500),
@TruckDescription varchar(500), @Weight decimal(18, 0),		    @Volume decimal(18, 0),
@BoxingDescription varchar(100), @DimenssionL decimal(18, 0),   @DimenssionW decimal(18, 0),
@DimenssionH decimal(18, 0),    @VehicleTypeName varchar(100),  @LoadingTypeName varchar(50),
@UnloadingTypeName varchar(50), @CarNumber int,				    @FromShipperDate datetime2(7),
@FromShipperTime datetime2(7),  @ToConsigneeDate datetime2(7),  @ToConsigneeTime datetime2(7),
@CreateDate datetime2(7),       @CreateTime datetime2(7),       @TruckTypeName varchar(50),
@PointSumma int,				@RouteLength int,               @RouteSelection varchar(50),
@PriotityType varchar(20),      @ShortName nvarchar(128),	    @TripType nvarchar(128),
@ShipperContactPerson varchar(255), @ShipperContactPersonPhone varchar(50), @ConsigneeContactPerson varchar(255), 
@ConsigneeContactPersonPhone varchar(50), @CreateDatetime1 datetime2(7)
AS
BEGIN
	--SET NOCOUNT ON;
    DECLARE @OrderType int, @ClientCFOId int, @ClientId int, @CreatedByUserId nvarchar(128),
	@OrderExecuterId nvarchar(128), @PriotityTypeId int, @TripTypeId int, @FromCountryId int,
	@ToCountryId int, @NeedReturnId int, @PayerId int
		
	select @OrderType = Id
	from [dbo].[OrderTypesBase]
	where LTRIM(RTRIM(UPPER([TypeName]))) = LTRIM(RTRIM(UPPER(@ShortName)))

	if not exists (SELECT Id FROM [dbo].[Centers] WHERE LTRIM(RTRIM(UPPER([Center]))) = LTRIM(RTRIM(UPPER(@ClientCFO))))
	begin
	INSERT INTO [dbo].[Centers]
			([Center])
		VALUES
			(@ClientCFO)
	end

	select @ClientCFOId = Id
	from [dbo].[Centers]
	where LTRIM(RTRIM(UPPER([Center]))) = LTRIM(RTRIM(UPPER(@ClientCFO)))

	if not exists (SELECT Id FROM [dbo].[OrderClients] 
	WHERE LTRIM(RTRIM(UPPER([ClientName]))) = LTRIM(RTRIM(UPPER(@ClientName))) and [ClientCFOId] = @ClientCFOId)
	begin
	INSERT INTO [dbo].[OrderClients]
			([ClientName], [ClientCFOId], [AccessRoleId])
		VALUES
			(@ClientName, @ClientCFOId,
			(select Id from [dbo].[AspNetRoles] where [Name] = 'administrator'))
	end

	select @ClientId = Id
	from [dbo].[OrderClients]
	where LTRIM(RTRIM(UPPER([ClientName])))= LTRIM(RTRIM(UPPER(@ClientName)))
	and [ClientCFOId] = @ClientCFOId

	if not exists (SELECT Id FROM [dbo].[AspNetUsers] WHERE LTRIM(RTRIM(UPPER([DisplayName]))) = LTRIM(RTRIM(UPPER(@ApplicationOwner))))
	begin

	INSERT INTO [dbo].[AspNetUsers]
			([Id], [DisplayName], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled],
			[LockoutEnabled], AccessFailedCount, UserName)
		VALUES
			(newid(), @ApplicationOwner, 1, 0, 0, 1, 0, @ApplicationOwner)	
	end
	
	select @CreatedByUserId = Id
	from [dbo].[AspNetUsers]
	where LTRIM(RTRIM(UPPER([DisplayName]))) = LTRIM(RTRIM(UPPER(@ApplicationOwner)))

	set @PriotityTypeId = 0;
	if (@PriotityType = 'Плановая заявка') set @PriotityTypeId = 0
	else set @PriotityTypeId = 1
	
	if not exists (SELECT Id FROM [dbo].[BalanceKeepers] WHERE LTRIM(RTRIM(UPPER([BalanceKeeper]))) = LTRIM(RTRIM(UPPER(@PayerName))))
	begin	
	INSERT INTO [dbo].[BalanceKeepers]
			([BalanceKeeper])
		VALUES
			(@PayerName)
	end
	
	SELECT @PayerId = Id
	FROM [dbo].[BalanceKeepers]
	WHERE LTRIM(RTRIM(UPPER([BalanceKeeper]))) = LTRIM(RTRIM(UPPER(@PayerName)))
	
	DECLARE @OrdersBaseId int, @OrderTruckTransportId int
	DECLARE @table table (OrdersBaseId int)
	DECLARE @tableOrderTruckTransport table (OrderTruckTransportId int)
	DECLARE @TruckTypeNameId int
	DECLARE @VehicleTypeNameId int
	DECLARE @LoadingTypeNameId int
	DECLARE @UnloadingTypeNameId int, @ShipperCountryId int, @ConsigneeCountryId int


	-- вставка
	IF NOT EXISTS (SELECT Id FROM [dbo].[OrdersBase] WHERE LTRIM(RTRIM(UPPER([ProjectNum]))) = LTRIM(RTRIM(UPPER(@ProjectNum))))
	BEGIN	
	INSERT INTO [dbo].[OrdersBase]
			([ProjectNum],      [CreatedByUser],      [CurrentOrderStatus],
			 [OrderType],       [OrderExecuter],      [PriotityType],
			[OrderDate],        [CreatorPosition],     [CreatorContact], 
			[ClientId],         [PayerId],
			[CreateDatetime])
	OUTPUT inserted.id into @table
		VALUES
			(@ProjectNum,      @CreatedByUserId,    /*17*/8, 
			 @OrderType,       @CreatedByUserId,    @PriotityTypeId,
			 @OrderDate,   	   @ApplicationOwner + @OwnerPost,    @CreatorContact,
			 @ClientId,		   @PayerId,
			 CONVERT(DATETIME, CONVERT(CHAR(8), @CreateDate, 112) + ' ' + CONVERT(CHAR(8), @CreateTime, 108)))
			 
	SELECT @OrdersBaseId = OrdersBaseId from @table

	IF NOT EXISTS (SELECT Id FROM [dbo].[OrderTruckTransport] WHERE [OrderId] = @OrdersBaseId)
		BEGIN	
				
		 IF (LTRIM(RTRIM(UPPER(@TripType))) = 'ГОРОДСКАЯ') set @TripTypeId = 0
		 ELSE IF (LTRIM(RTRIM(UPPER(@TripType))) = 'МЕЖДУГОРОДНЯЯ') set @TripTypeId = 1
		 ELSE SET @TripTypeId = 2
	
	IF NOT EXISTS (SELECT TruckTypeName FROM [dbo].[OrderTruckTypes] WHERE LTRIM(RTRIM(UPPER([TruckTypeName]))) = LTRIM(RTRIM(UPPER(@TruckTypeName))))
	BEGIN	
	INSERT INTO [dbo].[OrderTruckTypes]
			([TruckTypeName])
		VALUES
			(@TruckTypeName)
	END
	
	SELECT @TruckTypeNameId = Id
	FROM [dbo].[OrderTruckTypes]
	WHERE LTRIM(RTRIM(UPPER([TruckTypeName]))) = LTRIM(RTRIM(UPPER(@TruckTypeName)))
	
	IF NOT EXISTS (SELECT VehicleTypeName FROM [dbo].[OrderVehicleTypes] WHERE LTRIM(RTRIM(UPPER([VehicleTypeName]))) = LTRIM(RTRIM(UPPER(@VehicleTypeName))))
	BEGIN	
	INSERT INTO [dbo].[OrderVehicleTypes]
			([VehicleTypeName])
		VALUES
			(@VehicleTypeName)
	END
	
	SELECT @VehicleTypeNameId = Id
	FROM [dbo].[OrderVehicleTypes]
	WHERE LTRIM(RTRIM(UPPER([VehicleTypeName]))) = LTRIM(RTRIM(UPPER(@VehicleTypeName)))	
	
	IF NOT EXISTS (SELECT LoadingTypeName FROM [dbo].[OrderLoadingTypes] WHERE LTRIM(RTRIM(UPPER([LoadingTypeName]))) = LTRIM(RTRIM(UPPER(@LoadingTypeName))))
	BEGIN	
	INSERT INTO [dbo].[OrderLoadingTypes]
			([LoadingTypeName])
		VALUES
			(@LoadingTypeName)
	END
	
	SELECT @LoadingTypeNameId = Id
	FROM [dbo].[OrderLoadingTypes]
	WHERE LTRIM(RTRIM(UPPER([LoadingTypeName]))) = LTRIM(RTRIM(UPPER(@LoadingTypeName)))			
	
	IF NOT EXISTS (SELECT UnloadingTypeName FROM [dbo].[OrderUnloadingTypes] WHERE LTRIM(RTRIM(UPPER([UnloadingTypeName]))) = LTRIM(RTRIM(UPPER(@UnloadingTypeName))))
	BEGIN	
	INSERT INTO [dbo].[OrderUnloadingTypes]
			([UnloadingTypeName])
		VALUES
			(@UnloadingTypeName)
	END
	
	SELECT @UnloadingTypeNameId = Id
	FROM [dbo].[OrderUnloadingTypes]
	WHERE LTRIM(RTRIM(UPPER([UnloadingTypeName]))) = LTRIM(RTRIM(UPPER(@UnloadingTypeName)))	
	
	IF NOT EXISTS (SELECT Сode FROM [dbo].[Countries] WHERE LTRIM(RTRIM(UPPER([Name]))) = LTRIM(RTRIM(UPPER(@FromCountry))))
	BEGIN	
	INSERT INTO [dbo].[Countries]
			([Сode], [Name], [Fullname], [alpha2], [alpha3])
		VALUES
			(((select max([Сode]) from [dbo].[Countries])+1),
			@FromCountry, @FromCountry, ' ',' ')
	END
	
	SELECT @ShipperCountryId = [Сode]
	FROM [dbo].[Countries]
	WHERE LTRIM(RTRIM(UPPER([Name]))) = LTRIM(RTRIM(UPPER(@FromCountry)))

	IF NOT EXISTS (SELECT Сode FROM [dbo].[Countries] WHERE LTRIM(RTRIM(UPPER([Name]))) = LTRIM(RTRIM(UPPER(@ToCountry))))
	BEGIN	
	INSERT INTO [dbo].[Countries]
			([Сode], [Name], [Fullname], [alpha2], [alpha3])
		VALUES
			(((select max([Сode]) from [dbo].[Countries])+1),
			@ToCountry, @ToCountry, ' ',' ')
	END
	
	SELECT @ConsigneeCountryId = [Сode]
	FROM [dbo].[Countries]
	WHERE LTRIM(RTRIM(UPPER([Name]))) = LTRIM(RTRIM(UPPER(@ToCountry)))

		
	INSERT INTO [dbo].[OrderTruckTransport]
				([OrderId],[TruckTypeId],[TruckDescription],[Weight],[Volume],[BoxingDescription]
      ,[DimenssionL],[DimenssionW],[DimenssionH],[VehicleTypeId],[LoadingTypeId],[Shipper],[Consignee]
      ,[FromShipperDatetime],[ToConsigneeDatetime],[UnloadingTypeId],[ShipperCountryId],[ConsigneeCountryId]
      ,[ShipperCity],[ConsigneeCity],[ShipperAdress],[ConsigneeAdress],[TripType],[ShipperContactPerson]
      ,[ShipperContactPersonPhone],[ConsigneeContactPerson],[ConsigneeContactPersonPhone])
	OUTPUT inserted.id into @tableOrderTruckTransport
	VALUES
		(@OrdersBaseId, @TruckTypeNameId, @TruckDescription, @Weight, @Volume, @BoxingDescription,
		@DimenssionL, @DimenssionW, @DimenssionH, @VehicleTypeNameId, @LoadingTypeNameId, @Shipper, @Consignee,
		CONVERT(DATETIME, CONVERT(CHAR(8), @FromShipperDate, 112) + ' ' + CONVERT(CHAR(8), @FromShipperTime, 108)),
		CONVERT(DATETIME, CONVERT(CHAR(8), @ToConsigneeDate, 112) + ' ' + CONVERT(CHAR(8), @ToConsigneeTime, 108)),
		@UnloadingTypeNameId, @ShipperCountryId, @ConsigneeCountryId, 
		@FromCity, @ToCity, @AdressFrom, @AdressTo, @TripTypeId, @ShipperContactPerson, @ShipperContactPersonPhone,
        @ConsigneeContactPerson, @ConsigneeContactPersonPhone)

	 
	SELECT @OrderTruckTransportId = OrderTruckTransportId FROM @tableOrderTruckTransport
	END

	END
END
