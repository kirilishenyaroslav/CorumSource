using System;
using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Cars;
using Corum.Models.ViewModels.Customers;


namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        bool IsUserOrderLPRPerson(string userId, int orderTypeId);

        void AddDefaultObservers(long orderId, int orderTypeId);

        IQueryable<ProjectTypeViewModel> getProjectTYpes();
        List<OrganizationViewModel> GetAllOrganizations(string searchTerm, int pageSize, int pageNum);

        IQueryable<OrderUsedCarViewModel> getOrderCarsInfo(long OrderId);
        OrderUsedCarViewModel getUsedCarInfo(int Id);

        IQueryable<OrderNotificationViewModel> getNotifications(long OrderId);
        IQueryable<OrderNotificationTypesViewModel> getNotificationTypes();

        IQueryable<OrderClientBalanceKeeperViewModel> getBalanceKeepers(string userId);
        IQueryable<OrderClientCFOViewModel> getCenters(string userId);

        IQueryable<OrderClientsViewModel>  getClients(string userId, string searchString="");
        IQueryable<OrderClientsViewModel> getClientsInPipeline(string userId, string searchString="");

        IQueryable<OrderBaseViewModel> getOrders(bool IsTransport,
                                                        string userId,
                                                        bool isAdmin,
                                                        bool UseStatusesFilter,
                                                        string FilterStatusId,
                                                        bool UseOrderCreatorFilter,
                                                        string FilterOrderCreatorId,
                                                        bool UseOrderTypeFilter,
                                                        string FilterOrderTypeId,
                                                        bool UseOrderClientFilter,
                                                        string FilterOrderClientId,
                                                        bool UseOrderPriorityFilter,
                                                        int FilterOrderPriority,
                                                        bool UseOrderDateFilter,        
                                                        DateTime FilterOrderDateBeg,
                                                        DateTime FilterOrderDateEnd,
                                                        bool UseOrderExDateFilter,
                                                        DateTime FilterOrderExDateBeg,
                                                        DateTime FilterOrderExDateEnd,
                                                        bool UseOrderEndDateFilter,
                                                        DateTime FilterOrderEndDateBeg,
                                                        DateTime FilterOrderEndDateEnd,
                                                        string FilterOrderExecuterId,
                                                        bool UseOrderExecuterFilter,
                                                        bool UseFinalStatusFilter,
                                                        bool FilterFinalStatus,
                                                        bool UseOrderProjectFilter,
                                                        string FilterOrderProjectId,
                                                        bool UseOrderPayerFilter,
                                                        string FilterOrderPayerId,
                                                        string FilterOrderOrgFromId,
                                                        bool UseOrderOrgFromFilter,
                                                        string FilterOrderOrgToId,
                                                        bool UseOrderOrgToFilter);

       
        IQueryable<OrderTypeViewModel> getAvailableOrderTypes(string userId, bool? IsTransport);
        IQueryable<OrderStatusViewModel> getAvailableOrderStatuses(string userId);
        IQueryable<OrderStatusViewModel> getAvailableOrderStatusesInPipeline(string userId, bool isAdmin, int currentStatus, int orderTypeId);
        IQueryable<OrderStatusViewModel> getPreviousOrderStatusesInPipeline(string userId, bool isAdmin, int currentStatus, int orderTypeId, long Id);
        IQueryable<OrderStatusHistoryViewModel> getOrderStatusHistory(string userId, long OrderId);
        IQueryable<OrderAttachmentViewModel> getOrderAttachments(string userId, long OrderId);
        IQueryable<OrderDocTypeViewModel> getAvailabbleDocTypes(string userId);
        IQueryable<OrderObserverViewModel> getOrderObservers(long Id);
        IQueryable<OrderPipelineStepViewModel> getPipelineSteps(string userId, int OrderTypeId);
        

        List<OrderStatusViewModel> GetStatuses(string searchTerm, int pageSize, int pageNum, bool isTransport);
        int GetStatusesCount(string searchTerm, bool isTransport);
        IQueryable<OrderStatusViewModel> GetStatusesBySearchString(string searchTerm, bool isTransport);

        List<UserViewModel> GetOrderCreators(string searchTerm, int pageSize, int pageNum, bool isTransport);
        int GetOrderCreatorsCount(string searchTerm, bool isTransport);
        IQueryable<UserViewModel> GetOrderCreatorsBySearchString(string searchTerm, bool isTransport);

        List<OrderTypeViewModel> GetOrderTypes(string searchTerm, int pageSize, int pageNum, bool isTransport);
        int GetOrderTypesCount(string searchTerm, bool isTransport);
        IQueryable<OrderTypeViewModel> GetOrderTypesBySearchString(string searchTerm, bool isTransport);

        List<OrderClientsViewModel> GetOrderClients(string searchTerm, int pageSize, int pageNum, bool isTransport);
        int GetOrderClientsCount(string searchTerm, bool isTransport);
        IQueryable<OrderClientsViewModel> GetOrderClientsBySearchString(string searchTerm, bool isTransport);

        List<OrderCountriesViewModel> GetOrderCountries(string searchTerm, int pageSize, int pageNum);
        int GetOrderCountriesCount(string searchTerm);
        IQueryable<OrderCountriesViewModel> GetOrderCountriesBySearchString(string searchTerm);

        List<UserViewModel> GetOrderExecutorsEx(int orderTypeId);

        List<UserViewModel> GetOrderExecutors(string searchTerm, int pageSize, int pageNum, bool isTransport);
        int GetOrderExecutorsCount(string searchTerm, bool isTransport);
        IQueryable<UserViewModel> GetOrderExecutorsBySearchString(string searchTerm, bool isTransport);

        IQueryable<UserViewModel> GetReceiversBySearchString(string searchTerm, long? Id);
        List<UserViewModel> GetReceivers(string searchTerm, int pageSize, int pageNum, long? Id);
        int GetReceiverCount(string searchTerm, long? Id);

        IQueryable<TruckTypeViewModel> GetTruckTypesBySearchString(string searchTerm);
        List<TruckTypeViewModel> TruckTypes(string searchTerm, int pageSize, int pageNum);
        int TruckTypesCount(string searchTerm);

        IQueryable<VehicleViewModel> GetVehicleTypesBySearchString(string searchTerm);
        List<VehicleViewModel> VehicleTypes(string searchTerm, int pageSize, int pageNum);
        int VehicleTypesCount(string searchTerm);

        IQueryable<LoadingTypeViewModel> GetLoadingTypesBySearchString(string searchTerm);
        List<LoadingTypeViewModel> LoadingTypes(string searchTerm, int pageSize, int pageNum);
        int LoadingTypesCount(string searchTerm);

        IQueryable<UnloadingTypeViewModel> GetUnloadingTypesBySearchString(string searchTerm);
        List<UnloadingTypeViewModel> UnloadingTypes(string searchTerm, int pageSize, int pageNum);
        int UnloadingTypesCount(string searchTerm);

        IQueryable<OrderClientBalanceKeeperViewModel> GetPayersBySearchString(string searchTerm, string userId);
        List<OrderClientBalanceKeeperViewModel> Payers(string searchTerm, int pageSize, int pageNum, string userId);
        int PayersCount(string searchTerm, string userId);

        IQueryable<OrderClientsViewModel> GetClientsBySearchString(string searchTerm, string userId);
        List<OrderClientsViewModel> Clients(string searchTerm, int pageSize, int pageNum, string userId);
        int ClientsCount(string searchTerm, string userId);

        string GetAcceptDate(long orderId);
        string GetStartExecuteDate(long orderId);
        string GetFromInfo(long orderId, int orderType);
        string GetToInfo(long orderId, int orderType);
        string GetProjectsInfo(long orderId, int orderType);
        DateTime GetStartDate(long orderId, int orderType);
        DateTime GetFinishDate(long orderId, int orderType);
        string GetPassInfo(long orderId, int orderType);
        string GetAutoCount(long orderId, int orderType);

        OrderBaseViewModel getOrder(long Id);
        void getPassTrasportOrderData(ref OrdersPassTransportViewModel order);
        void getTruckTrasportOrderData(ref OrdersTruckTransportViewModel order);
        OrderAttachmentViewModel getAttachment(long Id);
        OrderClientsViewModel getClient(long Id);
        OrderStatusViewModel getStatus(int Id);
        OrderPipelineStepViewModel getStep(int Id);
        OrderClientBalanceKeeperViewModel getPayer(long Id);

        OrderCountriesViewModel getDefaultCountry();
        OrderCountriesViewModel getCountryById(int Id);

        bool UpdatePipelineStep(OrderPipelineStepViewModel model);
        bool NewPipelineStep(OrderPipelineStepViewModel model);
        bool NewAttachment(OrderAttachmentViewModel model);
        bool NewObserver(OrderObserverViewModel model);
        bool UpdateOrder(OrderBaseViewModel model);
        long NewOrder(OrderBaseViewModel model);
        bool NewClient(OrderClientsViewModel model);
        bool UpdateClient(OrderClientsViewModel model);
        bool SaveNotificationHistory(OrderNotificationViewModel model);
        long NewUsedCar(OrderUsedCarViewModel model);
        bool UpdateUsedCar(OrderUsedCarViewModel model);
        bool UpdateUsedCarAddInfo(OrderUsedCarViewModel model);

        bool DeleteOrder(long id);
        bool DeleteAttachment(long id);
        bool DeleteClient(long id);
        bool DeleteObserver(long id);
        bool DeletePipelineStep(int id);
        bool DeleteUsedCar(long Id);


        void AddStatus(OrderStatusViewModel model);
        void UpdateStatus(OrderStatusViewModel model);
        void RemoveStatus(int Id);
        string GetToInfoForExport(long orderId);        
        string GetFromInfoForExport(long orderId);

        List<AvailableRoles> GetRoles(string searchTerm, int pageSize, int pageNum, string userId);
        int GetRolesCount(string searchTerm, string userId);
        IQueryable<AvailableRoles> GetRolesBySearchString(string searchTerm, string userId);

        void AddOrderFilter(OrderFilterSettingsModel model);
        IQueryable<OrderFilterSettingsModel> GetFilterSettingsBySearchString(string searchTerm, string userId);
        List<OrderFilterSettingsModel> GetFilterSettings(string searchTerm, int pageSize, int pageNum, string userId);
        int GetFilterSettingsCount(string searchTerm, string userId);
        OrderFilterSettingsModel getOrderFilterSettingById(int Id);
        void RemoveOrderFilter(int Id);
        List<OrderFilterSettingsModel> GetFilterSettingsBtn(int groupSize, int fromNumb, string userId);

        int getCountryByUserId(string userId);
        string getCountryNameByUserId(int UserCountryId);

        string getContactName(long OrderId);
        UserProfileViewModel getUserProfileByUserId(string userId);

        IQueryable<OrderObserverViewModel> getOrderAssignObservers(long Id, string[] ObserversList);

        List<OrderBaseViewModel> GetOrders(string searchTerm, int pageSize, int pageNum);
        IQueryable<OrderBaseViewModel> GetOrdersBySearchString(string searchTerm);
        int GetOrderCount(string searchTerm);

        long NewDirUsedCar(OrderUsedCarViewModel model);
        
        IQueryable<CarOwnersAccessViewModel> GetExpeditorNameBySearchString(string searchTerm, long? Id);
        List<CarOwnersAccessViewModel> GetExpeditorName(string searchTerm, int pageSize, int pageNum, long? Id);
        int GetExpeditorNameCount(string searchTerm, long? Id);

        IQueryable<ContractsViewModel> GetContractExpCarInfoBySearchString(string searchTerm, int? urlData1, int? urlData2);
        List<ContractsViewModel> GetContractExpCarrierInfo(string searchTerm, int pageSize, int pageNum, int? urlData1, int? urlData2);
        int GetContractExpCarrierInfoCount(string searchTerm, int? urlData1, int? urlData2);

        IQueryable<CarOwnersAccessViewModel> GetCarrierInfoBySearchString(string searchTerm, long? Id, int? urlData);
        List<CarOwnersAccessViewModel> GetCarrierInfo(string searchTerm, int pageSize, int pageNum, long? Id, int? urlData);
        int GetCarrierInfoCount(string searchTerm, long? Id, int? urlData);

        IQueryable<CarsViewModel> GetCarInfoBySearchString(string searchTerm, long? Id, int? urlData);
        List<CarsViewModel> GetCarInfo(string searchTerm, int pageSize, int pageNum, long? Id, int? urlData);
        int GetCarInfoCount(string searchTerm, long? Id, int? urlData);

        IQueryable<OrderProjectViewModel> GetProjectsBySearchString(string searchTerm);
        List<OrderProjectViewModel> GetProjects(string searchTerm, int pageSize, int pageNum);
        int GetProjectsCount(string searchTerm);

        OrderProjectViewModel GetProjectById(int Id);
        int NewProject(OrderProjectViewModel model);
        void UpdateProject(OrderProjectViewModel model);
        void RemoveProject(int Id);

        IQueryable<OrderProjectViewModel> GetProjects();

        IQueryable<BaseReportViewModel> getBaseReport(string userId,
                                                       bool isAdmin,
                                                       bool UseOrderClientFilter,
                                                       bool UseOrderTypeFilter,
                                                       bool UseTripTypeFilter,
                                                       string FilterOrderClientId,
                                                       string FilterOrderTypeId,
                                                       string FilterTripTypeId,
                                                       DateTime FilterOrderDateBeg,
                                                       DateTime FilterOrderDateEnd,
                                                       DateTime FilterAcceptDateBeg,
                                                       DateTime FilterAcceptDateEnd,
                                                       bool UseOrderDateFilter,
                                                       bool UseAcceptDateFilter,
                                                       bool isPassOrders);

        IQueryable<TruckViewModel> getTruckReport(string userId,
            bool isAdmin,
            bool UseOrderTypeFilter,
            string FilterOrderTypeId,
            DateTime FilterOrderDate,            
            bool UseOrderDateFilter,
            String IdTree,
            ref  List<TruckViewModel> TruckInfo);

        IQueryable<TruckViewModel> getTruckReportData(string userId,
            bool isAdmin,
            bool UseOrderTypeFilter,
            string FilterOrderTypeId,
            DateTime FilterOrderDate,
            bool UseOrderDateFilter, 
            String IdTree);

        IQueryable<string> getFinalPipelineSteps(string userId, int OrderTypeId);

        List<OrderTypeViewModel> GetOrderTruckTypes(string searchTerm, int pageSize, int pageNum);
        int GetOrderTruckTypesCount(string searchTerm);
        IQueryable<OrderTypeViewModel> GetOrderTruckTypesBySearchString(string searchTerm);

        List<RouteTypesViewModel> GetOrderTruckTripTypes(string searchTerm, int pageSize, int pageNum);
        int GetOrderTruckTripTypesCount(string searchTerm);
        IQueryable<RouteTypesViewModel> GetOrderTruckTripTypesBySearchString(string searchTerm);

        List<RouteTypesViewModel> GetOrderPassTripTypes(string searchTerm, int pageSize, int pageNum);
        int GetOrderPassTripTypesCount(string searchTerm);
        IQueryable<RouteTypesViewModel> GetOrderPassTripTypesBySearchString(string searchTerm);

        List<OrderTypeViewModel> GetOrderPassTypes(string searchTerm, int pageSize, int pageNum);
        int GetOrderPassTypesCount(string searchTerm);
        IQueryable<OrderTypeViewModel> GetOrderPassTypesBySearchString(string searchTerm);

        string GetAcceptOnlyDate(long orderId);
        string GetComment(long orderId);

        IQueryable<StatusReportViewModel> getStatusReport(string userId,
                                                      bool isAdmin,
                                                      bool UseOrderClientFilter,
                                                      bool UseOrderTypeFilter,
                                                      bool UseTripTypeFilter,
                                                      string FilterOrderClientId,
                                                      string FilterOrderTypeId,
                                                      string FilterTripTypeId,
                                                      DateTime FilterOrderDateBeg,
                                                      DateTime FilterOrderDateEnd,
                                                      DateTime FilterAcceptDateBeg,
                                                      DateTime FilterAcceptDateEnd,
                                                      bool UseOrderDateFilter,
                                                      bool UseAcceptDateFilter,
                                                      bool isPassOrders);

        IQueryable<OrdersReportViewModel> getOrdersReport(string userId,
                                                      bool isAdmin,
                                                      bool UseOrderClientFilter,
                                                      bool UseOrderTypeFilter,
                                                      bool UseTripTypeFilter,
                                                      string FilterOrderClientId,
                                                      string FilterOrderTypeId,
                                                      string FilterTripTypeId,
                                                      DateTime FilterOrderDateBeg,
                                                      DateTime FilterOrderDateEnd,
                                                      DateTime FilterAcceptDateBeg,
                                                      DateTime FilterAcceptDateEnd,
                                                      bool UseOrderDateFilter,
                                                      bool UseAcceptDateFilter,
                                                      bool isPassOrders);


        IQueryable<FinalReportViewModel> getFinalReport(string userId,
                                                      bool isAdmin,
                                                      bool UseOrderClientFilter,
                                                      bool UseOrderTypeFilter,
                                                      bool UseTripTypeFilter,
                                                      string FilterOrderClientId,
                                                      string FilterOrderTypeId,
                                                      string FilterTripTypeId,
                                                     DateTime FilterOrderDateBeg,
                                                    DateTime FilterOrderDateEnd,
                                                    DateTime FilterAcceptDateBeg,
                                                    DateTime FilterAcceptDateEnd,
                                                    bool UseOrderDateFilter,
                                                    bool UseAcceptDateFilter,
                                                    bool isPassOrders);

        IQueryable<ContractsViewModel> GetContractExpBkInfoBySearchString(string searchTerm, int urlData1, int urlData2);
        List<ContractsViewModel> GetContractExpBkInfo(string searchTerm, int pageSize, int pageNum, int urlData1, int urlData2);
        int GetContractExpBkInfoPECount(string searchTerm, int urlData1, int urlData2);

        List<RouteTypesViewModel> GetTripTypes(string searchTerm, int pageSize, int pageNum);
        int GetTripTypesCount(string searchTerm);
        IQueryable<RouteTypesViewModel> GetTripTypesBySearchString(string searchTerm);
        IQueryable<OrderBaseProjectsViewModel> getOrderProjects(long Id);
        IQueryable<SpecificationTypesViewModel> getOrderSpecification(long OrderId);

        IQueryable<OrderAdditionalRoutePointModel> getLoadPoints(long OrderId, bool IsLoading);
        long NewRoutePoint(OrderAdditionalRoutePointModel model);
        bool DeleteRoutePoint(long Id);
        void UpdateRoutePoint(OrderAdditionalRoutePointModel model);
        void GetCoordinates(string address, out decimal latitude, out decimal longitude);

        List<UserViewModel> GetOrderExecuter(string searchTerm, int pageSize, int pageNum, string UserRoleIdForExecuterData);
        IQueryable<UserViewModel> GetOrderExecuterBySearchString(string searchTerm, string UserRoleIdForExecuterData);
        int GetOrderExecuterCount(string searchTerm, string UserRoleIdForExecuterData);

        IQueryable<OrderUsedCarViewModel> getFactCars(FactCarsFilter factCarsFilter);


        List<OrderUsedCarViewModel> GetOrderId(string searchTerm, int pageSize, int pageNum);
        int GetOrderIdCount(string searchTerm);
        IQueryable<OrderUsedCarViewModel> GetOrderIdBySearchString(string searchTerm);

        void UpdateFactCars(OrderUsedCarViewModel model);

        IQueryable<CarOwnersAccessViewModel> GetExpeditorFilterBySearchString(string searchTerm, long? Id);
        List<CarOwnersAccessViewModel> GetExpeditorFilter(string searchTerm, int pageSize, int pageNum, long? Id);
        int GetExpeditorFilterCount(string searchTerm, long? Id);

        IQueryable<OrderUsedCarViewModel> GetCarModelInfoFilterBySearchString(string searchTerm, long? Id);
        List<OrderUsedCarViewModel> GetCarModelInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id);
        int GetCarModelInfoFilterCount(string searchTerm, long? Id);

       IQueryable<OrderUsedCarViewModel> GetCarRegNumFilterBySearchString(string searchTerm, long? Id);
       List<OrderUsedCarViewModel> GetCarRegNumFilter(string searchTerm, int pageSize, int pageNum, long? Id);
       int GetCarRegNumFilterCount(string searchTerm, long? Id);

       IQueryable<OrderUsedCarViewModel> GetCarCapacityFilterBySearchString(string searchTerm, long? Id);
       List<OrderUsedCarViewModel> GetCarCapacityFilter(string searchTerm, int pageSize, int pageNum, long? Id);
       int GetCarCapacityFilterCount(string searchTerm, long? Id);

       IQueryable<OrderUsedCarViewModel> GetCarDriverInfoFilterBySearchString(string searchTerm, long? Id);
       List<OrderUsedCarViewModel> GetCarDriverInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id);
       int GetCarDriverInfoFilterCount(string searchTerm, long? Id);

       IQueryable<OrderUsedCarViewModel> GetDriverCardInfoFilterBySearchString(string searchTerm, long? Id);
       List<OrderUsedCarViewModel> GetDriverCardInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id);
       int GetDriverCardInfoFilterCount(string searchTerm, long? Id);

       IQueryable<OrderUsedCarViewModel> GetDriverContactInfoFilterBySearchString(string searchTerm, long? Id);
       List<OrderUsedCarViewModel> GetDriverContactInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id);
       int GetDriverContactInfoFilterCount(string searchTerm, long? Id);
        
       IQueryable<OrderUsedCarViewModel> GetCommentsFilterBySearchString(string searchTerm, long? Id);
       List<OrderUsedCarViewModel> GetCommentsFilter(string searchTerm, int pageSize, int pageNum, long? Id);
       int GetCommentsFilterCount(string searchTerm, long? Id);
    
        CarOwnersAccessViewModel getExpeditors(int ExpeditorId);

       IQueryable<ContractsViewModel> GetContractExpBySearchString(string searchTerm, long? Id);
       List<ContractsViewModel> GetContractExpInfo(string searchTerm, int pageSize, int pageNum, long? Id);
       int GetContractExpCount(string searchTerm, long? Id);

        ContractsViewModel getContracts(int ContractsId);

        IQueryable<OrderUsedCarViewModel> GetCarrierInfoFilterBySearchString(string searchTerm, long? Id);
        List<OrderUsedCarViewModel> GetCarrierInfoFilter(string searchTerm, int pageSize, int pageNum, long? Id);
        int GetCarrierInfoFilterCount(string searchTerm, long? Id);

        IQueryable<OrderUsedCarViewModel> GetContractExpBkInfoBySearchString2(string searchTerm, long? Id);
        List<OrderUsedCarViewModel> GetContractExpBkInfo2(string searchTerm, int pageSize, int pageNum, long? Id);
        int GetContractExpBkInfoPECount2(string searchTerm, long? Id);

        List<ProjectTypeViewModel> GetOrderProjects(long orderId, string searchTerm, int pageSize, int pageNum);
        int GetOrderProjectsCount(long orderId, string searchTerm);

        IQueryable<TruckReportViewModel> getTruckReportDetails(string userId,
            bool isAdmin,
            int OrgId,
            DateTime ReportDate, int IdGroudId, string Id);

         IQueryable<TruckViewModel> 
            getTruckReportDetails2(List<TruckViewModel> modelTruckReport2,
            int IdGroudId, string Id);

        string GetShipperAddress(long orderId);

        string GetConsigneeAddress(long orderId);

        String getTruckReportTitle(List<TruckViewModel> TruckInfo,
            int IdGroup, string Id, ref String Address);
    }
}
