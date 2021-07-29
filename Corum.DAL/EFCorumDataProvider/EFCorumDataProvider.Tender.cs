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
using System.Globalization;
using Corum.Models.ViewModels.Tender;


namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {

        public List<Corum.Models.Tender.TenderServices> GetTenderServices()
        {
            List<TenderServices> services = new List<Models.Tender.TenderServices>();
            var tenderList = db.TenderServices.ToList();
            foreach (var item in tenderList)
            {
                Corum.Models.Tender.TenderServices tender = new Models.Tender.TenderServices();
                tender.Id = item.Id;
                tender.industryName = item.industryName;
                tender.industryId = item.industryId;
                tender.industryId_Test = item.industryId_Test;
                services.Add(tender);
            }
            return services;
        }

        public List<SpecificationNames> GetSpecificationNames()
        {
            List<Corum.Models.Tender.SpecificationNames> specList = new List<Models.Tender.SpecificationNames>();
            var specNamesList = db.SpecificationNames.ToList();
            foreach (var item in specNamesList)
            {
                Corum.Models.Tender.SpecificationNames specNames = new Models.Tender.SpecificationNames();
                specNames.Id = item.Id;
                specNames.SpecCode = item.SpecCode;
                specNames.SpecName = item.SpecName;
                specNames.nmcTestId = item.nmcTestId;
                specNames.nmcWorkId = item.nmcWorkId;
                specNames.industryId = item.industryId;
                specNames.industryId_Test = item.industryId_Test;
                specList.Add(specNames);
            }
            return specList;
        }

        public List<Countries> GetCountries()
        {
            List<Corum.Models.Tender.Countries> countriesList = new List<Models.Tender.Countries>();
            var countriesNamesList = db.Countries.ToList();
            foreach (var item in countriesNamesList)
            {
                Corum.Models.Tender.Countries countriesNames = new Models.Tender.Countries();
                countriesNames.Code = item.Сode;
                countriesNames.alpha2 = item.alpha2;
                countriesNames.alpha3 = item.alpha3;
                countriesNames.Fullname = item.Fullname;
                countriesNames.IsDefault = item.IsDefault;
                countriesNames.Name = item.Name;

                countriesList.Add(countriesNames);
            }
            return countriesList;
        }

        public List<BalanceKeepers> GetBalanceKeepers()
        {
            List<Corum.Models.Tender.BalanceKeepers> balanceKeepList = new List<Models.Tender.BalanceKeepers>();
            var balanceNamesList = db.BalanceKeepers.ToList();
            foreach (var item in balanceNamesList)
            {
                Corum.Models.Tender.BalanceKeepers balanceKeepNames = new Models.Tender.BalanceKeepers();
                balanceKeepNames.Id = item.Id;
                balanceKeepNames.BalanceKeeper = item.BalanceKeeper;
                balanceKeepNames.subCompanyId = item.subCompanyId;
                balanceKeepNames.subCompanyId_Test = item.subCompanyId_Test;

                balanceKeepList.Add(balanceKeepNames);
            }
            return balanceKeepList;
        }
        public List<RegisterTenders> GetRegisterTenders()
        {
            List<Corum.Models.Tender.RegisterTenders> registerTenders = new List<Corum.Models.Tender.RegisterTenders>();
            var registerTendersList = db.RegisterTenders.OrderByDescending(x => x.dateEnd).ToList();
            foreach (var item in registerTendersList)
            {
                Corum.Models.Tender.RegisterTenders register = new Corum.Models.Tender.RegisterTenders();
                register.cargoName = item.cargoName;
                register.dateEnd = item.dateEnd;
                register.dateStart = item.dateStart;
                register.downloadAddress = item.downloadAddress;
                register.downloadDataRequired = item.downloadDataRequired;
                register.Id = item.Id;
                register.industryId = item.industryId;
                register.industryName = item.industryName;
                register.lotState = item.lotState;
                register.mode = item.mode;
                register.OrderId = item.OrderId;
                register.process = item.process;
                register.routeOrder = item.routeOrder;
                register.stageMode = item.stageMode;
                register.stageNumber = item.stageNumber;
                register.subCompanyId = item.subCompanyId;
                register.subCompanyName = item.subCompanyName;
                register.tenderNumber = item.tenderNumber;
                register.TenderUuid = item.TenderUuid;
                register.unloadAddress = item.unloadAddress;
                register.unloadDataRequired = item.unloadDataRequired;
                register.processValue = item.processValue;
                register.resultsTender = item.resultsTender;
                register.tenderOwnerPath = item.tenderOwnerPath;
                register.remainingTime = item.remainingTime;
                register.dateCreate = item.dateCreate;
                register.uuidFile = item.uuidFile;
                register.lotStateName = item.lotStateName;
                register.lotResultNote = item.lotResultNote;
                registerTenders.Add(register);
            }
            return registerTenders;
        }

        public List<RegisterTenders> GetRegisterTendersOfOrder(long orderId)
        {
            List<RegisterTenders> allRegisterTenders = new List<RegisterTenders>();
            var registerTendersList = db.RegisterTenders.OrderByDescending(x => x.dateEnd).ToList();
            foreach (var item in registerTendersList)
            {
                if (item.OrderId == orderId)
                {
                    RegisterTenders registerTenders = new RegisterTenders();
                    registerTenders.dateEnd = item.dateEnd;
                    registerTenders.dateStart = item.dateStart;
                    registerTenders.Id = item.Id;
                    registerTenders.processValue = item.processValue;
                    registerTenders.remainingTime = UpdateRegistersRemainingTime(item.tenderNumber);
                    registerTenders.TenderUuid = item.TenderUuid;
                    registerTenders.OrderId = item.OrderId;
                    registerTenders.stageNumber = item.stageNumber;
                    registerTenders.tenderNumber = item.tenderNumber;
                    registerTenders.cargoName = item.cargoName;
                    registerTenders.dateCreate = item.dateCreate;
                    registerTenders.dateUpdateStatus = item.dateUpdateStatus;
                    registerTenders.downloadAddress = item.downloadAddress;
                    registerTenders.downloadDataRequired = item.downloadDataRequired;
                    registerTenders.industryId = item.industryId;
                    registerTenders.industryName = item.industryName;
                    registerTenders.lotState = item.lotState;
                    registerTenders.mode = item.mode;
                    registerTenders.process = item.process;
                    registerTenders.resultsTender = item.resultsTender;
                    registerTenders.routeOrder = item.routeOrder;
                    registerTenders.stageMode = item.stageMode;
                    registerTenders.subCompanyId = item.subCompanyId;
                    registerTenders.subCompanyName = item.subCompanyName;
                    registerTenders.tenderOwnerPath = item.tenderOwnerPath;
                    registerTenders.unloadAddress = item.unloadAddress;
                    registerTenders.unloadDataRequired = item.unloadDataRequired;
                    registerTenders.uuidFile = item.uuidFile;
                    registerTenders.lotStateName = item.lotStateName;
                    registerTenders.lotResultNote = item.lotResultNote;
                    allRegisterTenders.Add(registerTenders);
                }
            }
            return allRegisterTenders;
        }
        public void UpdateStatusRegisterTender(int tenderNumber, int process, DateTime dateUpdateStatus, RequestJSONDeserializedToModel resultDeserializedClass)
        {
            try
            {
                var tender = db.RegisterTenders.Where(x => x.tenderNumber == tenderNumber).OrderByDescending(x => x.dateEnd).FirstOrDefault();
                tender.processValue = GetStatusTenders()[process];
                tender.dateUpdateStatus = dateUpdateStatus;
                db.SaveChanges();
                string result = null;
                int countWinner = 0;
                if (resultDeserializedClass != null)
                {
                    if (resultDeserializedClass.data.competitorList != null)
                    {
                        tender.uuidFile = Guid.Parse(resultDeserializedClass.data.competitorList.tenderFileUuid);
                    }
                    while (countWinner < resultDeserializedClass.data.lots[0].items.Count)
                    {
                        if (resultDeserializedClass.data.lots[0].items[countWinner].winner != null)
                        {
                            result += $"\nКонтрагент-победитель: {resultDeserializedClass.data.lots[0].items[countWinner].winner.ownershipType} \"{resultDeserializedClass.data.lots[0].items[countWinner].winner.name}\"" +
                                $" ({resultDeserializedClass.data.lots[0].items[countWinner].winner.country}, {resultDeserializedClass.data.lots[0].items[countWinner].winner.addressActual},\n" +
                                $"ЕДРПОУ: {resultDeserializedClass.data.lots[0].items[countWinner].winner.edrpou})\nКритерии:\n";
                            foreach (var item in resultDeserializedClass.data.lots[0].items[countWinner].criteriaValues)
                            {
                                result += $"{item.name}: {item.value};\n";
                            }
                        }
                        ++countWinner;
                    }
                    tender.process = (byte)process;
                    tender.stageNumber = (byte)resultDeserializedClass.data.stageNumber;
                    if (resultDeserializedClass.data.lots.Count != 0)
                    {
                        tender.lotState = resultDeserializedClass.data.lots[0].lotState;
                        tender.lotStateName = resultDeserializedClass.data.lots[0].lotStateName;
                        tender.lotResultNote = resultDeserializedClass.data.lots[0].lotResultNote;
                    }
                    tender.resultsTender = result;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        public Dictionary<int, string> GetStatusTenders()
        {
            Dictionary<int, string> statusTenders = new Dictionary<int, string>();
            var statusTendersList = db.StatusTenders.ToList();
            foreach (var item in statusTendersList)
            {
                statusTenders[item.statusID] = item.processValue;
            }
            return statusTenders;
        }

        public Dictionary<string, int> ShareTendersFromRegistyTenders()
        {
            Dictionary<int, int> shareTenders = new Dictionary<int, int>();
            Dictionary<string, int> shareTendersName = new Dictionary<string, int>();
            var registerTendersList = db.RegisterTenders.OrderByDescending(x => x.dateEnd).ToList();
            int countTenders = 0;
            foreach (var item in registerTendersList)
            {
                countTenders = (item.process != 9) ? ++countTenders : countTenders;
            }
            InitializeShareTenders(ref shareTenders, ref shareTendersName);
            foreach (var item in registerTendersList)
            {
                switch (item.process)
                {
                    case 1: shareTenders[item.process] = (shareTenders.ContainsKey(item.process)) ? ++shareTenders[item.process] : shareTenders[item.process] = 1; shareTendersName[item.processValue] = shareTenders[item.process]; break;
                    case 2: shareTenders[item.process] = (shareTenders.ContainsKey(item.process)) ? ++shareTenders[item.process] : shareTenders[item.process] = 1; shareTendersName[item.processValue] = shareTenders[item.process]; break;
                    case 3: shareTenders[item.process] = (shareTenders.ContainsKey(item.process)) ? ++shareTenders[item.process] : shareTenders[item.process] = 1; shareTendersName[item.processValue] = shareTenders[item.process]; break;
                    case 4: shareTenders[item.process] = (shareTenders.ContainsKey(item.process)) ? ++shareTenders[item.process] : shareTenders[item.process] = 1; shareTendersName[item.processValue] = shareTenders[item.process]; break;
                    case 5: shareTenders[item.process] = (shareTenders.ContainsKey(item.process)) ? ++shareTenders[item.process] : shareTenders[item.process] = 1; shareTendersName[item.processValue] = shareTenders[item.process]; break;
                    case 6: shareTenders[item.process] = (shareTenders.ContainsKey(item.process)) ? ++shareTenders[item.process] : shareTenders[item.process] = 1; shareTendersName[item.processValue] = shareTenders[item.process]; break;
                    case 7: shareTenders[item.process] = (shareTenders.ContainsKey(item.process)) ? ++shareTenders[item.process] : shareTenders[item.process] = 1; shareTendersName[item.processValue] = shareTenders[item.process]; break;
                    case 8: shareTenders[item.process] = (shareTenders.ContainsKey(item.process)) ? ++shareTenders[item.process] : shareTenders[item.process] = 1; shareTendersName[item.processValue] = shareTenders[item.process]; break;
                }
            }

            return shareTendersName;
        }
        private void InitializeShareTenders(ref Dictionary<int, int> shareTenders, ref Dictionary<string, int> shareTendersName)
        {
            var status = db.StatusTenders.ToList();
            for (int i = 0; i < status.Count - 1; i++)
            {
                shareTenders[i] = 0; shareTendersName[status[i].processValue] = 0;
            }
        }

        public bool IsRegisterTendersExist(long orderId, bool isMultipleTenders)
        {
            return (db.RegisterTenders.Count(x => x.OrderId == orderId) != 0) ? isMultipleTenders : true;
        }
        public OrderTruckTransport GetOrderTruckTransport(long orderId)
        {
            var orderTruckData = db.OrderTruckTransport.Where(x => x.OrderId == orderId).OrderByDescending(x => x.Id).FirstOrDefault();

            return new OrderTruckTransport()
            {
                Id = orderTruckData.Id,
                OrderId = orderTruckData.OrderId,
                BoxingDescription = orderTruckData.BoxingDescription,
                Consignee = orderTruckData.Consignee,
                ConsigneeAdress = orderTruckData.ConsigneeAdress,
                ConsigneeCity = orderTruckData.ConsigneeCity,
                ConsigneeContactPerson = orderTruckData.ConsigneeContactPerson,
                ConsigneeContactPersonPhone = orderTruckData.ConsigneeContactPersonPhone,
                ConsigneeCountryId = orderTruckData.ConsigneeCountryId,
                ConsigneeId = orderTruckData.ConsigneeId,
                DimenssionH = orderTruckData.DimenssionH,
                DimenssionL = orderTruckData.DimenssionL,
                DimenssionW = orderTruckData.DimenssionW,
                FromShipperDatetime = orderTruckData.FromShipperDatetime,
                LoadingTypeId = orderTruckData.LoadingTypeId,
                Shipper = orderTruckData.Shipper,
                ShipperAdress = orderTruckData.ShipperAdress,
                ShipperCity = orderTruckData.ShipperCity,
                ShipperContactPerson = orderTruckData.ShipperContactPerson,
                ShipperContactPersonPhone = orderTruckData.ShipperContactPersonPhone,
                ShipperCountryId = orderTruckData.ShipperCountryId,
                ShipperId = orderTruckData.ShipperId,
                ToConsigneeDatetime = orderTruckData.ToConsigneeDatetime,
                TripType = orderTruckData.TripType,
                TruckDescription = orderTruckData.TruckDescription,
                TruckTypeId = orderTruckData.TruckTypeId,
                UnloadingTypeId = orderTruckData.UnloadingTypeId,
                VehicleTypeId = orderTruckData.VehicleTypeId,
                Volume = orderTruckData.Volume,
                Weight = orderTruckData.Weight
            };
        }

        public void AddNewDataTender(RegisterTenders model)
        {
            var mainDataOrder = GetOrderTruckTransport(model.OrderId);
            try
            {
                db.RegisterTenders.Add(new Entity.RegisterTenders()
                {
                    OrderId = model.OrderId,
                    TenderUuid = model.TenderUuid,
                    dateEnd = model.dateEnd,
                    dateStart = model.dateStart,
                    industryId = model.industryId,
                    industryName = model.industryName,
                    mode = model.mode,
                    process = model.process,
                    stageMode = model.stageMode,
                    stageNumber = model.stageNumber,
                    subCompanyId = model.subCompanyId,
                    subCompanyName = model.subCompanyName,
                    tenderNumber = model.tenderNumber,
                    downloadAddress = model.downloadAddress,
                    unloadAddress = model.unloadAddress,
                    downloadDataRequired = model.downloadDataRequired,
                    unloadDataRequired = model.unloadDataRequired,
                    routeOrder = model.routeOrder,
                    cargoName = $"{ model.cargoName }\nГруз: {mainDataOrder.Weight}тн.",
                    lotState = model.lotState,
                    processValue = GetStatusTenders()[model.process],
                    resultsTender = model.resultsTender,
                    tenderOwnerPath = model.tenderOwnerPath,
                    remainingTime = RemainingCount(model.remainingTime),
                    dateCreate = model.dateCreate
                });
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }

        public void UpdateRegisterTenders(int tenderNumber, string results)
        {
            var tender = db.RegisterTenders.Where(x => x.tenderNumber == tenderNumber).OrderByDescending(x => x.Id).FirstOrDefault();
            tender.resultsTender = results;

            db.SaveChanges();
        }

        public void RemainingTime(Dictionary<string, Time> time)
        {
            if (time != null)
            {
                foreach (var item in time.Values)
                {
                    System.Guid tenderUuid = System.Guid.Parse(item.TenderUuid);
                    var tender = db.RegisterTenders.Where(x => x.TenderUuid == tenderUuid).OrderByDescending(x => x.Id).FirstOrDefault();
                    tender.remainingTime = item.remainingTime;
                    db.SaveChanges();
                }
            }
        }

        public string RemainingCount(string endDateTime)
        {
            string time = null;
            TimeSpan timeSpan = TimeSpan.Parse(endDateTime);
            try
            {
                double hours = 0d;
                double minutes = 0d;
                if (!(timeSpan.Ticks < 0))
                {
                    minutes = (Math.Round((timeSpan.TotalHours % 1) * 60, MidpointRounding.AwayFromZero) != 60) ? Math.Round((timeSpan.TotalHours % 1) * 60, MidpointRounding.AwayFromZero) : 0;
                    hours = (Math.Round((timeSpan.TotalHours % 1) * 60, MidpointRounding.AwayFromZero) != 60) ? timeSpan.TotalHours - (timeSpan.TotalHours % 1) : (timeSpan.TotalHours - (timeSpan.TotalHours % 1)) + 1;
                    time = $"{hours}ч : {minutes} м";
                }
                else
                {
                    time = "Завершен";
                }
            }
            catch (Exception e)
            {

            }
            return time;
        }

        public void UpdateTimeRemainingTime(RequestJSONDeserializedToModel myDeserializedClass, int numberTender)
        {
            DateTime nowDateTime = DateTime.Now;
            var tender = db.RegisterTenders.Where(x => x.tenderNumber == numberTender).OrderByDescending(x => x.Id).FirstOrDefault();
            tender.dateStart = myDeserializedClass.data.dateStart;
            tender.dateEnd = myDeserializedClass.data.dateEnd;
            tender.process = Byte.Parse(myDeserializedClass.data.process);
            TimeSpan timeSpan = myDeserializedClass.data.dateEnd - nowDateTime;
            tender.remainingTime = RemainingCount(timeSpan.ToString());

            db.SaveChanges();
        }

        public string UpdateRegistersRemainingTime(int tenderNumber)
        {
            DateTime nowDateTime = DateTime.Now;
            var tender = db.RegisterTenders.Where(x => x.tenderNumber == tenderNumber).OrderByDescending(x => x.Id).FirstOrDefault();
            TimeSpan timeSpan = tender.dateEnd - nowDateTime;
            var remainingT = RemainingCount(timeSpan.ToString());
            tender.remainingTime = remainingT;
            db.SaveChanges();
            return remainingT;
        }

        public UpdateRegisterStatusTender UpdateCLStatusTenderOrder(RequestJSONDeserializedToModel myDeserializedClass, int numberTender)
        {
            var registerTender = db.RegisterTenders.Where(x => x.tenderNumber == numberTender).OrderByDescending(x => x.Id).FirstOrDefault();
            UpdateRegisterStatusTender updateDeserializedClass = new UpdateRegisterStatusTender();
            DateTime dateStart = myDeserializedClass.data.dateStart;
            DateTime dateEnd = myDeserializedClass.data.dateEnd;
            string processValue = GetStatusTenders()[Int32.Parse(myDeserializedClass.data.process)];
            string remainingTime = UpdateRegistersRemainingTime(numberTender);
            byte stageNumber = (byte)myDeserializedClass.data.stageNumber;
            DateTime dateUpdateStatus = DateTime.Now;
            int lotState = myDeserializedClass.data.lots[0].lotState;
            byte process = Byte.Parse(myDeserializedClass.data.process);
            registerTender.dateEnd = dateEnd;
            registerTender.dateStart = dateStart;
            registerTender.processValue = processValue;
            registerTender.remainingTime = remainingTime;
            registerTender.stageNumber = stageNumber;
            registerTender.dateUpdateStatus = dateUpdateStatus;
            registerTender.lotState = lotState;
            registerTender.process = process;
            db.SaveChanges();
            string result = null;
            int countWinner = 0;
            if (myDeserializedClass != null)
            {
                if (myDeserializedClass.data.competitorList != null)
                {
                    registerTender.uuidFile = Guid.Parse(myDeserializedClass.data.competitorList.tenderFileUuid);
                }
                while (countWinner < myDeserializedClass.data.lots[0].items.Count)
                {
                    if (myDeserializedClass.data.lots[0].items[countWinner].winner != null)
                    {
                        result += $"\nКонтрагент-победитель: {myDeserializedClass.data.lots[0].items[countWinner].winner.ownershipType} \"{myDeserializedClass.data.lots[0].items[countWinner].winner.name}\"" +
                            $" ({myDeserializedClass.data.lots[0].items[countWinner].winner.country}, {myDeserializedClass.data.lots[0].items[countWinner].winner.addressActual},\n" +
                            $"ЕДРПОУ: {myDeserializedClass.data.lots[0].items[countWinner].winner.edrpou})\nКритерии:\n";
                        foreach (var item in myDeserializedClass.data.lots[0].items[countWinner].criteriaValues)
                        {
                            result += $"{item.name}: {item.value};\n";
                        }
                    }
                    ++countWinner;
                }
                registerTender.process = (byte)process;
                registerTender.stageNumber = (byte)myDeserializedClass.data.stageNumber;
                if (myDeserializedClass.data.lots.Count != 0)
                {
                    registerTender.lotState = myDeserializedClass.data.lots[0].lotState;
                    registerTender.lotStateName = myDeserializedClass.data.lots[0].lotStateName;
                    registerTender.lotResultNote = myDeserializedClass.data.lots[0].lotResultNote;
                }
                registerTender.resultsTender = result;
                db.SaveChanges();
            }
            updateDeserializedClass.dateEnd = dateEnd;
            updateDeserializedClass.dateStart = dateStart;
            updateDeserializedClass.processValue = processValue;
            updateDeserializedClass.remainingTime = remainingTime;
            updateDeserializedClass.stageNumber = stageNumber.ToString();
            updateDeserializedClass.dateUpdateStatus = dateUpdateStatus;
            updateDeserializedClass.lotState = lotState.ToString();
            updateDeserializedClass.process = process.ToString();
            updateDeserializedClass.resultsTender = result;
            updateDeserializedClass.lotStateName = myDeserializedClass.data.lots[0].lotStateName;
            updateDeserializedClass.lotResultNote = myDeserializedClass.data.lots[0].lotResultNote;
            return updateDeserializedClass;
        }

        public List<ContrAgentModel> GetAgentModels(List<RequestJSONContragentMainData> listRequestJSONContragent, RequestJSONContragentModel myDeserializedClassContragent)
        {
            List<ContrAgentModel> listContrAgents = new List<ContrAgentModel>();
            try
            {
                int count = 0;
                foreach (var item in listRequestJSONContragent)
                {
                    ContrAgentModel contrAgentModel = new ContrAgentModel()
                    {
                        SupplierId = item.Data.SupplierId,
                        SupplierEdrpou = item.Data.SupplierEdrpou,
                        OwnershipTypeId = item.Data.OwnershipTypeId,
                        OwnershipTypeName = item.Data.OwnershipTypeName,
                        SupplierName = item.Data.SupplierName,
                        RegistrationDate = item.Data.RegistrationDate,
                        WebSiteUrl = item.Data.WebSiteUrl,
                        ContactEmail = item.Data.ContactEmail,
                        ContactPhone = item.Data.ContactPhone,
                        LegalAddressContragent = new ContrAgentModel.LegalAddress()
                        {
                            CountryId = item.Data.LegalAddress.CountryId,
                            City = item.Data.LegalAddress.City,
                            CountryName = item.Data.LegalAddress.CountryName,
                            PostalCode = item.Data.LegalAddress.PostalCode,
                            RegionId = item.Data.LegalAddress.RegionId,
                            RegionName = item.Data.LegalAddress.RegionName,
                            Street = item.Data.LegalAddress.Street
                        },
                        PhysicalAddressContragent = new ContrAgentModel.PhysicalAddress()
                        {
                            Street = item.Data.PhysicalAddress.Street,
                            RegionName = item.Data.PhysicalAddress.RegionName,
                            RegionId = item.Data.PhysicalAddress.RegionId,
                            PostalCode = item.Data.PhysicalAddress.PostalCode,
                            CountryName = item.Data.PhysicalAddress.CountryName,
                            City = item.Data.PhysicalAddress.City,
                            CountryId = item.Data.PhysicalAddress.CountryId
                        },
                        CompanyTaxationContragent = new ContrAgentModel.CompanyTaxation()
                        {
                            CodeIPN = item.Data.CompanyTaxation.CodeIPN,
                            CodeVAT = item.Data.CompanyTaxation.CodeVAT,
                            TaxSystemId = item.Data.CompanyTaxation.TaxSystemId,
                            TaxSystemName = item.Data.CompanyTaxation.TaxSystemName
                        },
                        BankAccounts = new List<ContrAgentModel.BankAccount>(),
                        SupplierAccountantContragent = new ContrAgentModel.SupplierAccountant()
                        {
                            EMail = item.Data.SupplierAccountant.EMail,
                            FullName = item.Data.SupplierAccountant.FullName,
                            Phone = item.Data.SupplierAccountant.Phone
                        },
                        SupplierCEOContragent = new ContrAgentModel.SupplierCEO()
                        {
                            EMail = item.Data.SupplierCEO.EMail,
                            Phone = item.Data.SupplierCEO.Phone,
                            FullName = item.Data.SupplierCEO.FullName
                        },
                        listCritariaValues = new List<Criteriavalues>(),
                        SupplierUsers = new List<ContrAgentModel.SupplierUser>(),
                        RegDocsConsent = item.Data.RegDocsConsent,
                        StateId = item.Data.StateId,
                        StateName = item.Data.StateName,
                        SecurityStateId = item.Data.SecurityStateId,
                        SecurityStateName = item.Data.SecurityStateName,
                        CompanyType = item.Data.CompanyType,
                        ComplianceStateId = item.Data.ComplianceStateId,
                        ComplianceStateName = item.Data.ComplianceStateName
                    };
                    for (int i = 0; i < item.Data.BankAccounts.Count; i++)
                    {
                        contrAgentModel.BankAccounts.Add(new ContrAgentModel.BankAccount()
                        {
                            AccountNumber = item.Data.BankAccounts[i].AccountNumber,
                            BankMFO = item.Data.BankAccounts[i].BankMFO,
                            BankName = item.Data.BankAccounts[i].BankName
                        });
                        contrAgentModel.SupplierUsers.Add(new ContrAgentModel.SupplierUser()
                        {
                            CreateDate = item.Data.SupplierUsers[i].CreateDate,
                            EMail = item.Data.SupplierUsers[i].EMail,
                            FirstName = item.Data.SupplierUsers[i].FirstName,
                            IsAdmin = item.Data.SupplierUsers[i].IsAdmin,
                            IsBlocked = item.Data.SupplierUsers[i].IsBlocked,
                            IsContactPerson = item.Data.SupplierUsers[i].IsContactPerson,
                            IsDeleted = item.Data.SupplierUsers[i].IsDeleted,
                            LastName = item.Data.SupplierUsers[i].LastName,
                            MiddleName = item.Data.SupplierUsers[i].MiddleName,
                            Phone = item.Data.SupplierUsers[i].Phone,
                            SuppUserId = item.Data.SupplierUsers[i].SuppUserId,
                            UserId = item.Data.SupplierUsers[i].UserId
                        });
                        List<Criteriavalues> criteriaValues = new List<Criteriavalues>();
                        foreach (var items in myDeserializedClassContragent.Data[count].CriteriaValues)
                        {
                            criteriaValues.Add(new Criteriavalues()
                            {
                                Id = items.id,
                                Name = items.name,
                                Value = items.value
                            });
                        }
                        contrAgentModel.listCritariaValues = criteriaValues;
                    }
                    listContrAgents.Add(contrAgentModel);
                    count++;
                }
            }
            catch { return null; }

            return listContrAgents;
        }

        public void UpdateDataRegisterContragents(Dictionary<long, List<RegisterTenderContragent>> regisContragents)
        {
            foreach (var item in regisContragents)
            {
                var regContr = db.RegisterTenderContragents.Where(x => x.itemExternalNumber == item.Key).FirstOrDefault();
                if (regContr == null)
                {
                    foreach (var items in item.Value)
                    {
                        try
                        {
                            db.RegisterTenderContragents.Add(new Entity.RegisterTenderContragents()
                            {
                                OrderId = items.OrderId,
                                tenderNumber = items.tenderNumber,
                                itemExternalNumber = items.itemExternalNumber,
                                acceptedTransportUnits = items.acceptedTransportUnits,
                                ContragentIdAps = items.ContragentIdAps,
                                ContragentName = items.ContragentName,
                                costOfCarWithNDS = items.costOfCarWithNDS,
                                costOfCarWithoutNDS = items.costOfCarWithoutNDS,
                                DateUpdateInfo = items.DateUpdateInfo,
                                EDRPOUContragent = items.EDRPOUContragent,
                                emailContragent = items.emailContragent,
                                IsWinner = items.IsWinner,
                                nmcName = items.nmcName,
                                PaymentDelay = items.PaymentDelay,
                                tenderItemUuid = items.tenderItemUuid,
                                transportUnitsProposed = items.transportUnitsProposed
                            });
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
        }

        public ContrAgentModel GetWinnerContragent(List<ContrAgentModel> listAllContragents, int SupplierIdWinnerContragent)
        {
            return listAllContragents.Find(x => x.SupplierId == SupplierIdWinnerContragent);
        }
    }
}
