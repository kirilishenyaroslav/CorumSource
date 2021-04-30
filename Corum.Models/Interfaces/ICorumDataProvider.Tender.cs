using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Admin;
using Corum.Models.Tender;
using Corum.Models.ViewModels.Orders;
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
    }
}
