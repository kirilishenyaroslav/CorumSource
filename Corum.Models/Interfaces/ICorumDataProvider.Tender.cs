using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Admin;
using Corum.Models.Tender;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels.Tender;
using System;
using System.Web;
using Corum.Models.ViewModels.OrderConcurs;

namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        List<TenderServices> GetTenderServices();
        List<SpecificationNames> GetSpecificationNames();
        List<BalanceKeepers> GetBalanceKeepers();
        OrderTruckTransport GetOrderTruckTransport(long orderId);
        List<Countries> GetCountries();
        void AddNewDataTender(RegisterTenders model);
        bool IsRegisterTendersExist(long orderId, bool isMultipleTenders);
        List<RegisterTenders> GetRegisterTenders();
        List<RegisterTenders> GetRegisterTendersOfOrder(long orderId);
        Dictionary<int, string> GetStatusTenders();
        void UpdateRegisterTenders(int tenderNumber, string resultsTender);
        void RemainingTime(Dictionary<string,Time> time);
        void UpdateStatusRegisterTender(int tenderNumber, int process, DateTime dateUpdateStatus, RequestJSONDeserializedToModel resultDeserializedClass);
        void UpdateTimeRemainingTime(RequestJSONDeserializedToModel myDeserializedClass, int numberTender);
        string UpdateRegistersRemainingTime(int tenderNumber);
        UpdateRegisterStatusTender UpdateCLStatusTenderOrder(RequestJSONDeserializedToModel myDeserializedClass, int numberTender);
        Dictionary<string, int> ShareTendersFromRegistyTenders();
        List<ContrAgentModel> GetAgentModels(List<RequestJSONContragentMainData> listRequestJSONContragent, RequestJSONContragentModel myDeserializedClassContragent);
        ContrAgentModel GetWinnerContragent(List<ContrAgentModel> listAllContragents, int SupplierIdWinnerContragent);
        void UpdateDataRegisterContragents(Dictionary<long, List<RegisterTenderContragent>> regisContragents);
        void FormInitMessageToContragents(ref InfoToContragentsAfterChange listInfoToCont);
        bool FormMessageToSendContragents(InfoToContragentsAfterChange listInfoToCont, bool toogle);
        List<RegisterMessageToContragents> GetListFormUuidToContragents(long orderId);
        bool CheckFormUuid(Guid formUuid);
        List<RegisterFormFromContragents> GetRegisterFormFromContragents(Guid formUuid);
        bool SetRegisterFormFromContragent(List<HttpPostedFileBase> listFiles, Dictionary<string, string> dic);
        void SetRegisterMessageData(int tenderNumber, SpecificationListViewModel mod, long orderId, Guid formUuid, int tenderTureNumber);
        void SetEdrpouInOrdCompList();
        SpecificationListViewModel GetSpesificationData(long? edrpou);
        void UpdateExecuterNotesOnOrdersBase(string ExecuterNotes, int orderId);
        string GetExecuterNotes(int orderId);
    }
}
