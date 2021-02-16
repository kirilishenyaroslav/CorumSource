using System.Collections.Generic;
using System.Linq;
using Corum.DAL.Mappings;
using Corum.Models;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Admin;
using Corum.Models.Tender;
using System;
using Corum.DAL.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using Corum.DAL.Entity;
using System.Globalization;


namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {

        public List<Corum.Models.Tender.TenderServices> GetTenderServices()
        {
            List<Corum.Models.Tender.TenderServices> services = new List<Models.Tender.TenderServices>();
            var tenderList = db.TenderServices.ToList();
            foreach (var item in tenderList)
            {
                Corum.Models.Tender.TenderServices tender = new Models.Tender.TenderServices();
                tender.Id = item.Id;
                tender.industryName = item.industryName;
                tender.industryId = item.industryId;

                services.Add(tender);
            }
            return services;
        }
    }
}
