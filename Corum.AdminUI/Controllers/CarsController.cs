
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Corum.Common;
using Corum.Models;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Cars;
using CorumAdminUI.Common;
using CorumAdminUI.Helpers;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;
using System.IO;
using System.Net;
using Corum.Models.Toastr;

namespace CorumAdminUI.Controllers
{
    [Authorize]    
    public partial class CarsController  : CorumBaseController
    {
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CarOwners()
        {
            var carOwnersItems = context.GetCarOwnersTree();

            var model = new ManageCarOwnersAccessViewModel()
            {
                JSONData = JsonConvert.SerializeObject(carOwnersItems)
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult NewCarOwner(int? parentId)
        {
            var listEdrpou = context.GetEdrpouListAllContragents();
            var listEmails = context.GetEmailsListAllContragents();
            var carOwnersInfo = new CarOwnersAccessViewModel()
            {
                parentId = parentId,                 
                AvailableCarOwners = context.GetCarOwnersTree().ToList(),
                edrpouListAllContragents = listEdrpou,
                emailsListAllContragents = listEmails
            };

            return PartialView(carOwnersInfo);
        }

        [HttpPost]
        public ActionResult NewCarOwner(CarOwnersAccessViewModel model)
        {
            context.AddNewCarOwner(model);
            return RedirectToAction("CarOwners", "Cars");
        }

        [HttpGet]
        public ActionResult RemoveCarOwner(int carOwnerId)
        {
            context.RemoveCarOwner(carOwnerId);
            return RedirectToAction("CarOwners", "Cars");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateCarOwner(int carOwnerId)
        {
            var carOwnerInfo = context.GetCarOwner(carOwnerId);
            List<string> emails = new List<string>();
            emails = context.GetListEmails(carOwnerInfo.email_aps);
            var listEdrpou = context.GetEdrpouListAllContragents();
            var listEmails = context.GetEmailsListAllContragents();
            var carOwnerItem = new CarOwnersAccessViewModel()
            {
                Id = carOwnerInfo.Id,
                CarrierName = carOwnerInfo.CarrierName,
                Address = carOwnerInfo.Address,
                Phone = carOwnerInfo.Phone,
                ContactPerson = carOwnerInfo.ContactPerson,
                parentId = carOwnerInfo.parentId,
                IdForScript = carOwnerInfo.IdForScript,
                is_Leaf = carOwnerInfo.is_Leaf,
                isRoot = carOwnerInfo.isRoot,
                IsForwarder = carOwnerInfo.IsForwarder,
                AvailableCarOwners = context.GetCarOwnersTree().ToList(),
                emails = emails,
                email_aps = (emails.Count > 0) ? emails[0] : null,
                email_aps2 = (emails.Count > 1)? emails[1]: null,
                email_aps3 = (emails.Count > 2) ? emails[2] : null, 
                edrpou_aps = carOwnerInfo.edrpou_aps,
                edrpouListAllContragents = listEdrpou,
                emailsListAllContragents = listEmails
            };

            return View(carOwnerItem);
        }

        [HttpPost]
        public ActionResult UpdateCarOwner(CarOwnersAccessViewModel model)
        {
            if (model.email_aps2 == null) 
            {
                ModelState["email_aps2"].Errors.Clear();
            }
            if (model.email_aps3 == null)
            {
                ModelState["email_aps3"].Errors.Clear();
            }
            if (!ModelState.IsValid) return View(model);
            context.UpdateCarOwner(model);
            return RedirectToAction("CarOwners", "Cars");
        }


        public ActionResult Cars(int? carOwnerId)
        {
            GroupCarsViewModel model = null;
            if (carOwnerId != null)
            {
                model = new GroupCarsViewModel()
                {
                    GroupCarsInfo = context.GetCarOwner(carOwnerId),
                    Cars = context.GetCars(carOwnerId)
                };
            }

            return PartialView(model);
        }

        [HttpGet]
        public ActionResult NewCar(int carOwnerId)
        {
            var model = new CarsViewModel
            {
                CarOwnersId =  carOwnerId,
                FuelTypeList = context.GetCarsFuelType().ToList()

            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewCar(CarsViewModel model)
        {
            if (context.CheckCarNumber(model.Number))
                ModelState.AddModelError("Number", "Автомобиль может принадлежать только одному владельцу!");

            if (!ModelState.IsValid) return View();

            context.AddNewCar(model);
            return RedirectToAction("CarOwners", "Cars");
        }

        [HttpGet]
        public ActionResult RemoveCar(int carId)
        {
            context.RemoveCar(carId);
            return RedirectToAction("CarOwners", "Cars");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateCar(int carId, int carOwnerId)
        {
            var carInfo = context.getCar(carId);
            if (carInfo != null)
            {    carInfo.CarOwnersId = carOwnerId;
                 carInfo.FuelTypeList = context.GetCarsFuelType().ToList();
            }
            return View(carInfo);
        }

        [HttpPost]
        public ActionResult UpdateCar(CarsViewModel model)
        {
            if (context.CheckCarNumber2(model.Number, model.CarId))
               ModelState.AddModelError("Number", "Автомобиль может принадлежать только одному владельцу!");

            if (!ModelState.IsValid) return PartialView(model);
            context.UpdateCar(model);

            return RedirectToAction("CarOwners", "Cars");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateOwnerForCar(int carId, int carOwnerId)
        {
            var carInfo = context.getCar(carId);
            if (carInfo != null)
                carInfo.CarOwnersId = carOwnerId;

            var carOwnerInfo = context.GetCarOwner(carOwnerId);

            var carOwnerItem = new CarOwnersAccessViewModel()
            {
                Id = carOwnerInfo.Id,
                CarrierName = carOwnerInfo.CarrierName,
                Address = carOwnerInfo.Address,
                Phone = carOwnerInfo.Phone,
                ContactPerson = carOwnerInfo.ContactPerson,
                parentId = carOwnerInfo.parentId,
                IdForScript = carOwnerInfo.IdForScript,
                is_Leaf = carOwnerInfo.is_Leaf,
                isRoot = carOwnerInfo.isRoot,
                IsForwarder = carOwnerInfo.IsForwarder,
                AvailableCarOwners = context.GetLeafCarOwnersTree().ToList(),
            };

            var groupCar = new CarsGroupViewModel()
            {
                GroupCarsInfo = carOwnerItem,
                Cars = carInfo
            };

            return View(groupCar);
            
        }

        [HttpPost]
        public ActionResult UpdateOwnerForCar(CarsGroupViewModel model)
        {
            context.UpdateOwnerForCar(model);
            return RedirectToAction("CarOwners", "Cars");
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Contracts(ContractNavigationInfo navInfo, bool IsForwarder, bool IsMainMenu)
        {
            var model = new ContractNavigationResult<ContractsViewModel>(navInfo, userId)
            {
                isMainMenu = IsMainMenu,
                isForwarder =  IsForwarder              
            };
            if (IsMainMenu) {
                model.DisplayValues = context.GetAllContractsExpBK();
            } else
            {
                model.DisplayValues = context.GetCarOwnerContracts(navInfo.carOwnerId);
                model.carOwnerInfo = context.GetCarOwner(navInfo.carOwnerId);
            }
            model.isChrome = Request.Browser.Type.Contains("Chrome");
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteContract(int contractId, int carOwnersId, bool IsForwarder, bool IsMainMenu)
        {
            context.DeleteContract(contractId);
            return RedirectToAction("Contracts", "Cars", new { carOwnerId = carOwnersId, isForwarder = IsForwarder, isMainMenu = IsMainMenu });
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewContract(int? carOwnerId, bool isForwarder)
        {
            int year = DateTime.Now.Year;
            
             var expeditorName = "";
             int? parentId = null;
             if ((!isForwarder)&&(carOwnerId!=null)) { 
             var ownerInfo = context.GetCarOwner(carOwnerId);
             parentId = ownerInfo.parentId;
             var parentInfo = context.GetCarOwner(parentId);
              expeditorName = parentInfo.CarrierName;
             };

            var model = new ContractsViewModel
            {
                CarOwnersId = carOwnerId,
                GroupCarsInfo = context.GetCarOwner(carOwnerId),
                ContractDate = DateTime.Now.ToString("dd.MM.yyyy"),
                ContractDateRaw = DateTimeConvertClass.getString(DateTime.Now),
                DateBeg = new DateTime(year, 1, 1).ToString("dd.MM.yyyy"),
                DateBegRaw = DateTimeConvertClass.getString(new DateTime(year, 1, 1)),
                DateEnd = new DateTime(year, 12, 31).ToString("dd.MM.yyyy"),
                DateEndRaw = DateTimeConvertClass.getString(new DateTime(year, 12, 31)),
                ReceiveDateReal = DateTime.Now.ToString("dd.MM.yyyy"),
                ReceiveDateRealRaw = DateTimeConvertClass.getString(DateTime.Now),
                AvailableKeepers = context.getBalanceKeepers(userId).ToList(),
                IsForwarder = isForwarder,
                ExpeditorId = parentId,
                ExpeditorName = expeditorName,
                IsActive = true,
                IsMainMenu = (carOwnerId == null) ? true : false,
                AvailableCarOwners = context.GetLeafCarOwnersTree().ToList(),
                NDSTax = "0,00"
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewContract(ContractsViewModel model)
        {
            context.AddNewContract(model);
            return RedirectToAction("Contracts", "Cars", new { carOwnerId = model.CarOwnersId, isForwarder = model.IsForwarder, IsMainMenu = model.IsMainMenu });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateContract(int contractId, bool isMainMenu)
        {
            var contractInfo = context.getContract(contractId);
            if (contractInfo != null)
            {
                if (contractInfo.BalanceKeeperId != null && contractInfo.BalanceKeeperId != 0)
                {
                    contractInfo.IsForwarder = true;
                } else
                {
                    contractInfo.IsForwarder = false;
                    var ownerInfo = context.GetCarOwner(contractInfo.CarOwnersId);
                    contractInfo.ExpeditorId = ownerInfo.parentId;
                    var parentInfo = context.GetCarOwner(contractInfo.ExpeditorId);
                    contractInfo.ExpeditorName = parentInfo.CarrierName;
                }
                //contractInfo.AvailableKeepers = context.getBalanceKeepers(userId).ToList();
                contractInfo.GroupCarsInfo = context.GetCarOwner(contractInfo.CarOwnersId);
                contractInfo.IsMainMenu = isMainMenu;

            }
            return View(contractInfo);
        }

        [HttpPost]
        public ActionResult UpdateContract(ContractsViewModel model)
        {
            context.UpdateContract(model);

            return RedirectToAction("Contracts", "Cars", new { carOwnerId = model.CarOwnersId, isForwarder = model.IsForwarder, IsMainMenu = model.IsMainMenu });
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult getCarInfoById(int Id)
        {
            var CarInfo = context.getCar(Id);

            return Json(CarInfo, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Specifications(ContractSpecNavigationInfo navInfo, int groupeSpecId, bool IsMainMenu)
        {
            var GroupeSpecInfo = context.GetGroupeSpecification(groupeSpecId);
            var model = new ContractSpecNavigationResult<ContractSpecificationsViewModel>(navInfo, userId)
            {
                DisplayValues = context.GetContractSpecifications(groupeSpecId),
                groupeSpecInfo = GroupeSpecInfo,
                contractInfo = context.getContract(GroupeSpecInfo.ContractId),
                isMainMenu = IsMainMenu
            };

            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewSpecification(int groupeSpecId, bool isMainMenu)
        {
            int year = DateTime.Now.Year;

            var groupeSpecInfo = context.GetGroupeSpecification(groupeSpecId);
            var contractInfo = context.getContract(groupeSpecInfo.ContractId);

            var model = new ContractSpecificationsViewModel
            {
                CreateDateRaw = DateTimeConvertClass.getString(DateTime.Now),
                CreateDate = DateTime.Now.ToString("dd.MM.yyyy"),
                CreatedByUserName = this.displayUserName,
                MovingType = 1,
                CreatedByUser = this.userId,
                GroupeSpecInfo = groupeSpecInfo,
                RouteTypesInfo = context.GetRouteTypes().ToList(),
                CarryCapacityInfo = context.GetCarryCapacities().ToList(),
                IntervalTypesInfo = context.GetRouteIntervals().ToList(),
                ContractInfo = contractInfo,
                GroupeSpecId = groupeSpecId,
                RateHour = "0,00",
                RateKm = "0,00",
                RateMachineHour = "0,00",
                RateTotalFreight = "0,00",
                RouteLength = "0,00", 
                NDSTax = groupeSpecInfo.NDSTax,
                IsMainMenu = isMainMenu,
                TypeVehicleInfo = context.getVehicleTypes().ToList(),
                SpecTypeInfo = context.getSpecificationTypes().ToList(),
                TypeSpecId = 1
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewSpecification(ContractSpecificationsViewModel model)
        {
            context.AddSpecification(model);
            return RedirectToAction("Specifications", "Cars", new { groupeSpecId = model.GroupeSpecId, isMainMenu = model.IsMainMenu });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateSpecification(int specId, bool isMainMenu)
        {
            var specInfo = context.GetContractSpecification(specId);
            
            if (specInfo != null)
            {
                specInfo.GroupeSpecInfo = context.GetGroupeSpecification(specInfo.GroupeSpecId);
                specInfo.RouteTypesInfo = context.GetRouteTypes().ToList();
                specInfo.CarryCapacityInfo = context.GetCarryCapacities().ToList();
                specInfo.IntervalTypesInfo = context.GetRouteIntervals().ToList();
                specInfo.ContractInfo = context.getContract(specInfo.ContractId);
                specInfo.TypeVehicleInfo = context.getVehicleTypes().ToList();
                specInfo.SpecTypeInfo = context.getSpecificationTypes().ToList();
                specInfo.IsMainMenu = isMainMenu;

            }
            return View(specInfo);
        }

        [HttpPost]
        public ActionResult UpdateSpecification(ContractSpecificationsViewModel model)
        {
            context.UpdateSpecification(model);

            return RedirectToAction("Specifications", "Cars", new { groupeSpecId = model.GroupeSpecId, isMainMenu = model.IsMainMenu }); 
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteSpecification(int specId, int GroupeSpecId, bool IsMainMenu)
        {
            context.DeleteSpecification(specId);
            return RedirectToAction("Specifications", "Cars", new { groupeSpecId = GroupeSpecId, isMainMenu = IsMainMenu  });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GroupesSpecifications(GroupesSpecNavigationInfo navInfo, int contractId, bool IsMainMenu)
        {

            var model = new GroupesSpecNavigationResult<GroupesSpecificationsViewModel>(navInfo, userId)
            {
                DisplayValues = context.GetGroupesSpecifications(contractId),
                contractInfo = context.getContract(contractId),
                isMainMenu = IsMainMenu
            };

            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewGroupeSpecification(int contractId, bool isMainMenu)
        {
            int year = DateTime.Now.Year;
            var contractInfo = context.getContract(contractId);
            var model = new GroupesSpecificationsViewModel
            {
                DateBeg = contractInfo.DateBeg,
                DateBegRaw = contractInfo.DateBegRaw,
                DateEnd = contractInfo.DateEnd,
                DateEndRaw = contractInfo.DateEndRaw,
                CreateDateRaw = DateTimeConvertClass.getString(DateTime.Now),
                CreateDate = DateTime.Now.ToString("dd.MM.yyyy"),
                CreatedByUserName = this.displayUserName,
                CreatedByUser = this.userId,
                ContractInfo = contractInfo,
                IsActive = contractInfo.IsActive,
                DaysDelay = contractInfo.DaysDelay, 
                ContractId = contractId,
                IsMainMenu = isMainMenu,
                ExchangeRateUahRub = "0,0000",
                FuelPrice = "0,00",
                NDSTax = contractInfo.NDSTax
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewGroupeSpecification(GroupesSpecificationsViewModel model)
        {
            context.AddGroupeSpecification(model);
            return RedirectToAction("GroupesSpecifications", "Cars", new { contractId = model.ContractId, isMainMenu = model.IsMainMenu });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateGroupeSpecification(int groupeSpecId, bool isMainMenu)
        {
            var specInfo = context.GetGroupeSpecification(groupeSpecId);

            if (specInfo != null)
            {
                specInfo.ContractInfo = context.getContract(specInfo.ContractId);
                specInfo.IsMainMenu = isMainMenu;
            }
            return View(specInfo);
        }

        [HttpPost]
        public ActionResult UpdateGroupeSpecification(GroupesSpecificationsViewModel model)
        {
            context.UpdateGroupeSpecification(model);

            return RedirectToAction("GroupesSpecifications", "Cars", new { contractId = model.ContractId, isMainMenu = model.IsMainMenu });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteGroupeSpecification(int groupeSpecId, long ContractId, bool IsMainMenu)
        {
            context.DeleteGroupeSpecification(groupeSpecId);
            return RedirectToAction("GroupesSpecifications", "Cars", new { contractId = ContractId, isMainMenu = IsMainMenu });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CarryCapacities(NavigationInfo navInfo)
        {
            var model = new NavigationResult<CarryCapacitiesViewModel>(navInfo, userId)
            {
                DisplayValues = context.getCarryCapacities()

            };
            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewCarryCapacity()
        {
            var model = new CarryCapacitiesViewModel
            {
                MaxCapacity = "0,00",
                CarryCapacity = "0,00"
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewCarryCapacity(CarryCapacitiesViewModel model)
        {
            context.AddCarryCapacity(model);
            return RedirectToAction("CarryCapacities", "Cars", null);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateCarryCapacity(int capacityId)
        {
            var capacityInfo = context.GetCarryCapacity(capacityId);

            return View(capacityInfo);
        }

        [HttpPost]
        public ActionResult UpdateCarryCapacity(CarryCapacitiesViewModel model)
        {
            context.UpdateCarryCapacity(model);

            return RedirectToAction("CarryCapacities", "Cars", null);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteCarryCapacity(int capacityId)
        {
            context.DeleteCarryCapacity(capacityId);
            return RedirectToAction("CarryCapacities", "Cars", null);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult RouteIntervalTypes(NavigationInfo navInfo)
        {
            var model = new NavigationResult<RouteIntervalTypesViewModel>(navInfo, userId)
            {
                DisplayValues = context.getRouteIntervalTypes()

            };
            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewRouteIntervalType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewRouteIntervalType(RouteIntervalTypesViewModel model)
        {
            context.AddRouteIntervalType(model);
            return RedirectToAction("RouteIntervalTypes", "Cars", null);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateRouteIntervalType(int intervalId)
        {
            var intervalInfo = context.GetRouteIntervalType(intervalId);

            return View(intervalInfo);
        }

        [HttpPost]
        public ActionResult UpdateRouteIntervalType(RouteIntervalTypesViewModel model)
        {
            context.UpdateRouteIntervalType(model);

            return RedirectToAction("RouteIntervalTypes", "Cars", null);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteRouteIntervalType(int intervalId)
        {
            context.DeleteRouteIntervalType(intervalId);
            return RedirectToAction("RouteIntervalTypes", "Cars", null);
        }



        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SpecificationNames(NavigationInfo navInfo)
        {
            var model = new NavigationResult<SpecificationNamesViewModel>(navInfo, userId)
            {
                DisplayValues = context.getSpecificationNames()

            };
            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewSpecificationName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewSpecificationName(SpecificationNamesViewModel model)
        {
            context.AddSpecificationName(model);
            return RedirectToAction("SpecificationNames", "Cars", null);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateSpecificationName(int nameId)
        {
            var nameInfo = context.GetSpecificationName(nameId);

            return View(nameInfo);
        }

        [HttpPost]
        public ActionResult UpdateSpecificationName(SpecificationNamesViewModel model)
        {
            context.UpdateSpecificationName(model);

            return RedirectToAction("SpecificationNames", "Cars", null);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteSpecificationName(int nameId)
        {
            context.DeleteSpecificationName(nameId);
            return RedirectToAction("SpecificationNames", "Cars", null);
        }

        [HttpGet]
        public ActionResult GetSpecificationNames(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.SpecificationNames(searchTerm, pageSize, pageNum);
            var storagesCount = context.SpecificationNamesCount(searchTerm);

            var pagedAttendees = SpecificationNames2VmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult SpecificationNames2VmToSelect2Format(IEnumerable<SpecificationNamesViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                    text = string.Concat(groupItem.SpecCode + "-" + groupItem.SpecName)
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CloneSpecification(int specId, bool isMainMenu)
        {
            ContractSpecificationsViewModel specInfo = context.GetContractSpecification(specId);

            if (specInfo != null)
            {
                specInfo.GroupeSpecInfo = context.GetGroupeSpecification(specInfo.GroupeSpecId);
                specInfo.RouteTypesInfo = context.GetRouteTypes().ToList();
                specInfo.CarryCapacityInfo = context.GetCarryCapacities().ToList();
                specInfo.IntervalTypesInfo = context.GetRouteIntervals().ToList();
                specInfo.ContractInfo = context.getContract(specInfo.ContractId);
                specInfo.TypeVehicleInfo = context.getVehicleTypes().ToList();
                specInfo.SpecTypeInfo = context.getSpecificationTypes().ToList();
                specInfo.IsMainMenu = isMainMenu;
                specInfo.CreateDateRaw = DateTimeConvertClass.getString(DateTime.Now);
                specInfo.CreateDate = DateTime.Now.ToString("dd.MM.yyyy");
                specInfo.CreatedByUserName = this.displayUserName;
                specInfo.CreatedByUser = this.userId;
                context.AddSpecification(specInfo);
            }

            return RedirectToAction("Specifications", "Cars", new { groupeSpecId = specInfo.GroupeSpecId, isMainMenu = specInfo.IsMainMenu });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CloneGroupeSpecification(int groupeSpecId, bool isMainMenu, int? ContractsId)
        {
            GroupesSpecificationsViewModel specGroupeInfo = context.GetGroupeSpecification(groupeSpecId);

            if (specGroupeInfo != null)
            {
                specGroupeInfo.ContractInfo = context.getContract(specGroupeInfo.ContractId);
                specGroupeInfo.IsMainMenu = isMainMenu;
                if (ContractsId != null) specGroupeInfo.ContractId = (int)ContractsId;
            }
            var groupeSpecIdClone = context.AddGroupeSpecification(specGroupeInfo);
            if (groupeSpecIdClone > 0)
            { 
            List<ContractSpecificationsViewModel> specInfo = context.GetContractSpecifications(groupeSpecId).ToList();

            foreach (var spec in specInfo)
            {
                spec.GroupeSpecId = groupeSpecIdClone;
                spec.IsMainMenu = isMainMenu;
                spec.CreateDateRaw = DateTimeConvertClass.getString(DateTime.Now);
                spec.CreateDate = DateTime.Now.ToString("dd.MM.yyyy");
                spec.CreatedByUserName = this.displayUserName;
                spec.CreatedByUser = this.userId;
                context.AddSpecification(spec);
            }
            }
            return RedirectToAction("GroupesSpecifications", "Cars", new { contractId = specGroupeInfo.ContractId, isMainMenu = specGroupeInfo.IsMainMenu });
        
       }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult CloneContract(int contractId, bool isMainMenu)
        {
            var contractInfo = context.getContract(contractId);
            if (contractInfo != null)
            {
                if (contractInfo.BalanceKeeperId != null && contractInfo.BalanceKeeperId != 0)
                {
                    contractInfo.IsForwarder = true;
                }
                else
                {
                    contractInfo.IsForwarder = false;
                    var ownerInfo = context.GetCarOwner(contractInfo.CarOwnersId);
                    contractInfo.ExpeditorId = ownerInfo.parentId;
                    var parentInfo = context.GetCarOwner(contractInfo.ExpeditorId);
                    contractInfo.ExpeditorName = parentInfo.CarrierName;
                }

                contractInfo.GroupCarsInfo = context.GetCarOwner(contractInfo.CarOwnersId);
                contractInfo.IsMainMenu = isMainMenu;

            }
            return View(contractInfo);
        }


        [HttpPost]
        public ActionResult CloneContract(ContractsViewModel model)
        {
            var contractIdClone = context.AddNewContract(model);

            List<GroupesSpecificationsViewModel> specGroupeInfo = context.GetGroupesSpecifications(model.Id).ToList();
            if (specGroupeInfo != null)
            {
                foreach (var groupe in specGroupeInfo)
                {
                   groupe.ContractId = contractIdClone;
                   groupe.DateBeg = model.DateBeg;
                   groupe.DateBegRaw = model.DateBegRaw;
                   groupe.DateEnd = model.DateEnd;
                   groupe.DateEndRaw = model.DateEndRaw;
                   groupe.IsActive = model.IsActive;
                   groupe.DaysDelay = model.DaysDelay;
                   groupe.NDSTax = model.NDSTax;
                   var groupeSpecIdClone = context.AddGroupeSpecification(groupe);

                   if (groupeSpecIdClone > 0)
                   {
                    List<ContractSpecificationsViewModel> specInfo = context.GetContractSpecifications(groupe.Id).ToList();

                    foreach (var spec in specInfo)
                    {
                        spec.GroupeSpecId = groupeSpecIdClone;
                        spec.CreateDateRaw = DateTimeConvertClass.getString(DateTime.Now);
                        spec.CreateDate = DateTime.Now.ToString("dd.MM.yyyy");
                        spec.CreatedByUserName = this.displayUserName;
                        spec.CreatedByUser = this.userId;
                        context.AddSpecification(spec);
                    }
                }
            }
            }
            return RedirectToAction("Contracts", "Cars", new { carOwnerId = model.CarOwnersId, isForwarder = model.IsForwarder, IsMainMenu = model.IsMainMenu });
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetContracts(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            var storages = context.GetContracts(searchTerm, pageSize, pageNum, Id);
            var storagesCount = context.GetContractsCount(searchTerm, Id);

            var pagedAttendees = ContractsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult ContractsVmToSelect2Format(IEnumerable<ContractsViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.Id.ToString(),
                   // text = groupItem.ContractNumber + " " + groupItem.BalanceKeeperName
                    text = groupItem.BalanceKeeperName + " " + groupItem.ContractNumber + " от " + groupItem.ContractDate + " (с " + groupItem.DateBeg + " по " + groupItem.DateEnd + " ) "
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }
    }
}