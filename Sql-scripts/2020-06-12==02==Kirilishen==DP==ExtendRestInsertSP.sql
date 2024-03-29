
ALTER PROCEDURE [dbo].[RestsSnapshotInsert] 
@InnerPartyKey varchar(500),       @Producer varchar(500),       @Product varchar(100),
@Shifr varchar(50),                @Figure varchar(50),          @Measure varchar(50),
@Weight decimal(16,2),             @pType varchar(100),          @pGroup varchar(500),
@pRecieverPlan varchar(500),       @pRecieverFact varchar(500),  @RecieverGroupPlan varchar(100),
@InnerOrderNum varchar(50),        @OrderedBy varchar(500),      @OrderNum varchar(50),
@QuantityBefore decimal(18,2),     @PE_Before decimal(18,2),     @PF_Before decimal(18,2),
@PCP_Before decimal(18,2),         @PCPC_Before decimal(18,2),   @FCP_Before decimal(18,2),
@FCPC_Before decimal(18,2),        @BP_Before decimal(18,2),     @PE_After decimal(18,2),
@PF_After decimal(18,2),           @PCP_After decimal(18,2),     @PCPC_After decimal(18,2),
@FCP_After decimal(18,2),          @FCPC_After decimal(18,2),    @BP_After decimal(18,2),
@QuantityAfter decimal(18,2),      @Storage varchar(100),        @StorageCity varchar(50),
@StorageCountry varchar(10),       @Сenter varchar(100),         @BalanceKeeper varchar(100),
@ReadyForSaleStatus varchar(50),   @ReserveStatus varchar(50),   @ProduceDate varchar(50),
@ReconcervationDate varchar(50),   @TermOnStorage int,           @PrihodDocType varchar(100),
@PrihodDocNum varchar(50),         @PrihodDocDate varchar(50),   @BalanceCurrency varchar(50),
@CurrencyIndexToUAH decimal(18,2), @BacodeProduct nvarchar(50),  @BacodeConsignment nvarchar(50),   
@BacodesAll nvarchar(max),         @Shifr_MDM nvarchar(50),      @id_snapshot INT
AS
BEGIN
  -- процедура вставки остатков (при импорте)
	
	SET NOCOUNT ON;

	-- вставка остатков
    INSERT INTO [dbo].[RestsSnapshot]
           ([id_snapshot],       [InnerPartyKey],      [Producer],
            [Product],           [Shifr],              [Figure],
            [Measure],           [Weight],             [pType],
            [pGroup],            [pRecieverPlan],      [pRecieverFact],
            [RecieverGroupPlan], [InnerOrderNum],      [OrderedBy],
            [OrderNum],          [QuantityBefore],     [PE_Before],
            [PF_Before],         [PCP_Before],         [PCPC_Before],
            [FCP_Before],        [FCPC_Before],        [BP_Before],
            [PE_After],          [PF_After],           [PCP_After],
            [PCPC_After],        [FCP_After],          [FCPC_After],
            [BP_After],          [QuantityAfter],      [Storage],
            [StorageCity],       [StorageCountry],     [Сenter],
            [BalanceKeeper],     [ReadyForSaleStatus], [ReserveStatus],
            [ProduceDate],       [ReconcervationDate], [TermOnStorage],
            [PrihodDocType],     [PrihodDocNum],       [PrihodDocDate],
            [BalanceCurrency],   [CurrencyIndexToUAH], [BacodeProduct],
			[BacodeConsignment], [BacodesAll],         [Shifr_MDM])
     VALUES
           (@id_snapshot,        @InnerPartyKey,       @Producer,       
		    @Product,    		 @Shifr ,              @Figure ,          
			@Measure,			 @Weight,              @pType,          
			@pGroup,			 @pRecieverPlan,       @pRecieverFact, 
			@RecieverGroupPlan,  @InnerOrderNum,       @OrderedBy,      
			@OrderNum,			 @QuantityBefore,      @PE_Before,	
			@PF_Before,			 @PCP_Before,          @PCPC_Before,
			@FCP_Before,		 @FCPC_Before,         @BP_Before,    
			@PE_After,  		 @PF_After,            @PCP_After,     
			@PCPC_After,		 @FCP_After,           @FCPC_After,   
			@BP_After, 			 @QuantityAfter,       @Storage,     
			@StorageCity,		 @StorageCountry,      @Сenter,         
			@BalanceKeeper,		 @ReadyForSaleStatus,  @ReserveStatus,   
			@ProduceDate,		 @ReconcervationDate,  @TermOnStorage,           
			@PrihodDocType,		 @PrihodDocNum,        @PrihodDocDate, 
			@BalanceCurrency,	 @CurrencyIndexToUAH,  @BacodeProduct,
			@BacodeConsignment,  @BacodesAll,          @Shifr_MDM)
END 