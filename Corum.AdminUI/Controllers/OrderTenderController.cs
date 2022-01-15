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
using Corum.Models.Interfaces;
using Corum.Models.ViewModels.Orders;
using Corum.RestRenderModels;
using Corum.ReportsUI;
using CorumAdminUI.HangFireTasks;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Corum.Models.ViewModels.Email;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace CorumAdminUI.Controllers
{
    [Authorize]
    public partial class OrderTenderController : CorumBaseController
    {
        static long OrderID;
        protected IReportRenderer report;

        public OrderTenderController()
        {
            report = DependencyResolver.Current.GetService<IReportRenderer>();
        }

        [HttpGet]
        public ActionResult TenderReport(int? orderId, long? dataStart, long? dataEnd, bool? active, string processValue)
        {
            var model = context.GetRegisterTenders();
            ViewBag.processValue = (processValue != null) ? processValue : "null";
            ViewBag.filterShare = (processValue != null) ? "true" : "false";
            ViewBag.shareTendersfromRegistyTenders = context.ShareTendersFromRegistyTenders();
            if (orderId != null && dataStart != null && dataEnd != null && active != false)
            {
                ViewBag.orderId = orderId;
                ViewBag.dataStart = dataStart;
                ViewBag.dataEnd = dataEnd;
                ViewBag.active = "true";
            }
            else
            {
                ViewBag.orderId = "null";
                ViewBag.dataStart = "null";
                ViewBag.dataEnd = "null";
                ViewBag.active = "false";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AsyncInitDataMessageToContragents(InfoToContragentsAfterChange listInfoToCont)
        {
            if (listInfoToCont.listLosersInfoAfterChange != null || listInfoToCont.listWinnersInfoAfterChange != null)
            {
                context.FormInitMessageToContragents(ref listInfoToCont);
            }
            return new JsonpResult
            {
                Data = new { listInfoToCont },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }


        [HttpPost]
        public ActionResult SendMessageToContragents(bool flag, InfoToContragentsAfterChange listInfoToCont)
        {
            bool isAvaliable = false;
            if (listInfoToCont.listLosersInfoAfterChange != null || listInfoToCont.listWinnersInfoAfterChange != null)
            {
                isAvaliable = context.FormMessageToSendContragents(listInfoToCont, flag);
                if (isAvaliable || flag)
                {
                    SendEmailToContragents(listInfoToCont);
                }
            }
            return new JsonpResult
            {
                Data = new { listInfoToCont, isAvaliable, flag },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        private void SendEmailToContragents(InfoToContragentsAfterChange model)
        {
            try
            {
                if (model.listWinnersInfoAfterChange.Count != 0)
                {
                    foreach (var item in model.listWinnersInfoAfterChange)
                    {
                        DataToAndFromContragent instance = new DataToAndFromContragent();
                        context.NewUsedCarExcel(item.formUuid, ref instance);
                        int orderId = (int)item.orderId;
                        List<string> listEmails = new List<string>()
                        {
                        "Litovchenko.Sergey@corum.com",
                        "avtogruz@corum.com"
                        //"corumsourcetest@gmail.com"
                        };
                        if ((item as ListInfoAfterChange).recipientEmailList != null && (item as ListInfoAfterChange).recipientEmailList[0].Contains('@'))
                        {
                            //listEmails.Add((item as ListInfoAfterChange).recipientEmail);
                            foreach(var value in (item as ListInfoAfterChange).recipientEmailList){
                                string addEmail = listEmails.Find(x => x == value);
                                if (addEmail == null) 
                                {
                                    listEmails.Add(value);
                                }
                            }
                        }
                        foreach (var value in listEmails)
                        {
                            SendToWinnerContragentsAsync(item as ListInfoAfterChange, orderId, instance, value).GetAwaiter();
                        }
                    }
                }
                if (model.listLosersInfoAfterChange.Count != 0)
                {
                    foreach (var item in model.listLosersInfoAfterChange)
                    {
                        List<string> listEmails = new List<string>()
                        {
                        "Litovchenko.Sergey@corum.com",
                        "avtogruz@corum.com"
                        //"corumsourcetest@gmail.com"
                        };
                        if ((item as ListInfoAfterChange).recipientEmailList != null && (item as ListInfoAfterChange).recipientEmailList[0].Contains('@'))
                        {
                            //listEmails.Add((item as ListInfoAfterChange).recipientEmail);
                            foreach (var value in (item as ListInfoAfterChange).recipientEmailList)
                            {
                                string addEmail = listEmails.Find(x => x == value);
                                if (addEmail == null)
                                {
                                    listEmails.Add(value);
                                }
                            }
                        }
                        foreach (var value in listEmails)
                        {
                            SendToLoserContragentsAsync(item as ListInfoAfterChange, value).GetAwaiter();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

        }
        private async Task SendToLoserContragentsAsync(ListInfoAfterChange model, string emailRecepient)
        {
            try
            {
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], "Corum Source", Encoding.UTF8);
                MailAddress to = new MailAddress(emailRecepient);
                using (MailMessage mail = new MailMessage(from, to))
                {
                    mail.Subject = $"№{model.tenderNumber}, ({model.orderId}) {model.routeShort}, погрузка {model.dataDownload.ToString("yyyy-MM-dd")}";
                    mail.Body = $"{model.bodyHTML}";
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SmtpServer"];
                    smtp.EnableSsl = false;
                    NetworkCredential networkCredential = new NetworkCredential(from.Address, ConfigurationManager.AppSettings["SmtpAccountPassw"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]);
                    if (true)
                    {
                        await Task.Run(() => { smtp.Send(mail); });
                    }
                }
            }

            catch (Exception e)
            {

            }
        }
        private async Task SendToWinnerContragentsAsync(ListInfoAfterChange model, int orderId, DataToAndFromContragent data, string emailRecepient)
        {
            try
            {
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SmtpAccountLogin"], "Corum Source", Encoding.UTF8);
                MailAddress to = new MailAddress(emailRecepient);
                using (MailMessage mail = new MailMessage(from, to))
                {
                    mail.Subject = $"№{model.tenderNumber}, ({model.orderId}) {model.routeShort}, погрузка {model.dataDownload.ToString("yyyy-MM-dd")}";
                    mail.Body = $"{model.bodyHTML}";
                    mail.IsBodyHtml = true;
                    MemoryStream stream = new MemoryStream();
                    byte[] mBArray = new ExportToExcelController().OrderAsExcelFromContragent(orderId, Request.UserLanguages[0], data);
                    stream = new MemoryStream(mBArray, false);
                    var reportName = "OrderReport " + orderId.ToString() + ".xlsx";
                    var attacment = new Attachment(stream, reportName);
                    attacment.ContentType = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    mail.Attachments.Add(attacment);
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SmtpServer"];
                    smtp.EnableSsl = false;
                    NetworkCredential networkCredential = new NetworkCredential(from.Address, ConfigurationManager.AppSettings["SmtpAccountPassw"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]);
                    if (true)
                    {
                        await Task.Run(() => { smtp.Send(mail); });
                    }
                }
            }

            catch (Exception e)
            {

            }
        }

        [HttpGet]
        public ActionResult CloseTenderOnRegistry(int tenderNumber)
        {
            bool toggle = context.CloseTenderOnRegistry(tenderNumber);

            return new JsonpResult
            {
                Data = new { toggle },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        public ActionResult UpdateStatusOrderTender(int tenderNumber)
        {
            UpdateRegisterStatusTender updateDeserializedClass = new UpdateRegisterStatusTender();
            CompetetiveListStepsInfoViewModel currentStatus = new CompetetiveListStepsInfoViewModel();

            long OrderID = 0;
            try
            {
                NameValueCollection allAppSettings = ConfigurationManager.AppSettings;
                BaseClient clientbase = new BaseClient($"{allAppSettings["ApiGetTenderId"]}{tenderNumber}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                var JSONresponse = new GetApiTendAjax().GetCallAsync(clientbase).Result.ResponseMessage;
                RequestJSONDeserializedToModel myDeserializedClass = JsonConvert.DeserializeObject<RequestJSONDeserializedToModel>(JSONresponse);
                if (myDeserializedClass.success)
                {
                    updateDeserializedClass = context.UpdateCLStatusTenderOrder(myDeserializedClass, tenderNumber);
                    if (Int32.Parse(updateDeserializedClass.process) >= 8)
                    {
                        string cargoWeight = null;
                        // Вытягивание данных о контрагентах из aps tender
                        Dictionary<long, List<RegisterTenderContragent>> contragents = new Dictionary<long, List<RegisterTenderContragent>>();
                        if (myDeserializedClass.data.lots[0].items.Count != 0 && Int32.Parse(myDeserializedClass.data.process) >= 8)
                        {
                            foreach (var items in myDeserializedClass.data.lots[0].items)
                            {
                                BaseClient clientbaseOffer = new BaseClient($"{allAppSettings["ApiUrlGetOffer"]}{items.tenderItemUuid}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                                var JSONresponseContragent = $"{{\"data\":{new GetApiTendAjax().GetCallAsync(clientbaseOffer).Result.ResponseMessage}}}";
                                bool check = JSONresponseContragent.Contains("errorCode");
                                if (!check)
                                {
                                    RequestJSONContragentModel myDeserializedClassContragent = JsonConvert.DeserializeObject<RequestJSONContragentModel>(JSONresponseContragent);
                                    List<RequestJSONContragentMainData> listContAgentModelJSONDesiarized = new List<RequestJSONContragentMainData>();
                                    int SupplierIdWinnerContragent = 0;
                                    foreach (var item in myDeserializedClassContragent.Data)
                                    {
                                        try
                                        {
                                            BaseClient clientbaseSuppContragent = new BaseClient($"{allAppSettings["ApiUrlGetSuppContrAgent"]}{item.SupplierId}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                                            var JSONresponseContAgentModel = new GetApiTendAjax().GetCallAsync(clientbaseSuppContragent).Result.ResponseMessage;
                                            RequestJSONContragentMainData myDeserializedClassContragentModel = JsonConvert.DeserializeObject<RequestJSONContragentMainData>(JSONresponseContAgentModel);
                                            listContAgentModelJSONDesiarized.Add(myDeserializedClassContragentModel);
                                            if ((item.IsWinner != null) ? true : false)
                                            {
                                                SupplierIdWinnerContragent = item.SupplierId;
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                        }
                                    }
                                    List<ContrAgentModel> listContragentModels = new List<ContrAgentModel>();
                                    ContrAgentModel isWinnerContragent = new ContrAgentModel();
                                    listContragentModels = context.GetAgentModels(listContAgentModelJSONDesiarized, myDeserializedClassContragent);   // Список всех контрагентов, которые принимали участие в тендере
                                    isWinnerContragent = context.GetWinnerContragent(listContragentModels, SupplierIdWinnerContragent); // Информация о контрагенте-победителе
                                    List<RegisterTenderContragent> tenderContragents = new List<RegisterTenderContragent>();
                                    foreach (var el in listContragentModels)
                                    {
                                        double costOfCarWithoutNDS = 0d;
                                        double costOfCarWithoutNDSToNull = 0d;
                                        int paymentDelay = 0;
                                        string note = null;
                                        string itemsDescription = items.itemNote;
                                        try
                                        {
                                            foreach (var it in items.propValues)
                                            {
                                                if (it.ВесТ != null)
                                                {
                                                    cargoWeight = it.ВесТ;
                                                    break;
                                                }
                                            }

                                            for (int i = 0; i < el.listCritariaValues.Count; i++)
                                            {
                                                if (costOfCarWithoutNDS == 0)
                                                {
                                                    costOfCarWithoutNDS = (el.listCritariaValues[i].Name.Contains("Абсолютная цена в валюте, без НДС (редукцион)")) ? Double.Parse(el.listCritariaValues[i].Value.ToString()) : 0d;
                                                }
                                                if (costOfCarWithoutNDSToNull == 0)
                                                {
                                                    costOfCarWithoutNDSToNull = (el.listCritariaValues[i].Name.Contains("Цена, приведенная к \"0\", грн без НДС (редукцион)")) ? Double.Parse(el.listCritariaValues[i].Value.ToString()) : 0d;
                                                }
                                                if (paymentDelay == 0)
                                                {
                                                    paymentDelay = (el.listCritariaValues[i].Name.Contains("Оплата 1.Дней (редукцион)")) ? Int32.Parse(el.listCritariaValues[i].Value.ToString()) : 0;
                                                }
                                                if (note == null)
                                                {
                                                    note = (el.listCritariaValues[i].Name.Contains("Примечания")) ? el.listCritariaValues[i].Value.ToString() : null;
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                        }
                                        if (isWinnerContragent != null && el.SupplierId != isWinnerContragent.SupplierId)
                                        {

                                            RegisterTenderContragent registerTenderContragent = new RegisterTenderContragent()
                                            {
                                                OrderId = Int64.Parse(myDeserializedClass.data.tenderExternalN.Remove(myDeserializedClass.data.tenderExternalN.IndexOf('-'))),
                                                tenderNumber = Int32.Parse(myDeserializedClass.data.tenderNumber),
                                                itemExternalNumber = Int64.Parse(items.itemExternalN),
                                                ContragentName = el.OwnershipTypeName + ' ' + el.SupplierName,
                                                ContragentIdAps = el.SupplierId,
                                                DateUpdateInfo = DateTime.Now,
                                                IsWinner = false,
                                                EDRPOUContragent = Int64.Parse(el.SupplierEdrpou),
                                                emailContragent = (el.ContactEmail != null) ? el.ContactEmail : el.SupplierCEOContragent.EMail,
                                                transportUnitsProposed = 1,
                                                acceptedTransportUnits = 0,
                                                costOfCarWithoutNDS = costOfCarWithoutNDS,
                                                costOfCarWithoutNDSToNull = costOfCarWithoutNDSToNull,
                                                PaymentDelay = paymentDelay,
                                                tenderItemUuid = Guid.Parse(items.tenderItemUuid),
                                                nmcName = items.nmcName,
                                                note = note,
                                                itemDescription = itemsDescription,
                                                cargoWeight = cargoWeight
                                            };
                                            tenderContragents.Add(registerTenderContragent);
                                        }
                                        else
                                        {
                                            RegisterTenderContragent registerTenderContragent = new RegisterTenderContragent()
                                            {
                                                OrderId = Int64.Parse(myDeserializedClass.data.tenderExternalN.Remove(myDeserializedClass.data.tenderExternalN.IndexOf('-'))),
                                                tenderNumber = Int32.Parse(myDeserializedClass.data.tenderNumber),
                                                itemExternalNumber = Int64.Parse(items.itemExternalN),
                                                ContragentName = el.OwnershipTypeName + ' ' + el.SupplierName,
                                                ContragentIdAps = el.SupplierId,
                                                DateUpdateInfo = DateTime.Now,
                                                IsWinner = true,
                                                EDRPOUContragent = Int64.Parse(el.SupplierEdrpou),
                                                emailContragent = (el.ContactEmail != null) ? el.ContactEmail : el.SupplierCEOContragent.EMail,
                                                transportUnitsProposed = 1,
                                                acceptedTransportUnits = 1,
                                                costOfCarWithoutNDS = costOfCarWithoutNDS,
                                                costOfCarWithoutNDSToNull = costOfCarWithoutNDSToNull,
                                                PaymentDelay = paymentDelay,
                                                tenderItemUuid = Guid.Parse(items.tenderItemUuid),
                                                nmcName = items.nmcName,
                                                note = note,
                                                itemDescription = itemsDescription,
                                                cargoWeight = cargoWeight
                                            };
                                            tenderContragents.Add(registerTenderContragent);
                                        }
                                    }
                                    contragents.Add(Int64.Parse(items.itemExternalN), tenderContragents);
                                }
                            }
                        }
                        context.UpdateDataRegisterContragents(contragents);
                        List<SpecificationListViewModel> specificationListViews = new List<SpecificationListViewModel>();
                        int AlgorithmId = 2;
                        long OrderId = Int64.Parse(myDeserializedClass.data.tenderExternalN.Remove(myDeserializedClass.data.tenderExternalN.IndexOf('-')));
                        OrderID = OrderId;
                        context.SaveListStatus(new CompetetiveListStepsInfoViewModel()
                        {
                            StepId = 3,
                            OrderId = OrderID,
                            userId = userId,
                            tenderNumber = tenderNumber
                        });
                        context.getCurrentStatusForListKL(OrderID, userId, tenderNumber);
                        currentStatus = context.getCurrentStatusForList(OrderID, tenderNumber);
                        var CompetitiveListInfo_ = context.getCompetitiveListInfo(OrderId);
                        var specificationList = context.GetSpecifications(null, 5, 1, OrderId, true, CompetitiveListInfo_.FilterTripTypeId,
                                   false, null, true, CompetitiveListInfo_.FilterVehicleTypeId, true, CompetitiveListInfo_.FilterPayerId,
                                   false, AlgorithmId);

                        foreach (var it in contragents)
                        {
                            for (int i = 0; i < it.Value.Count; i++)
                            {
                                SpecificationListViewModel model = new SpecificationListViewModel();

                                var spec = specificationList.Find(x => x.edrpou_aps == it.Value[i].EDRPOUContragent);
                                if (spec != null)
                                {
                                    model.OrderId = (int)OrderId;
                                    model.tenderNumber = tenderNumber;
                                    model.CarryCapacity = specificationList[0].CarryCapacity;
                                    model.DaysDelay = it.Value[i].PaymentDelay;
                                    model.ExpeditorName = it.Value[i].ContragentName;
                                    model.NameSpecification = it.Value[i].nmcName;
                                    model.itemExternalNumber = it.Value[i].itemExternalNumber;
                                    model.ContragentIdAps = it.Value[i].ContragentIdAps;
                                    model.DateUpdateInfo = it.Value[i].DateUpdateInfo;
                                    model.IsWinner = it.Value[i].IsWinner;
                                    model.EDRPOUContragent = it.Value[i].EDRPOUContragent;
                                    model.emailContragent = it.Value[i].emailContragent;
                                    model.transportUnitsProposed = it.Value[i].transportUnitsProposed;
                                    model.acceptedTransportUnits = it.Value[i].acceptedTransportUnits;
                                    model.costOfCarWithoutNDS = it.Value[i].costOfCarWithoutNDS;
                                    model.tenderItemUuid = it.Value[i].tenderItemUuid;
                                    model.costOfCarWithoutNDSToNull = it.Value[i].costOfCarWithoutNDSToNull;
                                    model.note = it.Value[i].note;
                                    model.ArrivalPoint = spec.ArrivalPoint;
                                    model.CanBeDelete = spec.CanBeDelete;
                                    model.CarryCapacityId = spec.CarryCapacityId;
                                    model.CarryCapacityVal = spec.CarryCapacityVal;
                                    model.ContragentName = spec.ContragentName;
                                    model.DeparturePoint = spec.DeparturePoint;
                                    model.edrpou_aps = spec.edrpou_aps;
                                    model.email_aps = spec.email_aps;
                                    model.FilterPayerId = spec.FilterPayerId;
                                    model.FilterSpecificationTypeId = spec.FilterSpecificationTypeId;
                                    model.FilterTripTypeId = spec.FilterTripTypeId;
                                    model.FilterVehicleTypeId = spec.FilterVehicleTypeId;
                                    model.FreightName = spec.FreightName;
                                    model.GenId = spec.GenId;
                                    model.GroupeSpecId = spec.GroupeSpecId;
                                    model.Id = spec.Id;
                                    model.IntervalTypeId = spec.IntervalTypeId;
                                    model.IntervalTypeName = spec.IntervalTypeName;
                                    model.IsForwarder = spec.IsForwarder;
                                    model.IsFreight = spec.IsFreight;
                                    model.isTruck = spec.isTruck;
                                    model.MovingType = spec.MovingType;
                                    model.MovingTypeName = spec.MovingTypeName;
                                    model.NameGroupeSpecification = spec.NameGroupeSpecification;
                                    model.NameIntervalType = spec.NameIntervalType;
                                    model.NDSTax = spec.NDSTax;
                                    model.nmcName = spec.nmcName;
                                    model.PaymentDelay = spec.PaymentDelay;
                                    model.RateHour = spec.RateHour;
                                    model.RateKm = spec.RateKm;
                                    model.RateMachineHour = spec.RateMachineHour;
                                    model.RateTotalFreight = spec.RateTotalFreight;
                                    model.RateValue = spec.RateValue;
                                    model.returnurl = spec.returnurl;
                                    model.RouteLength = spec.RouteLength;
                                    model.RouteName = spec.RouteName;
                                    model.RouteTypeId = spec.RouteTypeId;
                                    model.RouteTypeName = spec.RouteTypeName;
                                    model.tripTypeName = spec.tripTypeName;
                                    model.UsedRateId = spec.UsedRateId;
                                    model.UsedRateName = spec.UsedRateName;
                                    model.UsePayerFilter = spec.UsePayerFilter;
                                    model.UseRouteFilter = spec.UseRouteFilter;
                                    model.UseSpecificationTypeFilter = spec.UseSpecificationTypeFilter;
                                    model.UseTripTypeFilter = spec.UseTripTypeFilter;
                                    model.UseVehicleTypeFilter = spec.UseVehicleTypeFilter;
                                    model.VehicleTypeName = spec.VehicleTypeName;
                                    model.itemDescription = it.Value[i].itemDescription;
                                    model.cargoWeight = cargoWeight;
                                    model.tenderTureNumber = myDeserializedClass.data.stageNumber;
                                    specificationListViews.Add(model);
                                }
                                else
                                {
                                    spec = context.GetSpesificationData(it.Value[i].EDRPOUContragent);
                                    spec.OrderId = (int)OrderId;
                                    spec.tenderNumber = tenderNumber;
                                    spec.DaysDelay = it.Value[i].PaymentDelay;
                                    spec.ExpeditorName = it.Value[i].ContragentName;
                                    spec.FilterPayerId = CompetitiveListInfo_.FilterPayerId;
                                    spec.FilterSpecificationTypeId = CompetitiveListInfo_.FilterSpecificationTypeId;
                                    spec.FilterTripTypeId = CompetitiveListInfo_.FilterTripTypeId;
                                    spec.FilterVehicleTypeId = CompetitiveListInfo_.FilterVehicleTypeId;
                                    spec.FilterTripTypeId = CompetitiveListInfo_.FilterTripTypeId;
                                    spec.FilterVehicleTypeId = CompetitiveListInfo_.FilterVehicleTypeId;
                                    spec.FilterPayerId = CompetitiveListInfo_.FilterPayerId;
                                    spec.isTruck = true;
                                    spec.UseTripTypeFilter = true;
                                    spec.UseVehicleTypeFilter = true;
                                    spec.NameSpecification = it.Value[i].nmcName;
                                    spec.UsePayerFilter = CompetitiveListInfo_.UsePayerFilter;
                                    spec.UseRouteFilter = CompetitiveListInfo_.UseRouteFilter;
                                    spec.UseSpecificationTypeFilter = CompetitiveListInfo_.UseSpecificationTypeFilter;
                                    spec.UseTripTypeFilter = CompetitiveListInfo_.UseTripTypeFilter;
                                    spec.UseVehicleTypeFilter = CompetitiveListInfo_.UseVehicleTypeFilter;
                                    spec.VehicleTypeName = CompetitiveListInfo_.VehicleTypeName;
                                    spec.edrpou_aps = it.Value[i].EDRPOUContragent;
                                    spec.email_aps = it.Value[i].emailContragent;
                                    spec.isTruck = CompetitiveListInfo_.IsTruck;
                                    spec.itemExternalNumber = it.Value[i].itemExternalNumber;
                                    spec.ContragentIdAps = it.Value[i].ContragentIdAps;
                                    spec.DateUpdateInfo = it.Value[i].DateUpdateInfo;
                                    spec.IsWinner = it.Value[i].IsWinner;
                                    spec.EDRPOUContragent = it.Value[i].EDRPOUContragent;
                                    spec.emailContragent = it.Value[i].emailContragent;
                                    spec.transportUnitsProposed = it.Value[i].transportUnitsProposed;
                                    spec.acceptedTransportUnits = it.Value[i].acceptedTransportUnits;
                                    spec.costOfCarWithoutNDS = it.Value[i].costOfCarWithoutNDS;
                                    spec.tenderItemUuid = it.Value[i].tenderItemUuid;
                                    spec.costOfCarWithoutNDSToNull = it.Value[i].costOfCarWithoutNDSToNull;
                                    spec.note = it.Value[i].note;
                                    spec.itemDescription = it.Value[i].itemDescription;
                                    spec.cargoWeight = cargoWeight;
                                    spec.tenderTureNumber = myDeserializedClass.data.stageNumber;
                                    
                                    specificationListViews.Add(spec);
                                }
                            }
                        }
                        if (specificationListViews.Count != 0)
                        {
                            if (context.IsContainTender(tenderNumber, myDeserializedClass.data.stageNumber))
                            {
                                foreach (var model in specificationListViews)
                                {
                                    Guid formUuid;
                                    context.NewSpecification(model, this.userId, tenderNumber, out formUuid);
                                    context.SetRegisterMessageData(tenderNumber, model, model.OrderId, formUuid, (int)model.tenderTureNumber);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            if (Int32.Parse(updateDeserializedClass.process) >= 8)
            {
                var DisplayValues = context.getOrderCompetitiveList(userId, OrderID, tenderNumber);
                var CompetitiveListInfo = context.getCompetitiveListInfo(OrderID, tenderNumber);
                var listStatuses = context.getAvialiableStepsForList(OrderID, tenderNumber);
                var listCurrentStatuses = context.listCurrentStatuses(OrderID);
                var listDisplayValues = context.listDisplayValues(OrderID, userId);
                var list_listStatuses = context.list_listStatuses(OrderID);
                var listDisplayValues_ = JsonConvert.SerializeObject(listDisplayValues);
                var list_listStatuses_ = JsonConvert.SerializeObject(list_listStatuses);
                List<CompetitiveListStepViewModel> listStKL = new List<CompetitiveListStepViewModel>();
                foreach (var el in listStatuses)
                {
                    listStKL.Add(el);
                }
                if (currentStatus != null && listStKL.Count <= 2)
                {
                    context.SaveListStatus(new CompetetiveListStepsInfoViewModel()
                    {
                        StepId = 3,
                        OrderId = OrderID,
                        userId = userId,
                        tenderNumber = tenderNumber
                    });
                    context.getCurrentStatusForListKL(OrderID, userId, tenderNumber);
                    currentStatus = context.getCurrentStatusForList(OrderID, tenderNumber);
                    DisplayValues = context.getOrderCompetitiveList(userId, OrderID, tenderNumber);
                    CompetitiveListInfo = context.getCompetitiveListInfo(OrderID, tenderNumber);
                    listStatuses = context.getAvialiableStepsForList(OrderID, tenderNumber);
                    listCurrentStatuses = context.listCurrentStatuses(OrderID);
                    listDisplayValues = context.listDisplayValues(OrderID, userId);
                    list_listStatuses = context.list_listStatuses(OrderID);
                    listDisplayValues_ = JsonConvert.SerializeObject(listDisplayValues);
                    list_listStatuses_ = JsonConvert.SerializeObject(list_listStatuses);
                    listStKL = new List<CompetitiveListStepViewModel>();
                    foreach (var el in listStatuses)
                    {
                        listStKL.Add(el);
                    }
                }
                return new JsonpResult
                {
                    Data = new { updateDeserializedClass, DisplayValues, CompetitiveListInfo, currentStatus, listStKL, listCurrentStatuses, listDisplayValues_, list_listStatuses_ },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                return new JsonpResult
                {
                    Data = new { updateDeserializedClass },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }

        private void GetCurrentParametersForTendersOfOrder(int tenderNumber, out UpdateRegisterStatusTender updateDeserializedClass_r, out IQueryable<OrderCompetitiveListViewModel> DisplayValues_r,
            out CompetitiveListViewModel CompetitiveListInfo_r, out CompetetiveListStepsInfoViewModel currentStatus_r, out List<CompetitiveListStepViewModel> listStKL_r,
            out List<CompetetiveListStepsInfoViewModel> listCurrentStatuses_r, out Dictionary<int, IQueryable<OrderCompetitiveListViewModel>> listDisplayValues_r,
            out string listDisplayValues_mod, out Dictionary<int, IEnumerable<CompetitiveListStepViewModel>> list_listStatuses_r, out string list_listStatuses_mod)
        {
            UpdateRegisterStatusTender updateDeserializedClass = new UpdateRegisterStatusTender();
            CompetetiveListStepsInfoViewModel currentStatus = new CompetetiveListStepsInfoViewModel();

            long OrderID = 0;
            try
            {
                NameValueCollection allAppSettings = ConfigurationManager.AppSettings;
                BaseClient clientbase = new BaseClient($"{allAppSettings["ApiGetTenderId"]}{tenderNumber}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                var JSONresponse = new GetApiTendAjax().GetCallAsync(clientbase).Result.ResponseMessage;
                RequestJSONDeserializedToModel myDeserializedClass = JsonConvert.DeserializeObject<RequestJSONDeserializedToModel>(JSONresponse);
                if (myDeserializedClass.success)
                {
                    updateDeserializedClass = context.UpdateCLStatusTenderOrder(myDeserializedClass, tenderNumber);
                    if (Int32.Parse(updateDeserializedClass.process) >= 8)
                    {
                        string cargoWeight = null;
                        // Вытягивание данных о контрагентах из aps tender
                        Dictionary<long, List<RegisterTenderContragent>> contragents = new Dictionary<long, List<RegisterTenderContragent>>();
                        if (myDeserializedClass.data.lots[0].items.Count != 0 && Int32.Parse(myDeserializedClass.data.process) >= 8)
                        {
                            foreach (var items in myDeserializedClass.data.lots[0].items)
                            {
                                BaseClient clientbaseOffer = new BaseClient($"{allAppSettings["ApiUrlGetOffer"]}{items.tenderItemUuid}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                                var JSONresponseContragent = $"{{\"data\":{new GetApiTendAjax().GetCallAsync(clientbaseOffer).Result.ResponseMessage}}}";
                                RequestJSONContragentModel myDeserializedClassContragent = JsonConvert.DeserializeObject<RequestJSONContragentModel>(JSONresponseContragent);
                                List<RequestJSONContragentMainData> listContAgentModelJSONDesiarized = new List<RequestJSONContragentMainData>();
                                int SupplierIdWinnerContragent = 0;
                                foreach (var item in myDeserializedClassContragent.Data)
                                {
                                    try
                                    {
                                        BaseClient clientbaseSuppContragent = new BaseClient($"{allAppSettings["ApiUrlGetSuppContrAgent"]}{item.SupplierId}", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                                        var JSONresponseContAgentModel = new GetApiTendAjax().GetCallAsync(clientbaseSuppContragent).Result.ResponseMessage;
                                        RequestJSONContragentMainData myDeserializedClassContragentModel = JsonConvert.DeserializeObject<RequestJSONContragentMainData>(JSONresponseContAgentModel);
                                        listContAgentModelJSONDesiarized.Add(myDeserializedClassContragentModel);
                                        if ((item.IsWinner != null) ? true : false)
                                        {
                                            SupplierIdWinnerContragent = item.SupplierId;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                }
                                List<ContrAgentModel> listContragentModels = new List<ContrAgentModel>();
                                ContrAgentModel isWinnerContragent = new ContrAgentModel();
                                listContragentModels = context.GetAgentModels(listContAgentModelJSONDesiarized, myDeserializedClassContragent);   // Список всех контрагентов, которые принимали участие в тендере
                                isWinnerContragent = context.GetWinnerContragent(listContragentModels, SupplierIdWinnerContragent); // Информация о контрагенте-победителе
                                List<RegisterTenderContragent> tenderContragents = new List<RegisterTenderContragent>();
                                foreach (var el in listContragentModels)
                                {
                                    double costOfCarWithoutNDS = 0d;
                                    double costOfCarWithoutNDSToNull = 0d;
                                    int paymentDelay = 0;
                                    string note = null;
                                    string itemsDescription = items.itemNote;
                                    try
                                    {
                                        foreach (var it in items.propValues)
                                        {
                                            if (it.ВесТ != null)
                                            {
                                                cargoWeight = it.ВесТ;
                                                break;
                                            }
                                        }

                                        for (int i = 0; i < el.listCritariaValues.Count; i++)
                                        {
                                            if (costOfCarWithoutNDS == 0)
                                            {
                                                costOfCarWithoutNDS = (el.listCritariaValues[i].Id == 61) ? Double.Parse(el.listCritariaValues[i].Value.ToString()) : 0d;
                                            }
                                            if (costOfCarWithoutNDSToNull == 0)
                                            {
                                                costOfCarWithoutNDSToNull = (el.listCritariaValues[i].Id == 64) ? Double.Parse(el.listCritariaValues[i].Value.ToString()) : 0d;
                                            }
                                            if (paymentDelay == 0)
                                            {
                                                paymentDelay = (el.listCritariaValues[i].Id == 66) ? Int32.Parse(el.listCritariaValues[i].Value.ToString()) : 0;
                                            }
                                            if (note == null)
                                            {
                                                note = (el.listCritariaValues[i].Id == 510) ? el.listCritariaValues[i].Value.ToString() : null;
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                    if (isWinnerContragent != null && el.SupplierId != isWinnerContragent.SupplierId)
                                    {

                                        RegisterTenderContragent registerTenderContragent = new RegisterTenderContragent()
                                        {
                                            OrderId = Int64.Parse(myDeserializedClass.data.tenderExternalN.Remove(myDeserializedClass.data.tenderExternalN.IndexOf('-'))),
                                            tenderNumber = Int32.Parse(myDeserializedClass.data.tenderNumber),
                                            itemExternalNumber = Int64.Parse(items.itemExternalN),
                                            ContragentName = el.OwnershipTypeName + ' ' + el.SupplierName,
                                            ContragentIdAps = el.SupplierId,
                                            DateUpdateInfo = DateTime.Now,
                                            IsWinner = false,
                                            EDRPOUContragent = Int64.Parse(el.SupplierEdrpou),
                                            emailContragent = (el.ContactEmail != null) ? el.ContactEmail : el.SupplierCEOContragent.EMail,
                                            transportUnitsProposed = 1,
                                            acceptedTransportUnits = 0,
                                            costOfCarWithoutNDS = costOfCarWithoutNDS,
                                            costOfCarWithoutNDSToNull = costOfCarWithoutNDSToNull,
                                            PaymentDelay = paymentDelay,
                                            tenderItemUuid = Guid.Parse(items.tenderItemUuid),
                                            nmcName = items.nmcName,
                                            note = note,
                                            itemDescription = itemsDescription,
                                            cargoWeight = cargoWeight
                                        };
                                        tenderContragents.Add(registerTenderContragent);
                                    }
                                    else
                                    {
                                        RegisterTenderContragent registerTenderContragent = new RegisterTenderContragent()
                                        {
                                            OrderId = Int64.Parse(myDeserializedClass.data.tenderExternalN.Remove(myDeserializedClass.data.tenderExternalN.IndexOf('-'))),
                                            tenderNumber = Int32.Parse(myDeserializedClass.data.tenderNumber),
                                            itemExternalNumber = Int64.Parse(items.itemExternalN),
                                            ContragentName = el.OwnershipTypeName + ' ' + el.SupplierName,
                                            ContragentIdAps = el.SupplierId,
                                            DateUpdateInfo = DateTime.Now,
                                            IsWinner = true,
                                            EDRPOUContragent = Int64.Parse(el.SupplierEdrpou),
                                            emailContragent = (el.ContactEmail != null) ? el.ContactEmail : el.SupplierCEOContragent.EMail,
                                            transportUnitsProposed = 1,
                                            acceptedTransportUnits = 1,
                                            costOfCarWithoutNDS = costOfCarWithoutNDS,
                                            costOfCarWithoutNDSToNull = costOfCarWithoutNDSToNull,
                                            PaymentDelay = paymentDelay,
                                            tenderItemUuid = Guid.Parse(items.tenderItemUuid),
                                            nmcName = items.nmcName,
                                            note = note,
                                            itemDescription = itemsDescription,
                                            cargoWeight = cargoWeight
                                        };
                                        tenderContragents.Add(registerTenderContragent);
                                    }
                                }
                                contragents.Add(Int64.Parse(items.itemExternalN), tenderContragents);
                            }
                        }
                        context.UpdateDataRegisterContragents(contragents);
                        List<SpecificationListViewModel> specificationListViews = new List<SpecificationListViewModel>();
                        int AlgorithmId = 2;
                        long OrderId = Int64.Parse(myDeserializedClass.data.tenderExternalN.Remove(myDeserializedClass.data.tenderExternalN.IndexOf('-')));
                        OrderID = OrderId;
                        context.SaveListStatus(new CompetetiveListStepsInfoViewModel()
                        {
                            StepId = 3,
                            OrderId = OrderID,
                            userId = userId,
                            tenderNumber = tenderNumber
                        });
                        context.getCurrentStatusForListKL(OrderID, userId, tenderNumber);
                        currentStatus = context.getCurrentStatusForList(OrderID, tenderNumber);
                        var CompetitiveListInfo_ = context.getCompetitiveListInfo(OrderId);
                        var specificationList = context.GetSpecifications(null, 5, 1, OrderId, true, CompetitiveListInfo_.FilterTripTypeId,
                                   false, null, true, CompetitiveListInfo_.FilterVehicleTypeId, true, CompetitiveListInfo_.FilterPayerId,
                                   false, AlgorithmId);

                        foreach (var it in contragents)
                        {
                            for (int i = 0; i < it.Value.Count; i++)
                            {
                                SpecificationListViewModel model = new SpecificationListViewModel();

                                var spec = specificationList.Find(x => x.edrpou_aps == it.Value[i].EDRPOUContragent);
                                if (spec != null)
                                {
                                    model.OrderId = (int)OrderId;
                                    model.tenderNumber = tenderNumber;
                                    model.CarryCapacity = specificationList[0].CarryCapacity;
                                    model.DaysDelay = it.Value[i].PaymentDelay;
                                    model.ExpeditorName = it.Value[i].ContragentName;
                                    model.NameSpecification = it.Value[i].nmcName;
                                    model.itemExternalNumber = it.Value[i].itemExternalNumber;
                                    model.ContragentIdAps = it.Value[i].ContragentIdAps;
                                    model.DateUpdateInfo = it.Value[i].DateUpdateInfo;
                                    model.IsWinner = it.Value[i].IsWinner;
                                    model.EDRPOUContragent = it.Value[i].EDRPOUContragent;
                                    model.emailContragent = it.Value[i].emailContragent;
                                    model.transportUnitsProposed = it.Value[i].transportUnitsProposed;
                                    model.acceptedTransportUnits = it.Value[i].acceptedTransportUnits;
                                    model.costOfCarWithoutNDS = it.Value[i].costOfCarWithoutNDS;
                                    model.tenderItemUuid = it.Value[i].tenderItemUuid;
                                    model.costOfCarWithoutNDSToNull = it.Value[i].costOfCarWithoutNDSToNull;
                                    model.note = it.Value[i].note;
                                    model.ArrivalPoint = spec.ArrivalPoint;
                                    model.CanBeDelete = spec.CanBeDelete;
                                    model.CarryCapacityId = spec.CarryCapacityId;
                                    model.CarryCapacityVal = spec.CarryCapacityVal;
                                    model.ContragentName = spec.ContragentName;
                                    model.DeparturePoint = spec.DeparturePoint;
                                    model.edrpou_aps = spec.edrpou_aps;
                                    model.email_aps = spec.email_aps;
                                    model.FilterPayerId = spec.FilterPayerId;
                                    model.FilterSpecificationTypeId = spec.FilterSpecificationTypeId;
                                    model.FilterTripTypeId = spec.FilterTripTypeId;
                                    model.FilterVehicleTypeId = spec.FilterVehicleTypeId;
                                    model.FreightName = spec.FreightName;
                                    model.GenId = spec.GenId;
                                    model.GroupeSpecId = spec.GroupeSpecId;
                                    model.Id = spec.Id;
                                    model.IntervalTypeId = spec.IntervalTypeId;
                                    model.IntervalTypeName = spec.IntervalTypeName;
                                    model.IsForwarder = spec.IsForwarder;
                                    model.IsFreight = spec.IsFreight;
                                    model.isTruck = spec.isTruck;
                                    model.MovingType = spec.MovingType;
                                    model.MovingTypeName = spec.MovingTypeName;
                                    model.NameGroupeSpecification = spec.NameGroupeSpecification;
                                    model.NameIntervalType = spec.NameIntervalType;
                                    model.NDSTax = spec.NDSTax;
                                    model.nmcName = spec.nmcName;
                                    model.PaymentDelay = spec.PaymentDelay;
                                    model.RateHour = spec.RateHour;
                                    model.RateKm = spec.RateKm;
                                    model.RateMachineHour = spec.RateMachineHour;
                                    model.RateTotalFreight = spec.RateTotalFreight;
                                    model.RateValue = spec.RateValue;
                                    model.returnurl = spec.returnurl;
                                    model.RouteLength = spec.RouteLength;
                                    model.RouteName = spec.RouteName;
                                    model.RouteTypeId = spec.RouteTypeId;
                                    model.RouteTypeName = spec.RouteTypeName;
                                    model.tripTypeName = spec.tripTypeName;
                                    model.UsedRateId = spec.UsedRateId;
                                    model.UsedRateName = spec.UsedRateName;
                                    model.UsePayerFilter = spec.UsePayerFilter;
                                    model.UseRouteFilter = spec.UseRouteFilter;
                                    model.UseSpecificationTypeFilter = spec.UseSpecificationTypeFilter;
                                    model.UseTripTypeFilter = spec.UseTripTypeFilter;
                                    model.UseVehicleTypeFilter = spec.UseVehicleTypeFilter;
                                    model.VehicleTypeName = spec.VehicleTypeName;
                                    model.itemDescription = it.Value[i].itemDescription;
                                    model.cargoWeight = cargoWeight;
                                    model.tenderTureNumber = myDeserializedClass.data.stageNumber;
                                    specificationListViews.Add(model);
                                }
                                else
                                {
                                    SpecificationListViewModel instance = new SpecificationListViewModel()
                                    {
                                        OrderId = (int)OrderId,
                                        GenId = specificationList[0].GenId,
                                        tenderNumber = tenderNumber,
                                        CarryCapacity = specificationList[0].CarryCapacity,
                                        DaysDelay = it.Value[i].PaymentDelay,
                                        ExpeditorName = it.Value[i].ContragentName,
                                        FilterPayerId = specificationList[0].FilterPayerId,
                                        FilterSpecificationTypeId = specificationList[0].FilterSpecificationTypeId,
                                        FilterTripTypeId = specificationList[0].FilterTripTypeId,
                                        FilterVehicleTypeId = specificationList[0].FilterVehicleTypeId,
                                        FreightName = specificationList[0].FreightName,
                                        GroupeSpecId = specificationList[0].GroupeSpecId,
                                        IntervalTypeId = specificationList[0].IntervalTypeId,
                                        IsForwarder = specificationList[0].IsForwarder,
                                        IsFreight = specificationList[0].IsFreight,
                                        NDSTax = specificationList[0].NDSTax,
                                        NameGroupeSpecification = specificationList[0].NameGroupeSpecification,
                                        NameSpecification = it.Value[i].nmcName,
                                        RateValue = specificationList[0].RateValue,
                                        RouteTypeId = specificationList[0].RouteTypeId,
                                        UsePayerFilter = specificationList[0].UsePayerFilter,
                                        UseRouteFilter = specificationList[0].UseRouteFilter,
                                        UseSpecificationTypeFilter = specificationList[0].UseSpecificationTypeFilter,
                                        UseTripTypeFilter = specificationList[0].UseTripTypeFilter,
                                        UseVehicleTypeFilter = specificationList[0].UseVehicleTypeFilter,
                                        UsedRateId = specificationList[0].UsedRateId,
                                        UsedRateName = specificationList[0].UsedRateName,
                                        VehicleTypeName = specificationList[0].VehicleTypeName,
                                        edrpou_aps = it.Value[i].EDRPOUContragent,
                                        email_aps = it.Value[i].emailContragent,
                                        isTruck = specificationList[0].isTruck,
                                        itemExternalNumber = it.Value[i].itemExternalNumber,
                                        ContragentIdAps = it.Value[i].ContragentIdAps,
                                        DateUpdateInfo = it.Value[i].DateUpdateInfo,
                                        IsWinner = it.Value[i].IsWinner,
                                        EDRPOUContragent = it.Value[i].EDRPOUContragent,
                                        emailContragent = it.Value[i].emailContragent,
                                        transportUnitsProposed = it.Value[i].transportUnitsProposed,
                                        acceptedTransportUnits = it.Value[i].acceptedTransportUnits,
                                        costOfCarWithoutNDS = it.Value[i].costOfCarWithoutNDS,
                                        tenderItemUuid = it.Value[i].tenderItemUuid,
                                        costOfCarWithoutNDSToNull = it.Value[i].costOfCarWithoutNDSToNull,
                                        note = it.Value[i].note,
                                        itemDescription = it.Value[i].itemDescription,
                                        cargoWeight = cargoWeight,
                                        tenderTureNumber = myDeserializedClass.data.stageNumber
                                    };
                                    specificationListViews.Add(instance);
                                }
                            }
                        }
                        if (specificationListViews.Count != 0)
                        {
                            if (context.IsContainTender(tenderNumber, myDeserializedClass.data.stageNumber))
                            {
                                foreach (var model in specificationListViews)
                                {
                                    Guid formUuid;
                                    context.NewSpecification(model, this.userId, tenderNumber, out formUuid);
                                    context.SetRegisterMessageData(tenderNumber, model, model.OrderId, formUuid, (int)model.tenderTureNumber);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            IQueryable<OrderCompetitiveListViewModel> DisplayValues = null;
            CompetitiveListViewModel CompetitiveListInfo = null;
            IEnumerable<CompetitiveListStepViewModel> listStatuses;
            List<CompetitiveListStepViewModel> listStKL = null;
            List<CompetetiveListStepsInfoViewModel> listCurrentStatuses = null;
            Dictionary<int, IQueryable<OrderCompetitiveListViewModel>> listDisplayValues = null;
            string listDisplayValues_ = null;
            Dictionary<int, IEnumerable<CompetitiveListStepViewModel>> list_listStatuses = null;
            string list_listStatuses_ = null;
            if (Int32.Parse(updateDeserializedClass.process) >= 8)
            {
                DisplayValues = context.getOrderCompetitiveList(userId, OrderID, tenderNumber);
                CompetitiveListInfo = context.getCompetitiveListInfo(OrderID, tenderNumber);
                listStatuses = context.getAvialiableStepsForList(OrderID, tenderNumber);
                listCurrentStatuses = context.listCurrentStatuses(OrderID);
                listDisplayValues = context.listDisplayValues(OrderID, userId);
                list_listStatuses = context.list_listStatuses(OrderID);
                listDisplayValues_ = JsonConvert.SerializeObject(listDisplayValues);
                list_listStatuses_ = JsonConvert.SerializeObject(list_listStatuses);
                listStKL = new List<CompetitiveListStepViewModel>();
                foreach (var el in listStatuses)
                {
                    listStKL.Add(el);
                }
                if (currentStatus != null && listStKL.Count <= 2)
                {
                    context.SaveListStatus(new CompetetiveListStepsInfoViewModel()
                    {
                        StepId = 3,
                        OrderId = OrderID,
                        userId = userId,
                        tenderNumber = tenderNumber
                    });
                    context.getCurrentStatusForListKL(OrderID, userId, tenderNumber);
                    currentStatus = context.getCurrentStatusForList(OrderID, tenderNumber);
                    DisplayValues = context.getOrderCompetitiveList(userId, OrderID, tenderNumber);
                    CompetitiveListInfo = context.getCompetitiveListInfo(OrderID, tenderNumber);
                    listStatuses = context.getAvialiableStepsForList(OrderID, tenderNumber);
                    listCurrentStatuses = context.listCurrentStatuses(OrderID);
                    listDisplayValues = context.listDisplayValues(OrderID, userId);
                    list_listStatuses = context.list_listStatuses(OrderID);
                    listDisplayValues_ = JsonConvert.SerializeObject(listDisplayValues);
                    list_listStatuses_ = JsonConvert.SerializeObject(list_listStatuses);
                    listStKL = new List<CompetitiveListStepViewModel>();
                    foreach (var el in listStatuses)
                    {
                        listStKL.Add(el);
                    }
                }
            }
            updateDeserializedClass_r = updateDeserializedClass;
            DisplayValues_r = DisplayValues;
            CompetitiveListInfo_r = CompetitiveListInfo;
            currentStatus_r = currentStatus;
            listStKL_r = listStKL;
            listCurrentStatuses_r = listCurrentStatuses;
            listDisplayValues_r = listDisplayValues;
            listDisplayValues_mod = listDisplayValues_;
            list_listStatuses_r = list_listStatuses;
            list_listStatuses_mod = list_listStatuses_;
        }

        [HttpGet]
        public ActionResult UpdateAllStatusOrderTender()
        {
            Debug.WriteLine("Вход в контроллер");
            Task.WaitAll(Task.Run(AsyncUpdateTenderRegisty));
            Debug.WriteLine("Продолжение главного потока");
            return RedirectToAction("Index", "Home");
        }
        private async Task AsyncUpdateTenderRegisty()
        {
            Debug.WriteLine("Вход в асинхронный метод");
            HangFireTasks.HangFireTasks task = new HangFireTasks.HangFireTasks();
            await task.ListTasks(false);
            Debug.WriteLine("Выход из асинхронного метода");
        }

        [HttpPost]
        public ActionResult ResultsTenderUpdate(TenderRegistryUpdate registryUpdate)
        {
            context.UpdateRegisterTenders(Convert.ToInt32(registryUpdate.tenderNumber), registryUpdate.resultsTender);
            return Json("{\"success\":true}");
        }

        [HttpPost]
        public ActionResult RemainingTimeUpdate(RemainingTimeUpdate timeUpdate)
        {
            context.RemainingTime(timeUpdate.TimeList);
            return Json("{\"success\":true}");
        }

        [HttpPost]
        public ActionResult SendNotificationTender(TenderSumOrderId tenderSumOrder)
        {
            RegisterTenders modifRegisterTenders = new RegisterTenders();
            OrderID = Convert.ToInt64(tenderSumOrder.OrderId);
            Dictionary<string, string> otherParams = new Dictionary<string, string>();
            DateTimeOffset localTimeStart, otherTimeStart, localTimeEnd, otherTimeEnd;
            dynamic tenderForma = null;
            var routePointsLoadinfo = context.getLoadPoints(OrderID, true).ToList();
            var routePointsUnloadinfo = context.getLoadPoints(OrderID, false).ToList();
            int num = 0;
            try
            {
                TendFormDeserializedJSON tendFormDeserializedJSON = tenderSumOrder.ListItemsModelTenderForm;
                num = (routePointsLoadinfo.Count < routePointsUnloadinfo.Count) ? routePointsUnloadinfo.Count : routePointsLoadinfo.Count;
                switch (num)
                {
                    case 0:
                        tenderForma = new TenderForma<PropAliasValuesOne>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 1:
                        tenderForma = new TenderForma<PropAliasValuesOne>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 2:
                        tenderForma = new TenderForma<PropAliasValuesTwo>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 3:
                        tenderForma = new TenderForma<PropAliasValuesThree>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 4:
                        tenderForma = new TenderForma<PropAliasValuesFour>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    case 5:
                        tenderForma = new TenderForma<PropAliasValuesFive>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                    default:
                        tenderForma = new TenderForma<PropAliasValuesOne>(context.getCompetitiveListInfo(OrderID), context.GetTenderServices(), context.GetBalanceKeepers(),
                   tendFormDeserializedJSON, context.GetSpecificationNames(), context.GetCountries(), context.GetOrderTruckTransport(OrderID),
                   context.getLoadPoints(OrderID, true).ToList(), context.getLoadPoints(OrderID, false).ToList()); break;
                }

                tenderForma.data.InitializedAfterDeserialized();
                tenderForma.data.tenderName = tenderSumOrder.ListItemsModelTenderForm.TenderName;
                localTimeStart = new DateTimeOffset(DateTime.Parse(tenderSumOrder.ListItemsModelTenderForm.DateStart));
                localTimeEnd = new DateTimeOffset(DateTime.Parse(tenderSumOrder.ListItemsModelTenderForm.DateEnd));
                otherTimeStart = localTimeStart.ToUniversalTime();
                otherTimeEnd = localTimeEnd.ToUniversalTime();
                tenderForma.data.dateStart = otherTimeStart.ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                tenderForma.data.dateEnd = otherTimeEnd.ToString("yyyy-MM-dd'T'HH:mm:ssZ");
                otherParams = tenderForma.otherParams;
            }
            catch (Exception e)
            {

            }
            NameValueCollection allAppSettings = ConfigurationManager.AppSettings;
            RequestJSONDeserializedToModel myDeserializedClass = null;
            var appsett = allAppSettings["SwitchToMultipleTenders"];
            if (context.IsRegisterTendersExist(OrderID, Boolean.Parse(appsett)))
            {
                BaseClient clientbase = new BaseClient(allAppSettings["ApiUrl"], allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                var response = new PostApiTender<PropAliasValuesOne>().GetCallAsync(clientbase, tenderForma, num).Result.ResponseMessage;
                try
                {
                    byte[] dataOrder = OrderAsExcelData((int)OrderID);
                    myDeserializedClass = JsonConvert.DeserializeObject<RequestJSONDeserializedToModel>(response);
                    DateTime nowDateTime = DateTime.Now;
                    TimeSpan timeSpan = myDeserializedClass.data.dateEnd - nowDateTime;
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
                        tenderOwnerPath = myDeserializedClass.data.tenderOwnerPath,
                        remainingTime = timeSpan.ToString(),
                        dateCreate = myDeserializedClass.data.dateCreate
                    };
                    context.AddNewDataTender(registerTenders);
                    HttpClientApi clientbaseAddFile = new HttpClientApi($"{allAppSettings["ApiUrlAddFile"]}{registerTenders.tenderNumber}&suppVisible=1", allAppSettings["ApiLogin"], allAppSettings["ApiPassordMD5"]);
                    new AddFilePostApiTender().GetCallAsync(clientbaseAddFile, dataOrder, $"OrderReport{OrderID}.xlsx");
                    modifRegisterTenders = context.GetRegisterTenders().Find(x => x.tenderNumber == registerTenders.tenderNumber);
                }
                catch (Exception e)
                {

                }
                //UpdateRegisterStatusTender updateDeserializedClass_r;
                //IQueryable<OrderCompetitiveListViewModel> DisplayValues_r;
                //CompetitiveListViewModel CompetitiveListInfo_r;
                //CompetetiveListStepsInfoViewModel currentStatus_r;
                //IEnumerable<CompetitiveListStepViewModel> listStatuses;
                //List<CompetitiveListStepViewModel> listStKL_r;
                //List<CompetetiveListStepsInfoViewModel> listCurrentStatuses_r;
                //Dictionary<int, IQueryable<OrderCompetitiveListViewModel>> listDisplayValues_r;
                //string listDisplayValues_mod;
                //Dictionary<int, IEnumerable<CompetitiveListStepViewModel>> list_listStatuses_r;
                //string list_listStatuses_mod;
                //GetCurrentParametersForTendersOfOrder(Convert.ToInt32(myDeserializedClass.data.tenderNumber), out updateDeserializedClass_r, out DisplayValues_r,
                //out CompetitiveListInfo_r, out currentStatus_r, out listStKL_r, out listCurrentStatuses_r, out listDisplayValues_r,
                //out listDisplayValues_mod, out list_listStatuses_r, out list_listStatuses_mod);

                return new JsonpResult
                {
                    Data = new { modifRegisterTenders, response },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                return Json("{\"success\":false,\"isLoadMultiple\":false}");
            }
        }

        public byte[] OrderAsExcelData(int id)
        {
            OrderBaseViewModel OrderTypeModel = null;
            var orderInfo = context.getOrder(id);
            var extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
            var extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
            int OrderType = 6;
            string AdressFrom, AdressTo;
            AdressFrom = "";
            AdressTo = "";
            if (orderInfo != null)
            {
                var DefaultCounty = context.getDefaultCountry();
                switch (orderInfo.OrderType)
                {
                    case 1:
                    case 3:
                    case 6:
                        OrderType = 6;
                        OrderTypeModel = orderInfo.ConvertTo<OrdersPassTransportViewModel>();
                        extOrderTypeModel1 = (OrderTypeModel as OrdersPassTransportViewModel);
                        context.getPassTrasportOrderData(ref extOrderTypeModel1);

                        if (((OrdersPassTransportViewModel)OrderTypeModel).CountryFrom == 0)
                        {
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFrom = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryFromName = DefaultCounty.CountryName;
                        }
                        if (((OrdersPassTransportViewModel)OrderTypeModel).CountryTo == 0)
                        {
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryTo = DefaultCounty.Id;
                            ((OrdersPassTransportViewModel)OrderTypeModel).CountryToName = DefaultCounty.CountryName;
                        }

                       ((OrdersPassTransportViewModel)OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                        ((OrdersPassTransportViewModel)OrderTypeModel).DefaultCountryName = DefaultCounty.CountryName;

                        AdressFrom = context.GetFromInfoForExport(extOrderTypeModel1.Id);
                        AdressTo = context.GetToInfoForExport(extOrderTypeModel1.Id);

                        break;

                    case 4:
                    case 5:
                    case 7:
                        OrderType = 7;
                        OrderTypeModel = orderInfo.ConvertTo<OrdersTruckTransportViewModel>();
                        extOrderTypeModel2 = (OrderTypeModel as OrdersTruckTransportViewModel);
                        context.getTruckTrasportOrderData(ref extOrderTypeModel2);

                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryId == 0) || (((OrdersTruckTransportViewModel)OrderTypeModel).TripType < 2))
                        {
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryId = DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ShipperCountryName = DefaultCounty.CountryName;
                        }
                        if ((((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryId == 0) || (((OrdersTruckTransportViewModel)OrderTypeModel).TripType < 2))
                        {
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryId = DefaultCounty.Id;
                            ((OrdersTruckTransportViewModel)OrderTypeModel).ConsigneeCountryName = DefaultCounty.CountryName;
                        }

                       ((OrdersTruckTransportViewModel)OrderTypeModel).DefaultCountry = DefaultCounty.Id;
                        ((OrdersTruckTransportViewModel)OrderTypeModel).DefaultCountryName = DefaultCounty.CountryName;

                        break;

                    default:
                        throw new NotImplementedException();
                }

                var Param = new RestParamsInfo();
                Param.Language = Request.UserLanguages[0];
                string MainHeader = "";
                if (OrderType == 6)
                    MainHeader = "Заявка на транспортные средства категории В ";
                else if (OrderType == 7)
                    MainHeader = "ЗАЯВКА НА ПЕРЕВОЗКУ ГРУЗОВ ПО МАРШРУТУ  № " + OrderTypeModel.Id.ToString();

                Param.MainHeader = MainHeader;

                string AcceptDate = context.GetAcceptDate(id);

                OrderClientsViewModel orderClientInfo = context.getClient(OrderTypeModel.ClientId);

                string ContractName = context.getContactName(OrderTypeModel.Id);
                List<OrderUsedCarViewModel> carList = context.getOrderCarsInfo(OrderTypeModel.Id).ToList();
                byte[] fileContents;
                fileContents = report.OrderRenderReport<OrderBaseViewModel>(OrderTypeModel, extOrderTypeModel1, AcceptDate, orderClientInfo, Param, AdressFrom, AdressTo, ContractName, extOrderTypeModel2, OrderType, carList);

                return fileContents;
            }

            return null;
        }
    }
}