﻿using System.Collections.Generic;
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
            var registerTendersList = db.RegisterTenders.ToList();
            //var statusValuesList = db.
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
                registerTenders.Add(register);
            }
            return registerTenders;
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
                    cargoName = model.cargoName,
                    lotState = model.lotState,
                    processValue = GetStatusTenders()[model.process],
                    resultsTender = model.resultsTender,
                    tenderOwnerPath = model.tenderOwnerPath
            });
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }
    }
}
