using System.Collections.Generic;
using System.Web.Mvc;
using Corum.Models;
using Corum.Models.ViewModels.OrderConcurs;
using CorumAdminUI.Common;
using Corum.Models.ViewModels.Cars;
using System;
using Corum.Models.ViewModels;
using Corum.Common;
using Corum.Models.Tender;
using Corum.Models.ViewModels.Tender;
using System.Collections;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using CorumAdminUI;
using Newtonsoft.Json;

namespace CorumAdminUI.Controllers
{
    [Authorize]
    public partial class OrderTenderController : CorumBaseController
    {
        static long OrderID;
        [HttpGet]
        public ActionResult TenderReport()
        {
            var model = context.GetRegisterTenders();
            return View(model);
        }

        [HttpPost]
        public ActionResult SendNotificationTender(TenderSumOrderId tenderSumOrder)
        {
            Dictionary<string, string> otherParams = new Dictionary<string, string>();
            TenderForma tenderForma = null;
            try
            {
                TendFormDeserializedJSON tendFormDeserializedJSON = tenderSumOrder.ListItemsModelTenderForm;
                OrderID = Convert.ToInt64(tenderSumOrder.OrderId);

                tenderForma = new TenderForma(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                               tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                               context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList());
                tenderForma.data.InitializedAfterDeserialized();
                otherParams = tenderForma.otherParams;
            }
            catch (Exception e)
            {

            }
            NameValueCollection allAppSettings = ConfigurationManager.AppSettings;
            var appsett = allAppSettings["SwitchToMultipleTenders"];
            if (context.IsRegisterTendersExist(OrderID, Boolean.Parse(appsett)))
            {
                BaseClient clientbase = new BaseClient(allAppSettings["ApiUrl"], allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                var response = new PostApiTender().GetCallAsync(clientbase, tenderForma).Result.ResponseMessage;
                try
                {
                    RequestJSONDeserializedToModel myDeserializedClass = JsonConvert.DeserializeObject<RequestJSONDeserializedToModel>(response);
                    RegisterTenders registerTenders = new RegisterTenders()
                    {
                        OrderId = OrderID,
                        TenderUuid = System.Guid.Parse(myDeserializedClass.data.tenderUuid),
                        tenderNumber = Convert.ToInt32(myDeserializedClass.data.tenderNumber),
                        industryId = myDeserializedClass.data.industryId,
                        industryName = myDeserializedClass.data.industryName,
                        dateStart = myDeserializedClass.data.dateStart,
                        dateEnd = myDeserializedClass.data.dateEnd,
                        mode = (byte)myDeserializedClass.data.mode,
                        process = Convert.ToByte(myDeserializedClass.data.process),
                        stageMode = myDeserializedClass.data.stageMode,
                        stageNumber = (byte)myDeserializedClass.data.stageNumber,
                        subCompanyId = myDeserializedClass.data.subCompanyId,
                        subCompanyName = myDeserializedClass.data.subCompanyName,
                        downloadAddress = otherParams["DOWNLOAD_ADDRESS"],
                        unloadAddress = otherParams["UNLOADING_ADDRESS"],
                        downloadDataRequired = DateTime.Parse(otherParams["DOWNLOADDATEREQUIRED"]),
                        unloadDataRequired = DateTime.Parse(otherParams["UNLOADINGDATEREQUIRED"]),
                        routeOrder = otherParams["ROUTE"],
                        cargoName = otherParams["CARGO_NAME"],
                        lotState = myDeserializedClass.data.lots[0].lotState,
                        resultsTender = null,
                        tenderOwnerPath = myDeserializedClass.data.tenderOwnerPath
                    };
                    context.AddNewDataTender(registerTenders);
                }
                catch (Exception e)
                {

                }

                return Json(response);
            }
            else
            {
                return Json("{\"success\":false,\"isLoadMultiple\":false}");
            }
        }
    }
}