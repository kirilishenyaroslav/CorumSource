using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Admin;
using Corum.Models.Tender;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels.Tender;
using System;

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
        Dictionary<int, string> GetStatusTenders();
        void UpdateRegisterTenders(int tenderNumber, string resultsTender);
        void RemainingTime(Dictionary<string,Time> time);
    }
}
