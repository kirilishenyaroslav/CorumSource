using System;
using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Cars;
using Corum.Models.ViewModels.OrderConcurs;

namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        IEnumerable<CompetitiveListStepViewModel> getAvialiableStepsForList(long orderId);
        IEnumerable<CompetitiveListStepViewModel> getAvialiableStepsForList(long orderId, int? tenderNumber);
        CompetetiveListStepsInfoViewModel getCurrentStatusForList(long orderId, int? tenderNumber);
        IQueryable<CompetetiveListStepsInfoViewModel> getTimeLineForList(long orderId); 
        CompetetiveListStepsInfoViewModel getCurrentStatusForList(long orderId);
        long SaveListStatus(CompetetiveListStepsInfoViewModel newStatusInfo);

        IQueryable<OrderCompetitiveListViewModel> getOrderCompetitiveList(string userId, long OrderId);
        IQueryable<OrderCompetitiveListViewModel> getOrderCompetitiveList(string userId, long OrderId, int? tenderNumber);
        CompetitiveListViewModel getCompetitiveListInfo(long OrderId);
        CompetitiveListViewModel getCompetitiveListInfo(long OrderId, int? tenderNumber);

        IQueryable<SpecificationListViewModel> GetSpecBySearchString(string searchTerm, long OrderId, bool UseTripTypeFilter, string FilterTripTypeId,
            bool UseSpecificationTypeFilter, string FilterSpecificationTypeId,
            bool UseVehicleTypeFilter, string FilterVehicleTypeId, bool UsePayerFilter, string FilterPayerId, bool UseRouteFilter, int AlgorithmId);
        List<SpecificationListViewModel> GetSpecifications(string searchTerm, int pageSize, int pageNum, long OrderId, bool UseTripTypeFilter, string FilterTripTypeId,
            bool UseSpecificationTypeFilter, string FilterSpecificationTypeId,
            bool UseVehicleTypeFilter, string FilterVehicleTypeId, bool UsePayerFilter, string FilterPayerId, bool UseRouteFilter, int AlgorithmId);
        int GetSpecificationsCount(string searchTerm, long OrderId, bool UseTripTypeFilter, string FilterTripTypeId,
            bool UseSpecificationTypeFilter, string FilterSpecificationTypeId, bool UseVehicleTypeFilter, string FilterVehicleTypeId, bool UsePayerFilter, string FilterPayerId, bool UseRouteFilter, int AlgorithmId);
        long NewSpecification(SpecificationListViewModel model, string userId);
        long NewSpecification(SpecificationListViewModel model, string userId, int? tenderNumber);
        bool DeleteConcurs(long id);
        OrderCompetitiveListViewModel getConcurs(long Id);
        void UpdateConcurs(OrderCompetitiveListViewModel model);

        List<SpecificationTypesViewModel> GetSpecificationTypes(string searchTerm, int pageSize, int pageNum);
        int GetSpecificationTypesCount(string searchTerm);
        IQueryable<SpecificationTypesViewModel> GetSpecificationTypesBySearchString(string searchTerm);

        IQueryable<OrderCompetitiveListViewModel> getConcursHistory(long Id, bool ShowAll, long OrderId,
            DateTime FilterOrderDateBeg,
            DateTime FilterOrderDateEnd);
        IQueryable<ConcursDiscountRateModel> GetConcursDiscountRate();        
        ConcursDiscountRateModel GetConcursDiscountRate(long Id);
        void UpdateDiscountRate(ConcursDiscountRateModel model);
        bool DeleteDiscountRate(long Id);
        void AddDiscountRate(ConcursDiscountRateModel model);
        void CloneConcurs(long Id, string userId);
        DateTime getConcursHistoryHeader(long OrderId);
        void getCurrentStatusForListKL(long orderId, string userId, int? tenderNumber);
        List<CompetetiveListStepsInfoViewModel> listCurrentStatuses(long orderId);
        int? getTenderNumber(long orderId);
        bool IsContainTender(int? tenderNumber, int tenderTureNumber);
        Dictionary<int, IQueryable<OrderCompetitiveListViewModel>> listDisplayValues(long orderId, string userId);
        Dictionary<int, IEnumerable<CompetitiveListStepViewModel>> list_listStatuses(long orderId);
        void ChangeRegisterMessageData(int tenderNumber, long orderId, Guid formUuid, OrderCompetitiveListViewModel model);
        void NewSpecification(SpecificationListViewModel model, string userId, int? tenderNumber, out Guid formUuid);
        int? GetTenderTureNumber(int tenderNumber);
        string[] GetCarsOwner(long? edrpou);
    }
}
