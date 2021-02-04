/****** Object:  StoredProcedure [dbo].[OrdersInsert]    Script Date: 17.10.2016 22:27:02 ******/
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
ALTER PROCEDURE [dbo].[OrdersInsert] 
@ProjectNum varchar(50),       @ShortName nvarchar(128),         @PriotityType nvarchar(128), 
@OrderDate datetime2(7),	   @—onfirmedDate datetime2(7),	     @AcceptedDate datetime2(7),
@TripType nvarchar(128),       @FromCountry varchar(100),        @FromCity nvarchar(128),
@AdressFrom varchar(500),      @OrgFrom varchar(500),            @StartDateTimeOfTrip datetime2(7),
@StartDateTimeOfTrip2 datetime2(7), @ToCountry varchar(100),     @ToCity varchar(100),
@AdressTo  varchar(500),       @OrgTo varchar(500),              @FinishDateTimeOfTrip datetime2(7),
@FinishDateTimeOfTrip2 datetime2(7), @NeedReturn varchar(50),    @ReturnStartDateTimeOfTrip datetime2(7),
@ReturnStartDateTimeOfTrip2 datetime2(7), @ReturnFinishDateTimeOfTrip datetime2(7), @ReturnFinishDateTimeOfTrip2 datetime2(7), 
@PassInfo varchar(MAX),        @TripDescription varchar(MAX),    @ClientCFO varchar(100),
@ClientName varchar(100),      @ProjectNumber varchar(100),      @PayerName varchar(100),
@CreatedByUser nvarchar(128),  @OrderExecuter nvarchar(128),     @CreatorPosition varchar(100), 
@CreatorContact varchar(100),  @Attachment varchar(100),         @ExpeditorName varchar(255),
@ContractInfo varchar(100),    @CarrierInfo varchar(100),        @CarModelInfo varchar(100),
@CarRegNum varchar(20),        @CarDriverInfo varchar(100),      @DriverCardInfo varchar(100),
@DriverContactInfo varchar(100), @Comments varchar(255),                               
@CreateDatetime datetime2(7)
AS
BEGIN
  -- ÔÓˆÂ‰Û‡ ‚ÒÚ‡‚ÍË (ÔË ËÏÔÓÚÂ)
	SET NOCOUNT ON;
	DECLARE @OrderType int, @ClientCFOId int, @ClientId int, @CreatedByUserId nvarchar(128),
	@OrderExecuterId nvarchar(128), @PriotityTypeId int, @TripTypeId int, @FromCountryId int,
	@ToCountryId int, @NeedReturnId int, @PayerId int
	

	if not exists (SELECT Id FROM [dbo].[OrderTypesBase] WHERE LTRIM(RTRIM(UPPER([ShortName]))) = LTRIM(RTRIM(UPPER(@ShortName))))
	begin
	INSERT INTO [dbo].[OrderTypesBase]
			([TypeName], [IsActive], [ShortName])
		VALUES
			(@ShortName, 1, @ShortName)
	end

	select @OrderType = Id
	from [dbo].[OrderTypesBase]
	where LTRIM(RTRIM(UPPER([ShortName]))) = LTRIM(RTRIM(UPPER(@ShortName)))

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

if not exists (SELECT Id FROM [dbo].[AspNetUsers] WHERE LTRIM(RTRIM(UPPER([DisplayName]))) = LTRIM(RTRIM(UPPER(@CreatedByUser))))
	begin

	INSERT INTO [dbo].[AspNetUsers]
			([Id], [DisplayName], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled],
			[LockoutEnabled], AccessFailedCount, UserName)
		VALUES
			(newid(), @CreatedByUser, 1, 0, 0, 1, 0, @CreatedByUser)	
	end
	
	select @CreatedByUserId = Id
	from [dbo].[AspNetUsers]
	where LTRIM(RTRIM(UPPER([DisplayName]))) = LTRIM(RTRIM(UPPER(@CreatedByUser)))

	if not exists (SELECT Id FROM [dbo].[AspNetUsers] WHERE LTRIM(RTRIM(UPPER([DisplayName]))) = LTRIM(RTRIM(UPPER(@OrderExecuter))))
	begin	
	INSERT INTO [dbo].[AspNetUsers]
			([Id], [DisplayName], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled],
			[LockoutEnabled], AccessFailedCount, UserName)
		VALUES
			(newid(), @OrderExecuter, 1, 0, 0, 1, 0, @OrderExecuter)
	end
	
	select @OrderExecuterId = Id
	from [dbo].[AspNetUsers]
	where LTRIM(RTRIM(UPPER([DisplayName]))) = LTRIM(RTRIM(UPPER(@OrderExecuter)))

	set @PriotityTypeId = 0;
	if (@PriotityType = 'œÎ‡ÌÓ‚‡ˇ Á‡ˇ‚Í‡') set @PriotityTypeId = 0
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

	DECLARE @OrdersBaseId int, @OrdersPassengerTransportId int
	DECLARE @table table (OrdersBaseId int)
	DECLARE @tablePassengerTransport table (OrdersPassengerTransportId int)
	
	-- ‚ÒÚ‡‚Í‡
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
			(@ProjectNum,      @CreatedByUserId,      9999, 
			 @OrderType,       @OrderExecuterId,    @PriotityTypeId,
			 @OrderDate,   	   @CreatorPosition,    @CreatorContact,
			 @ClientId,		   @PayerId,
			 @CreateDatetime)
			 
	SELECT @OrdersBaseId = OrdersBaseId from @table

		IF NOT EXISTS (SELECT Id FROM [dbo].[OrdersPassengerTransport] WHERE [OrderId] = @OrdersBaseId)
		BEGIN	
				
		 IF (LTRIM(RTRIM(UPPER(@TripType))) = '√Œ–Œƒ— ¿ﬂ') set @TripTypeId = 0
		 ELSE IF (LTRIM(RTRIM(UPPER(@TripType))) = 'Ã≈∆ƒ”√Œ–ŒƒÕﬂﬂ') set @TripTypeId = 1
		 ELSE SET @TripTypeId = 2
	
	IF NOT EXISTS (SELECT —ode FROM [dbo].[Countries] WHERE LTRIM(RTRIM(UPPER([Name]))) = LTRIM(RTRIM(UPPER(@FromCountry))))
	BEGIN	
	INSERT INTO [dbo].[Countries]
			([—ode], [Name], [Fullname], [alpha2], [alpha3])
		VALUES
			(((select max([—ode]) from [dbo].[Countries])+1),
			@FromCountry, @FromCountry, ' ',' ')
	END
	
	SELECT @FromCountryId = [—ode]
	FROM [dbo].[Countries]
	WHERE LTRIM(RTRIM(UPPER([Name]))) = LTRIM(RTRIM(UPPER(@FromCountry)))

	IF NOT EXISTS (SELECT —ode FROM [dbo].[Countries] WHERE LTRIM(RTRIM(UPPER([Name]))) = LTRIM(RTRIM(UPPER(@ToCountry))))
	BEGIN	
	INSERT INTO [dbo].[Countries]
			([—ode], [Name], [Fullname], [alpha2], [alpha3])
		VALUES
			(((select max([—ode]) from [dbo].[Countries])+1),
			@ToCountry, @ToCountry, ' ',' ')
	END
	
	SELECT @ToCountryId = [—ode]
	FROM [dbo].[Countries]
	WHERE LTRIM(RTRIM(UPPER([Name]))) = LTRIM(RTRIM(UPPER(@ToCountry)))

	 IF (LTRIM(RTRIM(UPPER(@NeedReturn))) = 'ƒ¿') SET @NeedReturnId = 1
	 ELSE SET @NeedReturnId = 0
			
	INSERT INTO [dbo].[OrdersPassengerTransport]
				([TripType], [FromCountry], [FromCity], [AdressFrom], [OrgFrom],
				[StartDateTimeOfTrip], [ToCountry], [ToCity], [AdressTo], [OrgTo],
				[FinishDateTimeOfTrip], [NeedReturn], [ReturnStartDateTimeOfTrip],
				[ReturnFinishDateTimeOfTrip], [PassInfo], [TripDescription], [OrderId])
	OUTPUT inserted.id into @tablePassengerTransport
	VALUES
		(@TripTypeId, @FromCountryId, @FromCity, @AdressFrom, @OrgFrom, 
		CONVERT(DATETIME, CONVERT(CHAR(8), @StartDateTimeOfTrip, 112) + ' ' + CONVERT(CHAR(8), @StartDateTimeOfTrip2, 108)),
		@ToCountryId, @ToCity, @AdressTo, @OrgTo,
		CONVERT(DATETIME, CONVERT(CHAR(8), @FinishDateTimeOfTrip, 112) + ' ' + CONVERT(CHAR(8), @FinishDateTimeOfTrip2, 108)),
		@NeedReturnId,
		CONVERT(DATETIME, CONVERT(CHAR(8), @ReturnStartDateTimeOfTrip, 112) + ' ' + CONVERT(CHAR(8), @ReturnStartDateTimeOfTrip2, 108)),
		CONVERT(DATETIME, CONVERT(CHAR(8), @ReturnFinishDateTimeOfTrip, 112) + ' ' + CONVERT(CHAR(8), @ReturnFinishDateTimeOfTrip2, 108)),
		@PassInfo, @TripDescription, @OrdersBaseId)
			 
	SELECT @OrdersPassengerTransportId = OrdersPassengerTransportId FROM @tablePassengerTransport
	END

	IF NOT EXISTS (SELECT Id FROM [dbo].[OrderUsedCars] WHERE [OrderId] = @OrdersBaseId)
		begin
		INSERT INTO [dbo].[OrderUsedCars]
				([OrderId], [ExpeditorName], [ContractInfo], [CarrierInfo], [CarModelInfo], 
				[CarRegNum], [CarDriverInfo], [DriverCardInfo], [DriverContactInfo], 
				[Comments])	
	VALUES
		(@OrdersBaseId, @ExpeditorName, @ContractInfo, @CarrierInfo, @CarModelInfo,
		@CarRegNum, @CarDriverInfo, @DriverCardInfo, @DriverContactInfo,
		@Comments)
			 

		END
	END
END
