use [uh417455_db2]
go
ALTER TABLE [dbo].[AclRecord]  WITH CHECK ADD  CONSTRAINT [FK_AclRecord_CustomerRole_CustomerRoleId] FOREIGN KEY([CustomerRoleId])
REFERENCES [dbo].[CustomerRole] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AclRecord] CHECK CONSTRAINT [FK_AclRecord_CustomerRole_CustomerRoleId]
GO


ALTER TABLE [dbo].[ActivityLog]  WITH CHECK ADD  CONSTRAINT [FK_ActivityLog_ActivityLogType_ActivityLogTypeId] FOREIGN KEY([ActivityLogTypeId])
REFERENCES [dbo].[ActivityLogType] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_ActivityLogType_ActivityLogTypeId]
GO
ALTER TABLE [dbo].[ActivityLog]  WITH CHECK ADD  CONSTRAINT [FK_ActivityLog_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [FK_ActivityLog_Customer_CustomerId]
GO

ALTER TABLE [dbo].[AdditionalRoutePoints]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalRoutePoints_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO
ALTER TABLE [dbo].[AdditionalRoutePoints] CHECK CONSTRAINT [FK_AdditionalRoutePoints_Order]
GO
ALTER TABLE [dbo].[AdditionalRoutePoints]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalRoutePoints_Organization] FOREIGN KEY([RoutePointId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[AdditionalRoutePoints] CHECK CONSTRAINT [FK_AdditionalRoutePoints_Organization]
GO

ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_Country_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_Country_CountryId]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_StateProvince_StateProvinceId] FOREIGN KEY([StateProvinceId])
REFERENCES [dbo].[StateProvince] ([Id])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_StateProvince_StateProvinceId]
GO

ALTER TABLE [dbo].[AddressAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_AddressAttributeValue_AddressAttribute_AddressAttributeId] FOREIGN KEY([AddressAttributeId])
REFERENCES [dbo].[AddressAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AddressAttributeValue] CHECK CONSTRAINT [FK_AddressAttributeValue_AddressAttribute_AddressAttributeId]
GO

ALTER TABLE [dbo].[Affiliate]  WITH CHECK ADD  CONSTRAINT [FK_Affiliate_Address_AddressId] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Affiliate] CHECK CONSTRAINT [FK_Affiliate_Address_AddressId]
GO

ALTER TABLE [dbo].[AspNetMenuRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetMenuRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetMenuRoles] CHECK CONSTRAINT [FK_dbo.AspNetMenuRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetMenuRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetMenuRoles_dbo.MenuStructure_Id] FOREIGN KEY([MenuId])
REFERENCES [dbo].[MenuStructure] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetMenuRoles] CHECK CONSTRAINT [FK_dbo.AspNetMenuRoles_dbo.MenuStructure_Id]
GO

ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO

ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO

ALTER TABLE [dbo].[BackInStockSubscription]  WITH CHECK ADD  CONSTRAINT [FK_BackInStockSubscription_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BackInStockSubscription] CHECK CONSTRAINT [FK_BackInStockSubscription_Customer_CustomerId]
GO
ALTER TABLE [dbo].[BackInStockSubscription]  WITH CHECK ADD  CONSTRAINT [FK_BackInStockSubscription_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BackInStockSubscription] CHECK CONSTRAINT [FK_BackInStockSubscription_Product_ProductId]
GO

ALTER TABLE [dbo].[BlogComment]  WITH CHECK ADD  CONSTRAINT [FK_BlogComment_BlogPost_BlogPostId] FOREIGN KEY([BlogPostId])
REFERENCES [dbo].[BlogPost] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogComment] CHECK CONSTRAINT [FK_BlogComment_BlogPost_BlogPostId]
GO
ALTER TABLE [dbo].[BlogComment]  WITH CHECK ADD  CONSTRAINT [FK_BlogComment_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogComment] CHECK CONSTRAINT [FK_BlogComment_Customer_CustomerId]
GO
ALTER TABLE [dbo].[BlogComment]  WITH CHECK ADD  CONSTRAINT [FK_BlogComment_Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogComment] CHECK CONSTRAINT [FK_BlogComment_Store_StoreId]
GO

ALTER TABLE [dbo].[BlogPost]  WITH CHECK ADD  CONSTRAINT [FK_BlogPost_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlogPost] CHECK CONSTRAINT [FK_BlogPost_Language_LanguageId]
GO

ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FKCars_CarOwners] FOREIGN KEY([CarOwnersId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FKCars_CarOwners]
GO
ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FKCars_CarsFuelType] FOREIGN KEY([FuelTypeId])
REFERENCES [dbo].[CarsFuelType] ([Id])
GO
ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FKCars_CarsFuelType]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ìàðêà' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'Model'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ãîñíîìåð' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'Number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ÔÈÎ âîäèòåëÿ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'Driver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ñåðèÿ ïðàâ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'DriverLicenseSeria'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Âèä òîïëèâà' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'FuelTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ðàñõîä íà 100êì â ðåæèìå ãîðîä' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'ConsumptionCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ðàñõîä íà 100êì â ðåæèìå çà ãîðîä' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'ConsumptionHighway'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Êîëè÷åñòâî ïàññàæèðîâ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'PassNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'íîìåð ïðàâ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'DriverLicenseNumber'
GO

ALTER TABLE [dbo].[CheckoutAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_CheckoutAttributeValue_CheckoutAttribute_CheckoutAttributeId] FOREIGN KEY([CheckoutAttributeId])
REFERENCES [dbo].[CheckoutAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CheckoutAttributeValue] CHECK CONSTRAINT [FK_CheckoutAttributeValue_CheckoutAttribute_CheckoutAttributeId]
GO

ALTER TABLE [dbo].[ContractGroupesSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_GroupesSpec] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[ContractGroupesSpecifications] CHECK CONSTRAINT [FK_Contracts_GroupesSpec]
GO
ALTER TABLE [dbo].[ContractGroupesSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_GroupesSpec_AspNetUsers] FOREIGN KEY([CreatedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ContractGroupesSpecifications] CHECK CONSTRAINT [FK_GroupesSpec_AspNetUsers]
GO

ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_BalanceKeepers] FOREIGN KEY([BalanceKeeperId])
REFERENCES [dbo].[BalanceKeepers] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FK_Contracts_BalanceKeepers]
GO
ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_CarOwners] FOREIGN KEY([ExpeditorId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FK_Contracts_CarOwners]
GO
ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FKContracts_CarOwners] FOREIGN KEY([CarOwnersId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FKContracts_CarOwners]
GO

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_GroupesSpec_Specifications] FOREIGN KEY([GroupeSpecId])
REFERENCES [dbo].[ContractGroupesSpecifications] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_GroupesSpec_Specifications]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Routes_Specifications] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Routes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Routes_Specifications]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_AspNetUsers] FOREIGN KEY([CreatedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_AspNetUsers]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_CarCarryCapacity] FOREIGN KEY([CarryCapacityId])
REFERENCES [dbo].[CarCarryCapacity] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_CarCarryCapacity]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Names] FOREIGN KEY([NameId])
REFERENCES [dbo].[SpecificationNames] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Names]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_RouteIntervalType] FOREIGN KEY([IntervalTypeId])
REFERENCES [dbo].[RouteIntervalType] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_RouteIntervalType]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_RouteTypes] FOREIGN KEY([RouteTypeId])
REFERENCES [dbo].[RouteTypes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_RouteTypes]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Types] FOREIGN KEY([TypeSpecId])
REFERENCES [dbo].[SpecificationTypes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Types]
GO
ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Specifications_Vehicles] FOREIGN KEY([TypeVehicleId])
REFERENCES [dbo].[OrderVehicleTypes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Specifications_Vehicles]
GO


ALTER TABLE [dbo].[Customer_CustomerRole_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Customer_CustomerRole_Mapping_Customer_Customer_Id] FOREIGN KEY([Customer_Id])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Customer_CustomerRole_Mapping] NOCHECK CONSTRAINT [FK_Customer_CustomerRole_Mapping_Customer_Customer_Id]
GO
ALTER TABLE [dbo].[Customer_CustomerRole_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Customer_CustomerRole_Mapping_CustomerRole_CustomerRole_Id] FOREIGN KEY([CustomerRole_Id])
REFERENCES [dbo].[CustomerRole] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Customer_CustomerRole_Mapping] NOCHECK CONSTRAINT [FK_Customer_CustomerRole_Mapping_CustomerRole_CustomerRole_Id]
GO

ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Address_BillingAddress_Id] FOREIGN KEY([BillingAddress_Id])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Address_BillingAddress_Id]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Address_ShippingAddress_Id] FOREIGN KEY([ShippingAddress_Id])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Address_ShippingAddress_Id]
GO

ALTER TABLE [dbo].[CustomerAddresses]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerAddresses_Address_Address_Id] FOREIGN KEY([Address_Id])
REFERENCES [dbo].[Address] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerAddresses] NOCHECK CONSTRAINT [FK_CustomerAddresses_Address_Address_Id]
GO
ALTER TABLE [dbo].[CustomerAddresses]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerAddresses_Customer_Customer_Id] FOREIGN KEY([Customer_Id])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerAddresses] NOCHECK CONSTRAINT [FK_CustomerAddresses_Customer_Customer_Id]
GO

ALTER TABLE [dbo].[CustomerAttributeValue]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerAttributeValue_CustomerAttribute_CustomerAttributeId] FOREIGN KEY([CustomerAttributeId])
REFERENCES [dbo].[CustomerAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerAttributeValue] NOCHECK CONSTRAINT [FK_CustomerAttributeValue_CustomerAttribute_CustomerAttributeId]
GO

ALTER TABLE [dbo].[CustomerPassword]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerPassword_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerPassword] NOCHECK CONSTRAINT [FK_CustomerPassword_Customer_CustomerId]
GO

ALTER TABLE [dbo].[Discount_AppliedToCategories]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToCategories_Category_Category_Id] FOREIGN KEY([Category_Id])
REFERENCES [dbo].[Category] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToCategories] NOCHECK CONSTRAINT [FK_Discount_AppliedToCategories_Category_Category_Id]
GO
ALTER TABLE [dbo].[Discount_AppliedToCategories]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToCategories_Discount_Discount_Id] FOREIGN KEY([Discount_Id])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToCategories] NOCHECK CONSTRAINT [FK_Discount_AppliedToCategories_Discount_Discount_Id]
GO

ALTER TABLE [dbo].[Discount_AppliedToProducts]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToProducts_Discount_Discount_Id] FOREIGN KEY([Discount_Id])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToProducts] NOCHECK CONSTRAINT [FK_Discount_AppliedToProducts_Discount_Discount_Id]
GO
ALTER TABLE [dbo].[Discount_AppliedToProducts]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToProducts_Product_Product_Id] FOREIGN KEY([Product_Id])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToProducts] NOCHECK CONSTRAINT [FK_Discount_AppliedToProducts_Product_Product_Id]
GO

ALTER TABLE [dbo].[Discount_AppliedToManufacturers]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToManufacturers_Discount_Discount_Id] FOREIGN KEY([Discount_Id])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToManufacturers] NOCHECK CONSTRAINT [FK_Discount_AppliedToManufacturers_Discount_Discount_Id]
GO
ALTER TABLE [dbo].[Discount_AppliedToManufacturers]  WITH NOCHECK ADD  CONSTRAINT [FK_Discount_AppliedToManufacturers_Manufacturer_Manufacturer_Id] FOREIGN KEY([Manufacturer_Id])
REFERENCES [dbo].[Manufacturer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Discount_AppliedToManufacturers] NOCHECK CONSTRAINT [FK_Discount_AppliedToManufacturers_Manufacturer_Manufacturer_Id]
GO

ALTER TABLE [dbo].[DiscountRequirement]  WITH NOCHECK ADD  CONSTRAINT [FK_DiscountRequirement_Discount_DiscountId] FOREIGN KEY([DiscountId])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiscountRequirement] NOCHECK CONSTRAINT [FK_DiscountRequirement_Discount_DiscountId]
GO
ALTER TABLE [dbo].[DiscountRequirement]  WITH NOCHECK ADD  CONSTRAINT [FK_DiscountRequirement_DiscountRequirement_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[DiscountRequirement] ([Id])
GO
ALTER TABLE [dbo].[DiscountRequirement] NOCHECK CONSTRAINT [FK_DiscountRequirement_DiscountRequirement_ParentId]
GO

ALTER TABLE [dbo].[DiscountUsageHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_DiscountUsageHistory_Discount_DiscountId] FOREIGN KEY([DiscountId])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiscountUsageHistory] NOCHECK CONSTRAINT [FK_DiscountUsageHistory_Discount_DiscountId]
GO
ALTER TABLE [dbo].[DiscountUsageHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_DiscountUsageHistory_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiscountUsageHistory] NOCHECK CONSTRAINT [FK_DiscountUsageHistory_Order_OrderId]
GO

ALTER TABLE [dbo].[DocsSnapshot]  WITH NOCHECK ADD  CONSTRAINT [FK_DocsSnapshot_LogisticSnapshots] FOREIGN KEY([id_snapshot])
REFERENCES [dbo].[LogisticSnapshots] ([id_spanshot])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocsSnapshot] NOCHECK CONSTRAINT [FK_DocsSnapshot_LogisticSnapshots]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Внутренний идентификатор, который однозначно идентифицирует партию' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'InnerPartyKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Производитель' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Producer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Наименование номенклатуры товара' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Product'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Артикул (шифр) товара' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Shifr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Информация о чертеже товара/изделия' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Figure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ед измерения' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Measure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Вес единицы товара в кг.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тип товара (товар или услуга)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Группа товара (группа номенклатуры)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Грузополучатель (плановый)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pRecieverPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Грузополучатель (фактический) ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pRecieverFact'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Группа грузополучателя (клиент)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RecieverGroupPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Внутренний номер заказа ПП ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'InnerOrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Заказчик изготовления продукции' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'OrderedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер заказа покупателя в базе ПП' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'OrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество товара в партии на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'QuantityPrihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для конечного потребителя  на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PE_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для первого покупателя на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PF_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая полная единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCP_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая переменная единицы товара  на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCPC_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая полная единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCP_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая переменная единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCPC_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Балансовая стоимость единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BP_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для конечного потребителя на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PE_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для первого покупателя на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PF_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая полная единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCP_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая переменная единицы товара  на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCPC_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая полная единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCP_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая переменная единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCPC_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Балансовая стоимость единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BP_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество товара в партии на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'QuantityRashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Наименование склада' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Storage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Расположение склада город' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'StorageCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Расположение склада страна' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'StorageCountry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Информация о центре финансовой ответственности' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Сenter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Балансодержатель' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BalanceKeeper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Признак ограничения к отгрузке' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ReadyForSaleStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Статус резерва' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ReserveStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата производства' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ProduceDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата реконсервации' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ReconcervationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество дней фактического хранения партии на складе' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'TermOnStorage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Документ прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Документ прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RashodDocType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RashodDocNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RashodDocDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Валюта балансовая' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BalanceCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Курс валюты к  грн' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'CurrencyIndexToUAH'
GO

ALTER TABLE [dbo].[ExternalAuthenticationRecord]  WITH NOCHECK ADD  CONSTRAINT [FK_ExternalAuthenticationRecord_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExternalAuthenticationRecord] NOCHECK CONSTRAINT [FK_ExternalAuthenticationRecord_Customer_CustomerId]
GO

ALTER TABLE [dbo].[FAQAnswers]  WITH NOCHECK ADD  CONSTRAINT [FK_PK_FAQAnswers_FAQGroupes] FOREIGN KEY([GroupId])
REFERENCES [dbo].[FAQGroupes] ([Id])
GO
ALTER TABLE [dbo].[FAQAnswers] NOCHECK CONSTRAINT [FK_PK_FAQAnswers_FAQGroupes]
GO

ALTER TABLE [dbo].[Forums_Forum]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Forum_Forums_Group_ForumGroupId] FOREIGN KEY([ForumGroupId])
REFERENCES [dbo].[Forums_Group] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forums_Forum] NOCHECK CONSTRAINT [FK_Forums_Forum_Forums_Group_ForumGroupId]
GO

ALTER TABLE [dbo].[Forums_Post]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Post_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_Post] NOCHECK CONSTRAINT [FK_Forums_Post_Customer_CustomerId]
GO
ALTER TABLE [dbo].[Forums_Post]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Post_Forums_Topic_TopicId] FOREIGN KEY([TopicId])
REFERENCES [dbo].[Forums_Topic] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forums_Post] NOCHECK CONSTRAINT [FK_Forums_Post_Forums_Topic_TopicId]
GO

ALTER TABLE [dbo].[Forums_PostVote]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_PostVote_Forums_Post_ForumPostId] FOREIGN KEY([ForumPostId])
REFERENCES [dbo].[Forums_Post] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forums_PostVote] NOCHECK CONSTRAINT [FK_Forums_PostVote_Forums_Post_ForumPostId]
GO

ALTER TABLE [dbo].[Forums_PrivateMessage]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_PrivateMessage_Customer_FromCustomerId] FOREIGN KEY([FromCustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_PrivateMessage] NOCHECK CONSTRAINT [FK_Forums_PrivateMessage_Customer_FromCustomerId]
GO
ALTER TABLE [dbo].[Forums_PrivateMessage]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_PrivateMessage_Customer_ToCustomerId] FOREIGN KEY([ToCustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_PrivateMessage] NOCHECK CONSTRAINT [FK_Forums_PrivateMessage_Customer_ToCustomerId]
GO

ALTER TABLE [dbo].[Forums_Subscription]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Subscription_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_Subscription] NOCHECK CONSTRAINT [FK_Forums_Subscription_Customer_CustomerId]
GO

ALTER TABLE [dbo].[Forums_Topic]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Topic_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Forums_Topic] NOCHECK CONSTRAINT [FK_Forums_Topic_Customer_CustomerId]
GO
ALTER TABLE [dbo].[Forums_Topic]  WITH NOCHECK ADD  CONSTRAINT [FK_Forums_Topic_Forums_Forum_ForumId] FOREIGN KEY([ForumId])
REFERENCES [dbo].[Forums_Forum] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forums_Topic] NOCHECK CONSTRAINT [FK_Forums_Topic_Forums_Forum_ForumId]
GO

ALTER TABLE [dbo].[GiftCard]  WITH NOCHECK ADD  CONSTRAINT [FK_GiftCard_OrderItem_PurchasedWithOrderItemId] FOREIGN KEY([PurchasedWithOrderItemId])
REFERENCES [dbo].[OrderItem] ([Id])
GO
ALTER TABLE [dbo].[GiftCard] NOCHECK CONSTRAINT [FK_GiftCard_OrderItem_PurchasedWithOrderItemId]
GO

ALTER TABLE [dbo].[GiftCardUsageHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_GiftCardUsageHistory_GiftCard_GiftCardId] FOREIGN KEY([GiftCardId])
REFERENCES [dbo].[GiftCard] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GiftCardUsageHistory] NOCHECK CONSTRAINT [FK_GiftCardUsageHistory_GiftCard_GiftCardId]
GO
ALTER TABLE [dbo].[GiftCardUsageHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_GiftCardUsageHistory_Order_UsedWithOrderId] FOREIGN KEY([UsedWithOrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GiftCardUsageHistory] NOCHECK CONSTRAINT [FK_GiftCardUsageHistory_Order_UsedWithOrderId]
GO

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'не пустое для остатков' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNotNullForRest'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'если движение, то должно быть при QuantityRashod<>0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNotZeroForQPrihodNotZeroForDocs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'если движение, то должно быть пустое если=0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNullForQPrihodZeroForDocs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'если движение, то должно быть ПУСТО при QuantityPrihod<>0 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNullForQPrihodNotZeroForDocs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Пустое для остатков' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNullForRest'
GO

ALTER TABLE [dbo].[LocaleStringResource]  WITH NOCHECK ADD  CONSTRAINT [FK_LocaleStringResource_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LocaleStringResource] NOCHECK CONSTRAINT [FK_LocaleStringResource_Language_LanguageId]
GO

ALTER TABLE [dbo].[LocalizedProperty]  WITH NOCHECK ADD  CONSTRAINT [FK_LocalizedProperty_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LocalizedProperty] NOCHECK CONSTRAINT [FK_LocalizedProperty_Language_LanguageId]
GO

ALTER TABLE [dbo].[Log]  WITH NOCHECK ADD  CONSTRAINT [FK_Log_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Log] NOCHECK CONSTRAINT [FK_Log_Customer_CustomerId]
GO

ALTER TABLE [dbo].[LogImportErrors]  WITH NOCHECK ADD  CONSTRAINT [FK_LogImportErrors_LogisticSnapshots] FOREIGN KEY([idSnapshot])
REFERENCES [dbo].[LogisticSnapshots] ([id_spanshot])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogImportErrors] NOCHECK CONSTRAINT [FK_LogImportErrors_LogisticSnapshots]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 - ошибка, 2 - комментарий' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogImportErrors', @level2type=N'COLUMN',@level2name=N'isCommentType'
GO

ALTER TABLE [dbo].[LoginHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_LoginHistory_AspNetUsers1] FOREIGN KEY([userId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoginHistory] NOCHECK CONSTRAINT [FK_LoginHistory_AspNetUsers1]
GO

ALTER TABLE [dbo].[News]  WITH NOCHECK ADD  CONSTRAINT [FK_News_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[News] NOCHECK CONSTRAINT [FK_News_Language_LanguageId]
GO

ALTER TABLE [dbo].[NewsComment]  WITH NOCHECK ADD  CONSTRAINT [FK_NewsComment_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsComment] NOCHECK CONSTRAINT [FK_NewsComment_Customer_CustomerId]
GO
ALTER TABLE [dbo].[NewsComment]  WITH NOCHECK ADD  CONSTRAINT [FK_NewsComment_News_NewsItemId] FOREIGN KEY([NewsItemId])
REFERENCES [dbo].[News] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsComment] NOCHECK CONSTRAINT [FK_NewsComment_News_NewsItemId]
GO
ALTER TABLE [dbo].[NewsComment]  WITH NOCHECK ADD  CONSTRAINT [FK_NewsComment_Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsComment] NOCHECK CONSTRAINT [FK_NewsComment_Store_StoreId]
GO

ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Address_BillingAddressId] FOREIGN KEY([BillingAddressId])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_Address_BillingAddressId]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Address_PickupAddressId] FOREIGN KEY([PickupAddressId])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_Address_PickupAddressId]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Address_ShippingAddressId] FOREIGN KEY([ShippingAddressId])
REFERENCES [dbo].[Address] ([Id])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_Address_ShippingAddressId]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_Customer_CustomerId]
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_RewardPointsHistory_RewardPointsHistoryEntryId] FOREIGN KEY([RewardPointsHistoryEntryId])
REFERENCES [dbo].[RewardPointsHistory] ([Id])
GO
ALTER TABLE [dbo].[Order] NOCHECK CONSTRAINT [FK_Order_RewardPointsHistory_RewardPointsHistoryEntryId]
GO

--ALTER TABLE [dbo].[OrderAttachments] ADD  CONSTRAINT [DF_OrderAttachments_AddedDateTime]  DEFAULT (getdate()) FOR [AddedDateTime]
--GO
ALTER TABLE [dbo].[OrderAttachments]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderAttachments_AspNetUsers] FOREIGN KEY([AddedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderAttachments] NOCHECK CONSTRAINT [FK_OrderAttachments_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderAttachments]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderAttachments_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderAttachments] NOCHECK CONSTRAINT [FK_OrderAttachments_OrdersBase]
GO
ALTER TABLE [dbo].[OrderAttachments]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderAttachments_OrdersDocTypes] FOREIGN KEY([DocType])
REFERENCES [dbo].[OrdersDocTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderAttachments] NOCHECK CONSTRAINT [FK_OrderAttachments_OrdersDocTypes]
GO

ALTER TABLE [dbo].[OrderBaseProjects]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBaseProjects_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO
ALTER TABLE [dbo].[OrderBaseProjects] NOCHECK CONSTRAINT [FK_OrderBaseProjects_OrdersBase]
GO
ALTER TABLE [dbo].[OrderBaseProjects]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBaseProjects_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[OrderBaseProjects] NOCHECK CONSTRAINT [FK_OrderBaseProjects_Projects]
GO

ALTER TABLE [dbo].[OrderBaseSpecification]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBaseSpecification_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO
ALTER TABLE [dbo].[OrderBaseSpecification] NOCHECK CONSTRAINT [FK_OrderBaseSpecification_OrdersBase]
GO
ALTER TABLE [dbo].[OrderBaseSpecification]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBaseSpecification_SpecificationTypes] FOREIGN KEY([SpecificationId])
REFERENCES [dbo].[SpecificationTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderBaseSpecification] NOCHECK CONSTRAINT [FK_OrderBaseSpecification_SpecificationTypes]
GO

ALTER TABLE [dbo].[OrderClients]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderClients_AspNetRoles] FOREIGN KEY([AccessRoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderClients] NOCHECK CONSTRAINT [FK_OrderClients_AspNetRoles]
GO
ALTER TABLE [dbo].[OrderClients]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderClients_Centers] FOREIGN KEY([ClientCFOId])
REFERENCES [dbo].[Centers] ([Id])
GO
ALTER TABLE [dbo].[OrderClients] NOCHECK CONSTRAINT [FK_OrderClients_Centers]
GO

ALTER TABLE [dbo].[OrderCompetitiveList]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderCompetitiveList_ContractSpecifications] FOREIGN KEY([SpecificationId])
REFERENCES [dbo].[ContractSpecifications] ([Id])
GO
ALTER TABLE [dbo].[OrderCompetitiveList] NOCHECK CONSTRAINT [FK_OrderCompetitiveList_ContractSpecifications]
GO
ALTER TABLE [dbo].[OrderCompetitiveList]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderCompetitiveList_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderCompetitiveList] NOCHECK CONSTRAINT [FK_OrderCompetitiveList_OrdersBase]
GO

ALTER TABLE [dbo].[OrderConcursListsSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderConcursListsSteps] FOREIGN KEY([StepId])
REFERENCES [dbo].[OrderConcursSteps] ([Id])
GO
ALTER TABLE [dbo].[OrderConcursListsSteps] NOCHECK CONSTRAINT [FK_OrderConcursListsSteps]
GO
ALTER TABLE [dbo].[OrderConcursListsSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderConcursListsSteps2] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderConcursListsSteps] NOCHECK CONSTRAINT [FK_OrderConcursListsSteps2]
GO
ALTER TABLE [dbo].[OrderConcursListsSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderConcursListsUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderConcursListsSteps] NOCHECK CONSTRAINT [FK_OrderConcursListsUsers]
GO

ALTER TABLE [dbo].[OrderDogs]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderDogs_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDogs] NOCHECK CONSTRAINT [FK_OrderDogs_OrderClients]
GO

ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_AspNetUsers] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_AspNetUsersEx] FOREIGN KEY([ExecuterId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_AspNetUsersEx]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_OrderClients]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_OrderFilterSettings2] FOREIGN KEY([OrderFilterSetId])
REFERENCES [dbo].[OrderFilterSettings2] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_OrderFilterSettings2]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_OrderStatuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_OrderStatuses]
GO
ALTER TABLE [dbo].[OrderFilters]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilters_OrderTypesBase] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO
ALTER TABLE [dbo].[OrderFilters] NOCHECK CONSTRAINT [FK_OrderFilters_OrderTypesBase]
GO

ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_AspNetUsers] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_AspNetUsersCur] FOREIGN KEY([IdCurrentUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_AspNetUsersCur]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_AspNetUsersEx] FOREIGN KEY([ExecuterId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_AspNetUsersEx]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_OrderClients]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_OrderStatuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_OrderStatuses]
GO
ALTER TABLE [dbo].[OrderFilterSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings_OrderTypesBase] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings] NOCHECK CONSTRAINT [FK_OrderFilterSettings_OrderTypesBase]
GO

ALTER TABLE [dbo].[OrderFilterSettings2]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderFilterSettings2_AspNetUsersCur] FOREIGN KEY([IdCurrentUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderFilterSettings2] NOCHECK CONSTRAINT [FK_OrderFilterSettings2_AspNetUsersCur]
GO

ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_Order_OrderId]
GO
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_Product_ProductId]
GO

ALTER TABLE [dbo].[OrderNote]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderNote_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderNote] NOCHECK CONSTRAINT [FK_OrderNote_Order_OrderId]
GO

ALTER TABLE [dbo].[OrderNotifications]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderNotifications] FOREIGN KEY([Reciever])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderNotifications] NOCHECK CONSTRAINT [FK_OrderNotifications]
GO
ALTER TABLE [dbo].[OrderNotifications]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderNotificationsCreator] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderNotifications] NOCHECK CONSTRAINT [FK_OrderNotificationsCreator]
GO
ALTER TABLE [dbo].[OrderNotifications]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderNotificationsTypes] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrderNotificationTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderNotifications] NOCHECK CONSTRAINT [FK_OrderNotificationsTypes]
GO

ALTER TABLE [dbo].[OrderObservers]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderObservers_AspNetUsers] FOREIGN KEY([userId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderObservers] NOCHECK CONSTRAINT [FK_OrderObservers_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderObservers]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderObservers_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderObservers] NOCHECK CONSTRAINT [FK_OrderObservers_OrdersBase]
GO

ALTER TABLE [dbo].[OrderPipelineSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderPipelineSteps_AspNetRoles] FOREIGN KEY([AccessRoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderPipelineSteps] NOCHECK CONSTRAINT [FK_OrderPipelineSteps_AspNetRoles]
GO
ALTER TABLE [dbo].[OrderPipelineSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderPipelineSteps_OrderStatuses] FOREIGN KEY([FromStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderPipelineSteps] NOCHECK CONSTRAINT [FK_OrderPipelineSteps_OrderStatuses]
GO
ALTER TABLE [dbo].[OrderPipelineSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderPipelineSteps_OrderStatuses1] FOREIGN KEY([ToStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderPipelineSteps] NOCHECK CONSTRAINT [FK_OrderPipelineSteps_OrderStatuses1]
GO
ALTER TABLE [dbo].[OrderPipelineSteps]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderPipelineSteps_OrderTypesBase] FOREIGN KEY([OrderTypeId])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO
ALTER TABLE [dbo].[OrderPipelineSteps] NOCHECK CONSTRAINT [FK_OrderPipelineSteps_OrderTypesBase]
GO

--ALTER TABLE [dbo].[OrdersBase] ADD  CONSTRAINT [DF_OrdersBase_CreateDatetime]  DEFAULT (getdate()) FOR [CreateDatetime]
--GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderBase_Routes] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Routes] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrderBase_Routes]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_AspNetUsers] FOREIGN KEY([CreatedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_AspNetUsers]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_AspNetUsers1] FOREIGN KEY([OrderExecuter])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_AspNetUsers1]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_BalanceKeepers] FOREIGN KEY([PayerId])
REFERENCES [dbo].[BalanceKeepers] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_BalanceKeepers]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_OrderClients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[OrderClients] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_OrderClients]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_OrderDogs] FOREIGN KEY([ClientDogId])
REFERENCES [dbo].[OrderDogs] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_OrderDogs]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_OrderStatuses] FOREIGN KEY([CurrentOrderStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_OrderStatuses]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_OrderTypesBase] FOREIGN KEY([OrderType])
REFERENCES [dbo].[OrderTypesBase] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_OrderTypesBase]
GO
ALTER TABLE [dbo].[OrdersBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersBase_SpecificationTypes] FOREIGN KEY([TypeSpecId])
REFERENCES [dbo].[SpecificationTypes] ([Id])
GO
ALTER TABLE [dbo].[OrdersBase] NOCHECK CONSTRAINT [FK_OrdersBase_SpecificationTypes]
GO

ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_Countries] FOREIGN KEY([FromCountry])
REFERENCES [dbo].[Countries] ([Сode])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_OrdersPassengerTransport_Countries]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_Countries1] FOREIGN KEY([ToCountry])
REFERENCES [dbo].[Countries] ([Сode])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_OrdersPassengerTransport_Countries1]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_OrdersPassengerTransport_OrdersBase]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrdersPassengerTransport_RouteTypes] FOREIGN KEY([TripType])
REFERENCES [dbo].[RouteTypes] ([Id])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_OrdersPassengerTransport_RouteTypes]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_Passenger_OrgFrom] FOREIGN KEY([OrgFromId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_Passenger_OrgFrom]
GO
ALTER TABLE [dbo].[OrdersPassengerTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_Passenger_OrgTo] FOREIGN KEY([OrgToId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrdersPassengerTransport] NOCHECK CONSTRAINT [FK_Passenger_OrgTo]
GO

--ALTER TABLE [dbo].[OrderStatusHistory] ADD  CONSTRAINT [DF_OrderStatusHistory_ChangeDateTime]  DEFAULT (getdate()) FOR [ChangeDateTime]
--GO
ALTER TABLE [dbo].[OrderStatusHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderStatusHistory_AspNetUsers] FOREIGN KEY([ChangedByUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderStatusHistory] NOCHECK CONSTRAINT [FK_OrderStatusHistory_AspNetUsers]
GO
ALTER TABLE [dbo].[OrderStatusHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderStatusHistory_OrdersBase] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderStatusHistory] NOCHECK CONSTRAINT [FK_OrderStatusHistory_OrdersBase]
GO
ALTER TABLE [dbo].[OrderStatusHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderStatusHistory_OrderStatuses] FOREIGN KEY([OldStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderStatusHistory] NOCHECK CONSTRAINT [FK_OrderStatusHistory_OrderStatuses]
GO
ALTER TABLE [dbo].[OrderStatusHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderStatusHistory_OrderStatuses1] FOREIGN KEY([NewStatus])
REFERENCES [dbo].[OrderStatuses] ([Id])
GO
ALTER TABLE [dbo].[OrderStatusHistory] NOCHECK CONSTRAINT [FK_OrderStatusHistory_OrderStatuses1]
GO

ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport] FOREIGN KEY([TruckTypeId])
REFERENCES [dbo].[OrderTruckTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport_ConsigneeCountries] FOREIGN KEY([ConsigneeCountryId])
REFERENCES [dbo].[Countries] ([Сode])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport_ConsigneeCountries]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport_RouteTypes] FOREIGN KEY([TripType])
REFERENCES [dbo].[RouteTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport_RouteTypes]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport_ShipperCountries] FOREIGN KEY([ShipperCountryId])
REFERENCES [dbo].[Countries] ([Сode])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport_ShipperCountries]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport2] FOREIGN KEY([VehicleTypeId])
REFERENCES [dbo].[OrderVehicleTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport2]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport3] FOREIGN KEY([LoadingTypeId])
REFERENCES [dbo].[OrderLoadingTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport3]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport4] FOREIGN KEY([UnloadingTypeId])
REFERENCES [dbo].[OrderUnloadingTypes] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport4]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTruckTransport5] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_OrderTruckTransport5]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_Passenger_Consignee] FOREIGN KEY([ConsigneeId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_Passenger_Consignee]
GO
ALTER TABLE [dbo].[OrderTruckTransport]  WITH NOCHECK ADD  CONSTRAINT [FK_Passenger_Shipper] FOREIGN KEY([ShipperId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[OrderTruckTransport] NOCHECK CONSTRAINT [FK_Passenger_Shipper]
GO

ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AccessFK] FOREIGN KEY([TypeAccessGroupId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AccessFK]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetAnnonymousUsers] FOREIGN KEY([UserForAnnonymousForm])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetAnnonymousUsers]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetRoles] FOREIGN KEY([UserRoleIdForClientData])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetRoles]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetRoles1] FOREIGN KEY([UserRoleIdForExecuterData])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetRoles1]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetRoles2] FOREIGN KEY([UserRoleIdForCompetitiveList])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetRoles2]
GO
ALTER TABLE [dbo].[OrderTypesBase]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetUsers] FOREIGN KEY([DefaultExecuterId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OrderTypesBase] NOCHECK CONSTRAINT [FK_OrderTypesBase_AspNetUsers]
GO

ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCars] FOREIGN KEY([CarId])
REFERENCES [dbo].[Cars] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCars]
GO
ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCars_CarOwners] FOREIGN KEY([ExpeditorId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCars_CarOwners]
GO
ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCarsContract] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCarsContract]
GO
ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCarsContracts1] FOREIGN KEY([ContractExpBkId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCarsContracts1]
GO
ALTER TABLE [dbo].[OrderUsedCars]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderUsedCarsOrder] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrdersBase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderUsedCars] NOCHECK CONSTRAINT [FK_OrderUsedCarsOrder]
GO

ALTER TABLE [dbo].[Organization]  WITH NOCHECK ADD  CONSTRAINT [FK_Organization_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Сode])
GO
ALTER TABLE [dbo].[Organization] NOCHECK CONSTRAINT [FK_Organization_Countries]
GO

ALTER TABLE [dbo].[PermissionRecord_Role_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionRecord_Role_Mapping_CustomerRole_CustomerRole_Id] FOREIGN KEY([CustomerRole_Id])
REFERENCES [dbo].[CustomerRole] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermissionRecord_Role_Mapping] NOCHECK CONSTRAINT [FK_PermissionRecord_Role_Mapping_CustomerRole_CustomerRole_Id]
GO
ALTER TABLE [dbo].[PermissionRecord_Role_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionRecord_Role_Mapping_PermissionRecord_PermissionRecord_Id] FOREIGN KEY([PermissionRecord_Id])
REFERENCES [dbo].[PermissionRecord] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermissionRecord_Role_Mapping] NOCHECK CONSTRAINT [FK_PermissionRecord_Role_Mapping_PermissionRecord_PermissionRecord_Id]
GO

ALTER TABLE [dbo].[Permissions]  WITH NOCHECK ADD  CONSTRAINT [FK_Permissions_AspNetRoles] FOREIGN KEY([roleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[Permissions] NOCHECK CONSTRAINT [FK_Permissions_AspNetRoles]
GO
ALTER TABLE [dbo].[Permissions]  WITH NOCHECK ADD  CONSTRAINT [FK_Permissions_MenuStructure] FOREIGN KEY([menuId])
REFERENCES [dbo].[MenuStructure] ([Id])
GO
ALTER TABLE [dbo].[Permissions] NOCHECK CONSTRAINT [FK_Permissions_MenuStructure]
GO

ALTER TABLE [dbo].[PictureBinary]  WITH NOCHECK ADD  CONSTRAINT [FK_PictureBinary_Picture_PictureId] FOREIGN KEY([PictureId])
REFERENCES [dbo].[Picture] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PictureBinary] NOCHECK CONSTRAINT [FK_PictureBinary_Picture_PictureId]
GO

ALTER TABLE [dbo].[Poll]  WITH NOCHECK ADD  CONSTRAINT [FK_Poll_Language_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Poll] NOCHECK CONSTRAINT [FK_Poll_Language_LanguageId]
GO

ALTER TABLE [dbo].[PollAnswer]  WITH NOCHECK ADD  CONSTRAINT [FK_PollAnswer_Poll_PollId] FOREIGN KEY([PollId])
REFERENCES [dbo].[Poll] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PollAnswer] NOCHECK CONSTRAINT [FK_PollAnswer_Poll_PollId]
GO

ALTER TABLE [dbo].[PollVotingRecord]  WITH NOCHECK ADD  CONSTRAINT [FK_PollVotingRecord_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PollVotingRecord] NOCHECK CONSTRAINT [FK_PollVotingRecord_Customer_CustomerId]
GO
ALTER TABLE [dbo].[PollVotingRecord]  WITH NOCHECK ADD  CONSTRAINT [FK_PollVotingRecord_PollAnswer_PollAnswerId] FOREIGN KEY([PollAnswerId])
REFERENCES [dbo].[PollAnswer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PollVotingRecord] NOCHECK CONSTRAINT [FK_PollVotingRecord_PollAnswer_PollAnswerId]
GO

ALTER TABLE [dbo].[PredefinedProductAttributeValue]  WITH NOCHECK ADD  CONSTRAINT [FK_PredefinedProductAttributeValue_ProductAttribute_ProductAttributeId] FOREIGN KEY([ProductAttributeId])
REFERENCES [dbo].[ProductAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PredefinedProductAttributeValue] NOCHECK CONSTRAINT [FK_PredefinedProductAttributeValue_ProductAttribute_ProductAttributeId]
GO

ALTER TABLE [dbo].[Product_Category_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Category_Mapping_Category_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Category_Mapping] NOCHECK CONSTRAINT [FK_Product_Category_Mapping_Category_CategoryId]
GO
ALTER TABLE [dbo].[Product_Category_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Category_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Category_Mapping] NOCHECK CONSTRAINT [FK_Product_Category_Mapping_Product_ProductId]
GO

ALTER TABLE [dbo].[Product_Manufacturer_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Manufacturer_Mapping_Manufacturer_ManufacturerId] FOREIGN KEY([ManufacturerId])
REFERENCES [dbo].[Manufacturer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Manufacturer_Mapping] NOCHECK CONSTRAINT [FK_Product_Manufacturer_Mapping_Manufacturer_ManufacturerId]
GO
ALTER TABLE [dbo].[Product_Manufacturer_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Manufacturer_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Manufacturer_Mapping] NOCHECK CONSTRAINT [FK_Product_Manufacturer_Mapping_Product_ProductId]
GO

ALTER TABLE [dbo].[Product_Picture_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Picture_Mapping_Picture_PictureId] FOREIGN KEY([PictureId])
REFERENCES [dbo].[Picture] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Picture_Mapping] NOCHECK CONSTRAINT [FK_Product_Picture_Mapping_Picture_PictureId]
GO
ALTER TABLE [dbo].[Product_Picture_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_Picture_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Picture_Mapping] NOCHECK CONSTRAINT [FK_Product_Picture_Mapping_Product_ProductId]
GO

ALTER TABLE [dbo].[Product_ProductAttribute_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_ProductAttribute_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_ProductAttribute_Mapping] NOCHECK CONSTRAINT [FK_Product_ProductAttribute_Mapping_Product_ProductId]
GO
ALTER TABLE [dbo].[Product_ProductAttribute_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_ProductAttribute_Mapping_ProductAttribute_ProductAttributeId] FOREIGN KEY([ProductAttributeId])
REFERENCES [dbo].[ProductAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_ProductAttribute_Mapping] NOCHECK CONSTRAINT [FK_Product_ProductAttribute_Mapping_ProductAttribute_ProductAttributeId]
GO

ALTER TABLE [dbo].[Product_ProductTag_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_ProductTag_Mapping_Product_Product_Id] FOREIGN KEY([Product_Id])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_ProductTag_Mapping] NOCHECK CONSTRAINT [FK_Product_ProductTag_Mapping_Product_Product_Id]
GO
ALTER TABLE [dbo].[Product_ProductTag_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_ProductTag_Mapping_ProductTag_ProductTag_Id] FOREIGN KEY([ProductTag_Id])
REFERENCES [dbo].[ProductTag] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_ProductTag_Mapping] NOCHECK CONSTRAINT [FK_Product_ProductTag_Mapping_ProductTag_ProductTag_Id]
GO

ALTER TABLE [dbo].[Product_SpecificationAttribute_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_SpecificationAttribute_Mapping_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_SpecificationAttribute_Mapping] NOCHECK CONSTRAINT [FK_Product_SpecificationAttribute_Mapping_Product_ProductId]
GO
ALTER TABLE [dbo].[Product_SpecificationAttribute_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_Product_SpecificationAttribute_Mapping_SpecificationAttributeOption_SpecificationAttributeOptionId] FOREIGN KEY([SpecificationAttributeOptionId])
REFERENCES [dbo].[SpecificationAttributeOption] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_SpecificationAttribute_Mapping] NOCHECK CONSTRAINT [FK_Product_SpecificationAttribute_Mapping_SpecificationAttributeOption_SpecificationAttributeOptionId]
GO

ALTER TABLE [dbo].[ProductAttributeCombination]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductAttributeCombination_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductAttributeCombination] NOCHECK CONSTRAINT [FK_ProductAttributeCombination_Product_ProductId]
GO

ALTER TABLE [dbo].[ProductAttributeValue]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductAttributeValue_Product_ProductAttribute_Mapping_ProductAttributeMappingId] FOREIGN KEY([ProductAttributeMappingId])
REFERENCES [dbo].[Product_ProductAttribute_Mapping] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductAttributeValue] NOCHECK CONSTRAINT [FK_ProductAttributeValue_Product_ProductAttribute_Mapping_ProductAttributeMappingId]
GO

ALTER TABLE [dbo].[ProductReview]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview] NOCHECK CONSTRAINT [FK_ProductReview_Customer_CustomerId]
GO
ALTER TABLE [dbo].[ProductReview]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview] NOCHECK CONSTRAINT [FK_ProductReview_Product_ProductId]
GO
ALTER TABLE [dbo].[ProductReview]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview] NOCHECK CONSTRAINT [FK_ProductReview_Store_StoreId]
GO

ALTER TABLE [dbo].[ProductReview_ReviewType_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_ReviewType_Mapping_ProductReview_ProductReviewId] FOREIGN KEY([ProductReviewId])
REFERENCES [dbo].[ProductReview] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview_ReviewType_Mapping] NOCHECK CONSTRAINT [FK_ProductReview_ReviewType_Mapping_ProductReview_ProductReviewId]
GO
ALTER TABLE [dbo].[ProductReview_ReviewType_Mapping]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReview_ReviewType_Mapping_ReviewType_ReviewTypeId] FOREIGN KEY([ReviewTypeId])
REFERENCES [dbo].[ReviewType] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReview_ReviewType_Mapping] NOCHECK CONSTRAINT [FK_ProductReview_ReviewType_Mapping_ReviewType_ReviewTypeId]
GO

ALTER TABLE [dbo].[ProductReviewHelpfulness]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductReviewHelpfulness_ProductReview_ProductReviewId] FOREIGN KEY([ProductReviewId])
REFERENCES [dbo].[ProductReview] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReviewHelpfulness] NOCHECK CONSTRAINT [FK_ProductReviewHelpfulness_ProductReview_ProductReviewId]
GO

ALTER TABLE [dbo].[ProductWarehouseInventory]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductWarehouseInventory_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductWarehouseInventory] NOCHECK CONSTRAINT [FK_ProductWarehouseInventory_Product_ProductId]
GO
ALTER TABLE [dbo].[ProductWarehouseInventory]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductWarehouseInventory_Warehouse_WarehouseId] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[Warehouse] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductWarehouseInventory] NOCHECK CONSTRAINT [FK_ProductWarehouseInventory_Warehouse_WarehouseId]
GO

ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD FOREIGN KEY([ProjectCFOId])
REFERENCES [dbo].[Centers] ([Id])
GO
ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD FOREIGN KEY([ProjectTypeId])
REFERENCES [dbo].[ProjectTypes] ([Id])
GO
ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD  CONSTRAINT [FK_Projects_Organization] FOREIGN KEY([Shipper])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Projects] NOCHECK CONSTRAINT [FK_Projects_Organization]
GO
ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD  CONSTRAINT [FK_Projects_Organization1] FOREIGN KEY([Consignee])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Projects] NOCHECK CONSTRAINT [FK_Projects_Organization1]
GO
ALTER TABLE [dbo].[Projects]  WITH NOCHECK ADD  CONSTRAINT [FK_Projects_Projects] FOREIGN KEY([Id])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[Projects] NOCHECK CONSTRAINT [FK_Projects_Projects]
GO

ALTER TABLE [dbo].[QueuedEmail]  WITH NOCHECK ADD  CONSTRAINT [FK_QueuedEmail_EmailAccount_EmailAccountId] FOREIGN KEY([EmailAccountId])
REFERENCES [dbo].[EmailAccount] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QueuedEmail] NOCHECK CONSTRAINT [FK_QueuedEmail_EmailAccount_EmailAccountId]
GO

ALTER TABLE [dbo].[RecurringPayment]  WITH NOCHECK ADD  CONSTRAINT [FK_RecurringPayment_Order_InitialOrderId] FOREIGN KEY([InitialOrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[RecurringPayment] NOCHECK CONSTRAINT [FK_RecurringPayment_Order_InitialOrderId]
GO

ALTER TABLE [dbo].[RecurringPaymentHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_RecurringPaymentHistory_RecurringPayment_RecurringPaymentId] FOREIGN KEY([RecurringPaymentId])
REFERENCES [dbo].[RecurringPayment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecurringPaymentHistory] NOCHECK CONSTRAINT [FK_RecurringPaymentHistory_RecurringPayment_RecurringPaymentId]
GO

ALTER TABLE [dbo].[RestsSnapshot]  WITH NOCHECK ADD  CONSTRAINT [FK_RestsSnapshot_LogisticSnapshots] FOREIGN KEY([id_snapshot])
REFERENCES [dbo].[LogisticSnapshots] ([id_spanshot])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RestsSnapshot] NOCHECK CONSTRAINT [FK_RestsSnapshot_LogisticSnapshots]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Внутренний идентификатор, который однозначно идентифицирует партию' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'InnerPartyKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Производитель' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Producer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Наименование номенклатуры товара' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Product'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Артикул (шифр) товара' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Shifr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Информация о чертеже товара/изделия' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Figure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ед измерения' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Measure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Вес единицы товара в кг.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тип товара (товар или услуга)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'pType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Группа товара (группа номенклатуры)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'pGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Грузополучатель (плановый)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'pRecieverPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Грузополучатель (фактический) ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'pRecieverFact'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Группа грузополучателя (клиент)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'RecieverGroupPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Внутренний номер заказа ПП ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'InnerOrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Заказчик изготовления продукции' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'OrderedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер заказа покупателя в базе ПП' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'OrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество товара в партии на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'QuantityBefore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для конечного потребителя  на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PE_Before'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для первого покупателя на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PF_Before'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая полная единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PCP_Before'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая переменная единицы товара  на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PCPC_Before'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая полная единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'FCP_Before'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая переменная единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'FCPC_Before'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Балансовая стоимость единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BP_Before'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для конечного потребителя на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PE_After'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для первого покупателя на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PF_After'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая полная единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PCP_After'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая переменная единицы товара  на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PCPC_After'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая полная единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'FCP_After'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая переменная единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'FCPC_After'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Балансовая стоимость единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BP_After'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество товара в партии на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'QuantityAfter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Наименование склада' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Storage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Расположение склада город' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'StorageCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Расположение склада страна' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'StorageCountry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Информация о центре финансовой ответственности' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Сenter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Балансодержатель' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BalanceKeeper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Признак ограничения к отгрузке' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'ReadyForSaleStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Статус резерва' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'ReserveStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата производства' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'ProduceDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата реконсервации' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'ReconcervationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество дней фактического хранения партии на складе' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'TermOnStorage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Документ прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Валюта балансовая' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BalanceCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Курс валюты к  грн' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'CurrencyIndexToUAH'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Штрихкод номенклатуры' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BacodeProduct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Штрихкод партии' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BacodeConsignment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Штрихкоды все' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BacodesAll'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Артикул МДМ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Shifr_MDM'
GO


ALTER TABLE [dbo].[ReturnRequest]  WITH NOCHECK ADD  CONSTRAINT [FK_ReturnRequest_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReturnRequest] NOCHECK CONSTRAINT [FK_ReturnRequest_Customer_CustomerId]
GO

ALTER TABLE [dbo].[RewardPointsHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_RewardPointsHistory_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RewardPointsHistory] NOCHECK CONSTRAINT [FK_RewardPointsHistory_Customer_CustomerId]
GO

ALTER TABLE [dbo].[RoleGroupsRole]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.RoleGroupsRole_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleGroupsRole] NOCHECK CONSTRAINT [FK_dbo.RoleGroupsRole_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[RoleGroupsRole]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.RoleGroupsRole_dbo.RoleGroups_RoleGroupsId] FOREIGN KEY([RoleGroupsId])
REFERENCES [dbo].[RoleGroups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleGroupsRole] NOCHECK CONSTRAINT [FK_dbo.RoleGroupsRole_dbo.RoleGroups_RoleGroupsId]
GO

ALTER TABLE [dbo].[RoutePoints]  WITH NOCHECK ADD  CONSTRAINT [FK_RoutePoints_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[RoutePoints] NOCHECK CONSTRAINT [FK_RoutePoints_Organization]
GO
ALTER TABLE [dbo].[RoutePoints]  WITH NOCHECK ADD  CONSTRAINT [FK_RoutePoints_RoutePointType] FOREIGN KEY([RoutePointTypeId])
REFERENCES [dbo].[RoutePointType] ([Id])
GO
ALTER TABLE [dbo].[RoutePoints] NOCHECK CONSTRAINT [FK_RoutePoints_RoutePointType]
GO
ALTER TABLE [dbo].[RoutePoints]  WITH NOCHECK ADD  CONSTRAINT [FK_RoutePoints_Routes] FOREIGN KEY([RoutePointId])
REFERENCES [dbo].[Routes] ([Id])
GO
ALTER TABLE [dbo].[RoutePoints] NOCHECK CONSTRAINT [FK_RoutePoints_Routes]
GO

ALTER TABLE [dbo].[Routes]  WITH NOCHECK ADD  CONSTRAINT [FK_Routes_FromOrganization] FOREIGN KEY([OrgFromId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Routes] NOCHECK CONSTRAINT [FK_Routes_FromOrganization]
GO
ALTER TABLE [dbo].[Routes]  WITH NOCHECK ADD  CONSTRAINT [FK_Routes_ToOrganization] FOREIGN KEY([OrgToId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Routes] NOCHECK CONSTRAINT [FK_Routes_ToOrganization]
GO

ALTER TABLE [dbo].[Shipment]  WITH NOCHECK ADD  CONSTRAINT [FK_Shipment_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Shipment] NOCHECK CONSTRAINT [FK_Shipment_Order_OrderId]
GO

ALTER TABLE [dbo].[ShipmentItem]  WITH NOCHECK ADD  CONSTRAINT [FK_ShipmentItem_Shipment_ShipmentId] FOREIGN KEY([ShipmentId])
REFERENCES [dbo].[Shipment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShipmentItem] NOCHECK CONSTRAINT [FK_ShipmentItem_Shipment_ShipmentId]
GO

ALTER TABLE [dbo].[ShippingMethodRestrictions]  WITH NOCHECK ADD  CONSTRAINT [FK_ShippingMethodRestrictions_Country_Country_Id] FOREIGN KEY([Country_Id])
REFERENCES [dbo].[Country] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShippingMethodRestrictions] NOCHECK CONSTRAINT [FK_ShippingMethodRestrictions_Country_Country_Id]
GO
ALTER TABLE [dbo].[ShippingMethodRestrictions]  WITH NOCHECK ADD  CONSTRAINT [FK_ShippingMethodRestrictions_ShippingMethod_ShippingMethod_Id] FOREIGN KEY([ShippingMethod_Id])
REFERENCES [dbo].[ShippingMethod] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShippingMethodRestrictions] NOCHECK CONSTRAINT [FK_ShippingMethodRestrictions_ShippingMethod_ShippingMethod_Id]
GO

ALTER TABLE [dbo].[ShoppingCartItem]  WITH NOCHECK ADD  CONSTRAINT [FK_ShoppingCartItem_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoppingCartItem] NOCHECK CONSTRAINT [FK_ShoppingCartItem_Customer_CustomerId]
GO
ALTER TABLE [dbo].[ShoppingCartItem]  WITH NOCHECK ADD  CONSTRAINT [FK_ShoppingCartItem_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoppingCartItem] NOCHECK CONSTRAINT [FK_ShoppingCartItem_Product_ProductId]
GO

ALTER TABLE [dbo].[SpecificationAttributeOption]  WITH NOCHECK ADD  CONSTRAINT [FK_SpecificationAttributeOption_SpecificationAttribute_SpecificationAttributeId] FOREIGN KEY([SpecificationAttributeId])
REFERENCES [dbo].[SpecificationAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SpecificationAttributeOption] NOCHECK CONSTRAINT [FK_SpecificationAttributeOption_SpecificationAttribute_SpecificationAttributeId]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 - фрахт, 1 - фиксированный' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SpecificationTypes', @level2type=N'COLUMN',@level2name=N'SpecificationType'
GO

ALTER TABLE [dbo].[StateProvince]  WITH NOCHECK ADD  CONSTRAINT [FK_StateProvince_Country_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StateProvince] NOCHECK CONSTRAINT [FK_StateProvince_Country_CountryId]
GO

ALTER TABLE [dbo].[StockQuantityHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_StockQuantityHistory_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockQuantityHistory] NOCHECK CONSTRAINT [FK_StockQuantityHistory_Product_ProductId]
GO

ALTER TABLE [dbo].[StoreMapping]  WITH NOCHECK ADD  CONSTRAINT [FK_StoreMapping_Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreMapping] NOCHECK CONSTRAINT [FK_StoreMapping_Store_StoreId]
GO

ALTER TABLE [dbo].[TierPrice]  WITH NOCHECK ADD  CONSTRAINT [FK_TierPrice_CustomerRole_CustomerRoleId] FOREIGN KEY([CustomerRoleId])
REFERENCES [dbo].[CustomerRole] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TierPrice] NOCHECK CONSTRAINT [FK_TierPrice_CustomerRole_CustomerRoleId]
GO
ALTER TABLE [dbo].[TierPrice]  WITH NOCHECK ADD  CONSTRAINT [FK_TierPrice_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TierPrice] NOCHECK CONSTRAINT [FK_TierPrice_Product_ProductId]
GO

ALTER TABLE [dbo].[UserMessages]  WITH NOCHECK ADD  CONSTRAINT [FK_PK_UserMessages_AspNetUsers] FOREIGN KEY([CreatedFromUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserMessages] NOCHECK CONSTRAINT [FK_PK_UserMessages_AspNetUsers]
GO
ALTER TABLE [dbo].[UserMessages]  WITH NOCHECK ADD  CONSTRAINT [FK_PK_UserMessages_AspNetUsers2] FOREIGN KEY([CreatedToUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserMessages] NOCHECK CONSTRAINT [FK_PK_UserMessages_AspNetUsers2]
GO

ALTER TABLE [dbo].[UserProfile]  WITH NOCHECK ADD  CONSTRAINT [FK_UserProfile_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserProfile] NOCHECK CONSTRAINT [FK_UserProfile_AspNetUsers]
GO
ALTER TABLE [dbo].[UserProfile]  WITH NOCHECK ADD  CONSTRAINT [FK_UserProfile_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Сode])
GO
ALTER TABLE [dbo].[UserProfile] NOCHECK CONSTRAINT [FK_UserProfile_Countries]
GO

ALTER TABLE [dbo].[UserSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_User] FOREIGN KEY([userId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserSettings] NOCHECK CONSTRAINT [FK_User]
GO

ALTER TABLE [dbo].[VendorNote]  WITH NOCHECK ADD  CONSTRAINT [FK_VendorNote_Vendor_VendorId] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VendorNote] NOCHECK CONSTRAINT [FK_VendorNote_Vendor_VendorId]
GO




