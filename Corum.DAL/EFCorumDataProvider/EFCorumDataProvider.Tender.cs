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
                    allRegisterTenders.Add(registerTenders);
                }
            }
            return allRegisterTenders;
        }
        public void UpdateStatusRegisterTender(int tenderNumber, int process, DateTime dateUpdateStatus)
        {
            var tender = db.RegisterTenders.Where(x => x.tenderNumber == tenderNumber).OrderByDescending(x => x.dateEnd).FirstOrDefault();
            tender.processValue = GetStatusTenders()[process];
            tender.dateUpdateStatus = dateUpdateStatus;
            db.SaveChanges();
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
            updateDeserializedClass.dateEnd = dateEnd;
            updateDeserializedClass.dateStart = dateStart;
            updateDeserializedClass.processValue = processValue;
            updateDeserializedClass.remainingTime = remainingTime;
            updateDeserializedClass.stageNumber = stageNumber.ToString();
            updateDeserializedClass.dateUpdateStatus = dateUpdateStatus;
            updateDeserializedClass.lotState = lotState.ToString();
            updateDeserializedClass.process = process.ToString();
            return updateDeserializedClass;
        }
    }
}
