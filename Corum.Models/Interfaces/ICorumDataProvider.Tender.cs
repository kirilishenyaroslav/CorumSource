using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Admin;
using Corum.Models.Tender;
using System;

namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        List<TenderServices> GetTenderServices();
        List<SpecificationNames> GetSpecificationNames();
    }
}
