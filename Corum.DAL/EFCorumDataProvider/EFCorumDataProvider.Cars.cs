using System;
using System.Collections.Generic;
using System.Linq;
using Corum.Models;
using Corum.Common;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Cars;
using Corum.Models.ViewModels.Orders;
using Corum.DAL.Mappings;
using Corum.DAL.Entity;
using System.Data.Entity.Validation;
using System.Globalization;

namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {
        public IEnumerable<CarOwnersAccessViewModel> GetCarOwnersTree()
        {
            List<CarOwnersAccessViewModel> carOwnersInfo = new List<CarOwnersAccessViewModel>();
            var carOwnersItems = db.CarOwners.ToList();

            foreach (var carOwnersItem in carOwnersItems)
            {
                CarOwnersAccessViewModel carOwnersInfoItem = new CarOwnersAccessViewModel();
                carOwnersInfoItem.Id = carOwnersItem.Id;
                carOwnersInfoItem.CarrierName = carOwnersItem.CarrierName;
                carOwnersInfoItem.Address = carOwnersItem.Address;
                carOwnersInfoItem.Phone = carOwnersItem.Phone;
                carOwnersInfoItem.ContactPerson = carOwnersItem.ContactPerson;
                carOwnersInfoItem.parentId = carOwnersItem.parentId;
                carOwnersInfoItem.IdForScript = carOwnersItem.Id;

                //если  нет children - элементов, то это лист
                carOwnersInfoItem.is_Leaf = !(CarOwnersHasChild(carOwnersItem.Id));

                var carOwnersCarsItem = db.Cars.Where(u => u.CarOwnersId == carOwnersInfoItem.Id);
                //можно удалять только если нет авто и если это корень, то нет детей от него
                if ((carOwnersCarsItem.Count() > 0) || (CarOwnersHasChild(carOwnersItem.Id)))
                    carOwnersInfoItem.CanBeDelete = false;
                else carOwnersInfoItem.CanBeDelete = true;
                carOwnersInfoItem.IsForwarder = (carOwnersItem.parentId == null) ? true : false;
                //if (carOwnersItem.IsForwarder == true)
                //    carOwnersInfoItem.IsForwarder = true;
                //else carOwnersInfoItem.IsForwarder = false;
                carOwnersInfo.Add(carOwnersInfoItem);

            }
            return carOwnersInfo;
        }

        //есть ли листья у элемента?
        public bool CarOwnersHasChild(int carOwnersId)
        {
            var childCarOwnersItems = db.CarOwners.Where(x => x.parentId == carOwnersId);

            if (childCarOwnersItems.Count() > 0)
                return true;
            else return false;
        }

        public void AddNewCarOwner(CarOwnersAccessViewModel model)
        {
            var carOwnersInfo = new CarOwners();

            if (carOwnersInfo != null)
            {
                carOwnersInfo.CarrierName = model.CarrierName;
                carOwnersInfo.Address = model.Address;
                carOwnersInfo.Phone = model.Phone;
                carOwnersInfo.ContactPerson = model.ContactPerson;
                carOwnersInfo.parentId = model.IsForwarder ? null : model.parentId;
                carOwnersInfo.IsForwarder = model.IsForwarder;
                /* if (model.IsForwarder == true)
                     carOwnersInfo.IsForwarder = true;
                 else carOwnersInfo.IsForwarder = false;*/

                db.CarOwners.Add(carOwnersInfo);
                db.SaveChanges();
            };
        }

        public void RemoveCarOwner(int carOwnerId)
        {
            var dbInfo = db.CarOwners.FirstOrDefault(u => u.Id == carOwnerId);
            if (dbInfo == null) return;

            db.CarOwners.Remove(dbInfo);
            db.SaveChanges();
        }

        public CarOwnersAccessViewModel GetCarOwner(int? carOwnerId)
        {
            if (carOwnerId != null)
            {
                var carOwnersItem = db.CarOwners.FirstOrDefault(u => u.Id == carOwnerId);

                CarOwnersAccessViewModel carOwnerInfoItem = new CarOwnersAccessViewModel();
                carOwnerInfoItem.Id = carOwnersItem.Id;
                carOwnerInfoItem.CarrierName = carOwnersItem.CarrierName;
                carOwnerInfoItem.Address = carOwnersItem.Address;
                carOwnerInfoItem.Phone = carOwnersItem.Phone;
                carOwnerInfoItem.ContactPerson = carOwnersItem.ContactPerson;
                carOwnerInfoItem.parentId = carOwnersItem.parentId;
                carOwnerInfoItem.IdForScript = carOwnersItem.Id;

                //если  нет children - элементов, то это лист
                carOwnerInfoItem.is_Leaf = !(CarOwnersHasChild(carOwnersItem.Id));


                var carOwnersCarsItem = db.Cars.Where(u => u.CarOwnersId == carOwnersItem.Id);
                //можно удалять только если нет авто и если это корень, то нет детей от него
                if ((carOwnersCarsItem.Count() > 0) || (CarOwnersHasChild(carOwnersItem.Id)))
                    carOwnerInfoItem.CanBeDelete = false;
                else carOwnerInfoItem.CanBeDelete = true;

                //carOwnerInfoItem.isRoot = (carOwnersItem.parentId == null) ? true : false;

                if (carOwnersItem.IsForwarder == true)
                    carOwnerInfoItem.IsForwarder = true;
                else carOwnerInfoItem.IsForwarder = false;
                return carOwnerInfoItem;
            }
            else return null;
        }
        public void UpdateCarOwner(CarOwnersAccessViewModel model)
        {
            var dbInfo = db.CarOwners.FirstOrDefault(u => u.Id == model.Id);
            if (dbInfo == null) return;

            dbInfo.CarrierName = model.CarrierName;
            dbInfo.Address = model.Address;
            dbInfo.Phone = model.Phone;
            dbInfo.ContactPerson = model.ContactPerson;
            dbInfo.parentId = model.IsForwarder ? null : model.parentId;
            dbInfo.IsForwarder = model.IsForwarder;
            db.SaveChanges();
        }


        public List<CarsViewModel> GetCars(int? carOwnerId)
        {
            List<CarsViewModel> cars = new List<CarsViewModel>();

            cars = (from R in db.Cars
                    where R.CarOwnersId == carOwnerId
                    select new CarsViewModel()
                    {
                        CarId = R.Id,
                        CarModel = R.Model,
                        Number = R.Number,
                        Driver = R.Driver,
                        DriverLicenseSeria = R.DriverLicenseSeria,
                        DriverLicenseNumber = R.DriverLicenseNumber,
                        FuelTypeId = R.FuelTypeId,
                        FuelType = db.CarsFuelType.FirstOrDefault(u => u.Id == R.FuelTypeId).FuelType,
                        ConsumptionCity = R.ConsumptionCity,
                        ConsumptionHighway = R.ConsumptionHighway,
                        PassNumber = R.PassNumber,
                        CarOwnersId = R.CarOwnersId
                        //Assigned = true

                    }).ToList();
            return cars.ToList();
        }

        public void AddNewCar(CarsViewModel model)
        {

            var carInfo = new Cars()
            {
                Model = model.CarModel,
                Number = model.Number,
                Driver = model.Driver,
                DriverLicenseSeria = model.DriverLicenseSeria,
                DriverLicenseNumber = model.DriverLicenseNumber,
                FuelTypeId = model.FuelTypeId,
                ConsumptionCity = model.ConsumptionCity,
                ConsumptionHighway = model.ConsumptionHighway,
                PassNumber = model.PassNumber,
                CarOwnersId = model.CarOwnersId
            };

            db.Cars.Add(carInfo);
            db.SaveChanges();

        }

        public void RemoveCar(int carId)
        {
            var dbInfo = db.Cars.FirstOrDefault(u => u.Id == carId);
            if (dbInfo == null) return;

            db.Cars.Remove(dbInfo);
            db.SaveChanges();
        }

        public CarsViewModel getCar(int carId)
        {
            return Mapper.Map(db.Cars.AsNoTracking().FirstOrDefault(u => u.Id == carId));
        }

        public void UpdateCar(CarsViewModel model)
        {
            var dbInfo = db.Cars.FirstOrDefault(u => u.Id == model.CarId);
            if (dbInfo == null) return;


            dbInfo.Model = model.CarModel;
            dbInfo.Number = model.Number;
            dbInfo.Driver = model.Driver;
            dbInfo.DriverLicenseSeria = model.DriverLicenseSeria;
            dbInfo.DriverLicenseNumber = model.DriverLicenseNumber;
            dbInfo.FuelTypeId = model.FuelTypeId;
            dbInfo.ConsumptionCity = model.ConsumptionCity;
            dbInfo.ConsumptionHighway = model.ConsumptionHighway;
            dbInfo.PassNumber = model.PassNumber;
            dbInfo.CarOwnersId = model.CarOwnersId;

            db.SaveChanges();
        }

        public bool CheckCarNumber(string Number)
        {
            return db.Cars.Any(u => u.Number == Number);
        }

        public bool CheckCarNumber2(string Number, int carId)
        {
            return db.Cars.Any(u => u.Number == Number && u.Id != carId);
        }

        public void UpdateOwnerForCar(CarsGroupViewModel model)
        {

            var dbInfo = db.Cars.FirstOrDefault(u => u.Id == model.Cars.CarId);
            if (dbInfo == null) return;

            dbInfo.CarOwnersId = model.GroupCarsInfo.parentId;

            db.SaveChanges();
        }

        public IEnumerable<CarOwnersAccessViewModel> GetLeafCarOwnersTree()
        {
            List<CarOwnersAccessViewModel> groupRoleInfo = new List<CarOwnersAccessViewModel>();
            var groupRoleItems = db.CarOwners.ToList();

            foreach (var carOwnersItem in groupRoleItems)
            {
                if (!(CarOwnersHasChild(carOwnersItem.Id)))
                {
                    CarOwnersAccessViewModel carOwnerInfoItem = new CarOwnersAccessViewModel();
                    carOwnerInfoItem.Id = carOwnersItem.Id;
                    carOwnerInfoItem.CarrierName = carOwnersItem.CarrierName;
                    carOwnerInfoItem.Address = carOwnersItem.Address;
                    carOwnerInfoItem.Phone = carOwnersItem.Phone;
                    carOwnerInfoItem.ContactPerson = carOwnersItem.ContactPerson;
                    carOwnerInfoItem.parentId = carOwnersItem.parentId;
                    carOwnerInfoItem.IdForScript = carOwnersItem.Id;

                    if (carOwnersItem.IsForwarder == true)
                        carOwnerInfoItem.IsForwarder = true;
                    else carOwnerInfoItem.IsForwarder = false;

                    //если  нет children - элементов, то это лист
                    carOwnerInfoItem.is_Leaf = !(CarOwnersHasChild(carOwnersItem.Id));

                    var carOwnersCarsItem = db.Cars.Where(u => u.CarOwnersId == carOwnersItem.Id);
                    //можно удалять только если нет ролей и если это корень, то нет детей от него
                    if ((carOwnersCarsItem.Count() > 0) || (CarOwnersHasChild(carOwnersItem.Id)))
                        carOwnerInfoItem.CanBeDelete = false;
                    else carOwnerInfoItem.CanBeDelete = true;
                   // carOwnerInfoItem.isRoot = (carOwnersItem.parentId == null) ? true : false;
                    groupRoleInfo.Add(carOwnerInfoItem);
                }
            }
            return groupRoleInfo;
        }

        public IQueryable<CarsFuelTypeViewModel> GetCarsFuelType()
        {
            return db.CarsFuelType
                            .AsNoTracking()
                             .Select(Mapper.Map)
                               .AsQueryable();
        }


        public IQueryable<ContractsViewModel> GetCarOwnerContracts(int? carOwnerId)
        {
            List<ContractsViewModel> contractsInfo = new List<ContractsViewModel>();
            var contractsItems = db.Contracts.Where(x => x.CarOwnersId == carOwnerId).ToList();
            string ContractDate = "";
            string DateBeg = "";
            string DateEnd = "";
            string ReceiveDateReal = "";
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateTimeInMonth = DateTime.Now.AddDays(30);

            foreach (var contractsItem in contractsItems)
            {   
                ContractsViewModel contrItem = new ContractsViewModel();

                if (contractsItem.ContractDate != null) ContractDate = contractsItem.ContractDate.Value.ToString("dd.MM.yyyy");
                if (contractsItem.DateBeg != null) DateBeg = contractsItem.DateBeg.Value.ToString("dd.MM.yyyy");
                if (contractsItem.DateEnd != null) DateEnd = contractsItem.DateEnd.Value.ToString("dd.MM.yyyy");
                if (contractsItem.ReceiveDateReal != null)
                {
                    ReceiveDateReal = contractsItem.ReceiveDateReal.Value.ToString("dd.MM.yyyy");
                } else
                {
                    ReceiveDateReal = "";
                }

                contrItem.Id = contractsItem.Id;
                contrItem.CarOwnersId = contractsItem.CarOwnersId;
                contrItem.BalanceKeeperId = contractsItem.BalanceKeeperId;
                contrItem.ContractNumber = contractsItem.ContractNumber;
                contrItem.ContractDate = ContractDate;
                contrItem.ContractDateRaw = DateTimeConvertClass.getString(contractsItem.ContractDate.Value);
                contrItem.DateBegRaw = DateTimeConvertClass.getString(contractsItem.DateBeg.Value);
                contrItem.DateEndRaw = DateTimeConvertClass.getString(contractsItem.DateEnd.Value);
                if (contractsItem.ReceiveDateReal != null)
                { contrItem.ReceiveDateRealRaw = DateTimeConvertClass.getString(contractsItem.ReceiveDateReal.Value); }
          
                contrItem.DateBeg = DateBeg;
                contrItem.DateEnd = DateEnd;
                contrItem.ReceiveDateReal = ReceiveDateReal;
                contrItem.IsActive = contractsItem.IsActive ?? true;
                contrItem.DaysDelay = contractsItem.DaysDelay ?? 0;
                contrItem.NDSTax = (contractsItem.NDSTax ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA"));
                contrItem.ContractRevision = contractsItem.ContractRevision;
                if (contractsItem.BalanceKeeperId != null)
                contrItem.BalanceKeeperName = db.BalanceKeepers.FirstOrDefault(p => p.Id == contractsItem.BalanceKeeperId).BalanceKeeper ?? "";
                contrItem.ExpeditorId = contractsItem.ExpeditorId ?? 0;
                if (contrItem.ExpeditorId != 0)
                    contrItem.ExpeditorName = db.CarOwners.FirstOrDefault(p => p.Id == contrItem.ExpeditorId).CarrierName??"";

                DateTime dateEnd = contractsItem.DateEnd ?? DateTime.MinValue;
                
                if (dateEnd < DateTime.Now)
                {
                    contrItem.BackgroundColor = "#FF0000";
                } else
                if ((dateEnd - dateTimeInMonth).TotalDays <= 30)
                {
                    contrItem.BackgroundColor = "#FFCC99";
                }


                contrItem.CanBeDelete = !((contractsItem.OrderUsedCars.Count() > 0) || (contractsItem.OrderUsedCars1.Count() > 0) || (contractsItem.ContractGroupesSpecifications.Count() > 0));
                contractsInfo.Add(contrItem);

            }
            return contractsInfo.AsQueryable();
        }

        public IQueryable<ContractsViewModel> GetAllContractsExpBK()
        {
            return db.Contracts         
                            .AsNoTracking()
                             .Where(s => s.BalanceKeeperId != null)
                             .Select(Mapper.Map)
                               .AsQueryable();
        }

        public bool DeleteContract(int id)
        {
            var contractInfo = db.Contracts.FirstOrDefault(o => o.Id == id);

            if (contractInfo != null)
            {
                db.Contracts.Remove(contractInfo);
                db.SaveChanges();
            }
            return true;

        }

        public int AddNewContract(ContractsViewModel model)
        {

            var contractInfo = new Contracts()
            {
                CarOwnersId = model.CarOwnersId,
                BalanceKeeperId = model.BalanceKeeperId,
                ExpeditorId = model.ExpeditorId,
                ContractNumber = model.ContractNumber,
                ContractDate = DateTimeConvertClass.getDateTime(model.ContractDateRaw),
                DateBeg = DateTimeConvertClass.getDateTime(model.DateBegRaw),
                DateEnd = DateTimeConvertClass.getDateTime(model.DateEndRaw),
                ReceiveDateReal = DateTimeConvertClass.getDateTime(model.ReceiveDateRealRaw),
                IsActive = model.IsActive,
                DaysDelay = model.DaysDelay,
                ContractRevision = model.ContractRevision,
                NDSTax = Convert.ToDecimal(model.NDSTax.Replace(".", ","))

            };

            db.Contracts.Add(contractInfo);
            db.SaveChanges();

            return contractInfo.Id;
        }

        public void UpdateContract(ContractsViewModel model)
        {
            var contractInfo = db.Contracts.FirstOrDefault(u => u.Id == model.Id);
            if (contractInfo == null) return;

            contractInfo.CarOwnersId = model.CarOwnersId;
            contractInfo.BalanceKeeperId = model.BalanceKeeperId;
            contractInfo.ExpeditorId = model.ExpeditorId;
            contractInfo.ContractNumber = model.ContractNumber;
            contractInfo.ContractDate = DateTimeConvertClass.getDateTime(model.ContractDateRaw);
            contractInfo.DateBeg = DateTimeConvertClass.getDateTime(model.DateBegRaw);
            contractInfo.DateEnd = DateTimeConvertClass.getDateTime(model.DateEndRaw);
            contractInfo.ReceiveDateReal = DateTimeConvertClass.getDateTime(model.ReceiveDateRealRaw);
            contractInfo.IsActive = model.IsActive;
            contractInfo.DaysDelay = model.DaysDelay;
            contractInfo.ContractRevision = model.ContractRevision;
            contractInfo.NDSTax = Convert.ToDecimal(model.NDSTax.Replace(".", ","));


            db.SaveChanges();
        }

      
        public ContractsViewModel getContract(int contractId)
        {
            return Mapper.Map(db.Contracts.AsNoTracking().FirstOrDefault(u => u.Id == contractId));
        }

        public IQueryable<ContractSpecificationsViewModel> GetContractSpecifications(int groupeSpecId)
        {
            
                var Contracts = db.ContractSpecifications
                .AsNoTracking()
                 .Where(u => u.GroupeSpecId == groupeSpecId)
                  .Select(Mapper.Map);
            
            int i = 1;
            List<ContractSpecificationsViewModel> contracts = new List<ContractSpecificationsViewModel>();
            foreach (var c in Contracts)
            {
                ContractSpecificationsViewModel contract = new ContractSpecificationsViewModel();
                contract.CountRows = i;
                contract.Id = c.Id;
                contract.GroupeSpecId = c.GroupeSpecId;
                contract.CreatedByUser = c.CreatedByUser;
                contract.CreatedByUserName = c.CreatedByUserName;
                contract.CreateDate = c.CreateDate;
                contract.CreateDateRaw = c.CreateDateRaw;
                contract.CarryCapacityId = c.CarryCapacityId;
                contract.CarryCapacityVal = c.CarryCapacityVal;
                contract.DeparturePoint = c.DeparturePoint;
                contract.ArrivalPoint = c.ArrivalPoint;
                contract.RouteLength = c.RouteLength;
                contract.MovingType = c.MovingType;
                contract.MovingTypeName = c.MovingTypeName;
                contract.RouteTypeId = c.RouteTypeId;
                contract.RouteTypeName = c.RouteTypeName;
                contract.IntervalTypeId = c.IntervalTypeId;
                contract.IntervalTypeName = c.IntervalTypeName;
                contract.RateKm = c.RateKm;
                contract.RateHour = c.RateHour;
                contract.RateMachineHour = c.RateMachineHour;
                contract.RateTotalFreight = c.RateTotalFreight;
                contract.NDSTax = c.NDSTax;
                contract.RouteId = c.RouteId;
                contract.IsTruck = c.IsTruck;
                contract.IsPriceNegotiated = c.IsPriceNegotiated;
                contract.TypeSpecId = c.TypeSpecId;
                contract.TypeSpecName = c.TypeSpecName;
                contract.NameId = c.NameId;
                contract.NameSpecification = c.NameSpecification;
                contract.TypeVehicleId = c.TypeVehicleId;
                contract.TypeVehicleName = c.TypeVehicleName;
                contract.RouteName = c.RouteName;
                contract.ContractId = c.ContractId;

                //вычисляем GenId                
                //фрахт
                if (c.TypeSpecId == 1) contract.GenId = 1;
                //дог. цена
               else if (c.IsPriceNegotiated) contract.GenId = 5;
                //иначе - считаем что тариф
               else contract.GenId = 2;

                contracts.Add(contract);
                i++;
            }
            return contracts.AsQueryable();
        }

        public ContractSpecificationsViewModel GetContractSpecification(int specId)
        {
            return Mapper.Map(db.ContractSpecifications.AsNoTracking().FirstOrDefault(u => u.Id == specId));
        }

        public IQueryable<CarryCapacitiesViewModel> GetCarryCapacities()
        {
            return db.CarCarryCapacity
                .AsNoTracking()
                  .Select(Mapper.Map)
                  .OrderBy(o => o.CarryCapacity)
                   .AsQueryable();
        }

        public IQueryable<RouteIntervalTypesViewModel> GetRouteIntervals()
        {
            return db.RouteIntervalType
                .AsNoTracking()
                  .Select(Mapper.Map)
                  .OrderBy(o => o.IntervalTypeName)
                   .AsQueryable();
        }

        public IQueryable<RouteTypesViewModel> GetRouteTypes()
        {
            return db.RouteTypes
                .AsNoTracking()
                  .Select(Mapper.Map)
                  .OrderBy(o => o.Id)
                   .AsQueryable();
        }

        public void AddSpecification(ContractSpecificationsViewModel model)
        {
          
            var specInfo = new ContractSpecifications()
            {
                CreatedByUser = model.CreatedByUser,
                CreateDate = DateTimeConvertClass.getDateTime(model.CreateDateRaw),
                CarryCapacityId = model.CarryCapacityId,
                DeparturePoint =model.DeparturePoint,
                ArrivalPoint = model.ArrivalPoint,
                RouteLength = Convert.ToDecimal(model.RouteLength.Replace(".", ",")),
                IntervalTypeId = model.IntervalTypeId,
                RouteTypeId = model.RouteTypeId,
                RateKm = Convert.ToDecimal(model.RateKm.Replace(".", ",")),
                RateHour = Convert.ToDecimal(model.RateHour.Replace(".", ",")),
                RateMachineHour = Convert.ToDecimal(model.RateMachineHour.Replace(".", ",")),
                RateTotalFreight = Convert.ToDecimal(model.RateTotalFreight.Replace(".", ",")),
                NDSTax = Convert.ToDecimal(model.NDSTax.Replace(".", ",")),
                GroupeSpecId = model.GroupeSpecId,
                RouteId = model.RouteId,
                IsTruck = model.IsTruck, 
                IsPriceNegotiated = model.IsPriceNegotiated,
                TypeVehicleId = model.TypeVehicleId,
                TypeSpecId = model.TypeSpecId,
                NameId = model.NameId,
                RouteName = model.RouteName
            
                        
            };

            db.ContractSpecifications.Add(specInfo);
            db.SaveChanges();
        }

        public bool DeleteSpecification(int id)
        {
            var specInfo = db.ContractSpecifications.FirstOrDefault(o => o.Id == id);

            if (specInfo != null)
            {
                db.ContractSpecifications.Remove(specInfo);
                db.SaveChanges();
            }
            return true;

        }

        public void UpdateSpecification(ContractSpecificationsViewModel model)
        {

            var specInfo = db.ContractSpecifications.FirstOrDefault(o => o.Id == model.Id);

            if (specInfo == null) return;

            specInfo.CreatedByUser = model.CreatedByUser;
            specInfo.CreateDate = DateTimeConvertClass.getDateTime(model.CreateDateRaw);
            specInfo.CarryCapacityId = model.CarryCapacityId;
            specInfo.DeparturePoint = model.DeparturePoint;
            specInfo.ArrivalPoint = model.ArrivalPoint;
            specInfo.RouteLength = Convert.ToDecimal(model.RouteLength.Replace(".", ","));
            specInfo.IntervalTypeId = model.IntervalTypeId;
            specInfo.MovingType = model.MovingType;
            specInfo.RouteTypeId = model.RouteTypeId;
            specInfo.RateKm = Convert.ToDecimal(model.RateKm.Replace(".", ","));
            specInfo.RateHour = Convert.ToDecimal(model.RateHour.Replace(".", ","));
            specInfo.RateMachineHour = Convert.ToDecimal(model.RateMachineHour.Replace(".", ","));
            specInfo.RateTotalFreight = Convert.ToDecimal(model.RateTotalFreight.Replace(".", ","));
            specInfo.NDSTax = Convert.ToDecimal(model.NDSTax.Replace(".", ","));
            specInfo.GroupeSpecId = model.GroupeSpecId;
            specInfo.RouteId = model.RouteId;
            specInfo.IsTruck = model.IsTruck;
            specInfo.IsPriceNegotiated = model.IsPriceNegotiated;
            specInfo.TypeVehicleId = model.TypeVehicleId;
            specInfo.TypeSpecId = model.TypeSpecId;
            specInfo.NameId = model.NameId;
            specInfo.RouteName = model.RouteName;

            db.SaveChanges();
        }

        public IQueryable<GroupesSpecificationsViewModel> GetGroupesSpecifications(int contractId)
        {
            return db.ContractGroupesSpecifications
                .AsNoTracking()
                 .Where(u => u.ContractId == contractId)
                  .Select(Mapper.Map)
                   .AsQueryable();

        }

        public int AddGroupeSpecification(GroupesSpecificationsViewModel model)
        {

            var specInfo = new ContractGroupesSpecifications()
            {
                CreatedByUser = model.CreatedByUser,
                DateBeg = DateTimeConvertClass.getDateTime(model.DateBegRaw),
                DateEnd = DateTimeConvertClass.getDateTime(model.DateEndRaw),
                CreateDate = DateTimeConvertClass.getDateTime(model.CreateDateRaw),
                NameGroupSpec = model.NameGroupeSpecification,
                DaysDelay = model.DaysDelay,
                IsActive = model.IsActive,
                ContractId = model.ContractId,
                FuelPrice = Convert.ToDecimal(model.FuelPrice.Replace(".", ",")),
                ExchangeRateUahRub = Convert.ToDecimal(model.ExchangeRateUahRub.Replace(".", ",")),
                NDSTax = Convert.ToDecimal(model.NDSTax.Replace(".", ","))

            };

            db.ContractGroupesSpecifications.Add(specInfo);
            
            db.SaveChanges();

            return specInfo.Id;
        }

        public GroupesSpecificationsViewModel GetGroupeSpecification(int groupeSpecId)
        {
            return Mapper.Map(db.ContractGroupesSpecifications.AsNoTracking().FirstOrDefault(u => u.Id == groupeSpecId));
        }

        public bool DeleteGroupeSpecification(int id)
        {
            var specInfo = db.ContractGroupesSpecifications.FirstOrDefault(o => o.Id == id);

            if (specInfo != null)
            {
                db.ContractGroupesSpecifications.Remove(specInfo);
                db.SaveChanges();
            }
            return true;

        }

        public void UpdateGroupeSpecification(GroupesSpecificationsViewModel model)
        {

            var specInfo = db.ContractGroupesSpecifications.FirstOrDefault(o => o.Id == model.Id);

            if (specInfo == null) return;

            specInfo.CreatedByUser = model.CreatedByUser;
            specInfo.DateBeg = DateTimeConvertClass.getDateTime(model.DateBegRaw);
            specInfo.DateEnd = DateTimeConvertClass.getDateTime(model.DateEndRaw);
            specInfo.CreateDate = DateTimeConvertClass.getDateTime(model.CreateDateRaw);
            specInfo.NameGroupSpec = model.NameGroupeSpecification;
            specInfo.DaysDelay = model.DaysDelay;
            specInfo.IsActive = model.IsActive;
            specInfo.ContractId = model.ContractId;
            specInfo.FuelPrice = Convert.ToDecimal(model.FuelPrice.Replace(".", ","));
            specInfo.ExchangeRateUahRub = Convert.ToDecimal(model.ExchangeRateUahRub.Replace(".", ","));
            specInfo.NDSTax = Convert.ToDecimal(model.NDSTax.Replace(".", ","));

            db.SaveChanges();
        }

        public IQueryable<CarryCapacitiesViewModel> getCarryCapacities()
        {
            return db.CarCarryCapacity.AsNoTracking().Select(Mapper.Map).AsQueryable();
        }

        public void AddCarryCapacity(CarryCapacitiesViewModel model)
        {

            var capacityInfo = new CarCarryCapacity()
            {
                CarryCapacity =  Convert.ToDecimal(model.CarryCapacity.Replace(".", ",")),
                MaxCapacity = Convert.ToDecimal(model.MaxCapacity.Replace(".", ",")),
                CapacityComment = model.CommentCapacity
            };

            db.CarCarryCapacity.Add(capacityInfo);
            db.SaveChanges();
        }

        public bool DeleteCarryCapacity(int id)
        {
            var capacityInfo = db.CarCarryCapacity.FirstOrDefault(o => o.Id == id);

            if (capacityInfo != null)
            {
                db.CarCarryCapacity.Remove(capacityInfo);
                db.SaveChanges();
            }
            return true;

        }

        public void UpdateCarryCapacity(CarryCapacitiesViewModel model)
        {

            var capacityInfo = db.CarCarryCapacity.FirstOrDefault(o => o.Id == model.Id);

            if (capacityInfo == null) return;

            capacityInfo.CarryCapacity = Convert.ToDecimal(model.CarryCapacity.Replace(".", ","));
            capacityInfo.MaxCapacity = Convert.ToDecimal(model.MaxCapacity.Replace(".", ","));
            capacityInfo.CapacityComment = model.CommentCapacity;

            db.SaveChanges();
        }

        public CarryCapacitiesViewModel GetCarryCapacity(int capacityId)
        {
            return Mapper.Map(db.CarCarryCapacity.AsNoTracking().FirstOrDefault(u => u.Id == capacityId));
        }

        public IQueryable<RouteIntervalTypesViewModel> getRouteIntervalTypes()
        {
            return db.RouteIntervalType.AsNoTracking().Select(Mapper.Map).AsQueryable();
        }

        public void AddRouteIntervalType(RouteIntervalTypesViewModel model)
        {

            var intervalInfo = new RouteIntervalType()
            {
                MaxInterval = model.MaxInterval,
                NameIntervalType = model.IntervalTypeName
            };

            db.RouteIntervalType.Add(intervalInfo);
            db.SaveChanges();
        }

        public bool DeleteRouteIntervalType(int id)
        {
            var intervalInfo = db.RouteIntervalType.FirstOrDefault(o => o.Id == id);

            if (intervalInfo != null)
            {
                db.RouteIntervalType.Remove(intervalInfo);
                db.SaveChanges();
            }
            return true;

        }

        public void UpdateRouteIntervalType(RouteIntervalTypesViewModel model)
        {

            var intervalInfo = db.RouteIntervalType.FirstOrDefault(o => o.Id == model.Id);

            if (intervalInfo == null) return;

            intervalInfo.MaxInterval = model.MaxInterval;
            intervalInfo.NameIntervalType = model.IntervalTypeName;

            db.SaveChanges();
        }

        public RouteIntervalTypesViewModel GetRouteIntervalType(int intervalId)
        {
            return Mapper.Map(db.RouteIntervalType.AsNoTracking().FirstOrDefault(u => u.Id == intervalId));
        }

        public IQueryable<SpecificationNamesViewModel> getSpecificationNames()
        {
            return db.SpecificationNames.AsNoTracking().Select(Mapper.Map).AsQueryable();
        }

        public void AddSpecificationName(SpecificationNamesViewModel model)
        {

            var nameInfo = new SpecificationNames()
            {
                SpecName = model.SpecName,
                SpecCode = model.SpecCode
            };

            db.SpecificationNames.Add(nameInfo);
            db.SaveChanges();
        }

        public bool DeleteSpecificationName(int id)
        {
            var nameInfo = db.SpecificationNames.FirstOrDefault(o => o.Id == id);

            if (nameInfo != null)
            {
                db.SpecificationNames.Remove(nameInfo);
                db.SaveChanges();
            }
            return true;

        }

        public void UpdateSpecificationName(SpecificationNamesViewModel model)
        {

            var nameInfo = db.SpecificationNames.FirstOrDefault(o => o.Id == model.Id);

            if (nameInfo == null) return;

            nameInfo.SpecCode = model.SpecCode;
            nameInfo.SpecName = model.SpecName;

            db.SaveChanges();
        }

        public SpecificationNamesViewModel GetSpecificationName(int nameId)
        {
            return Mapper.Map(db.SpecificationNames.AsNoTracking().FirstOrDefault(u => u.Id == nameId));
        }

        public IQueryable<SpecificationNamesViewModel> GetSpecNamesBySearchString(string searchTerm)
        {
            return db.SpecificationNames
                           .AsNoTracking()
                           .Where(s => (((s.SpecName.Contains(searchTerm) || s.SpecName.StartsWith(searchTerm) || s.SpecName.EndsWith(searchTerm))
                           || (s.SpecCode.ToString().Contains(searchTerm) || s.SpecCode.ToString().StartsWith(searchTerm) || s.SpecCode.ToString().EndsWith(searchTerm)))))
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();
        }

        public List<SpecificationNamesViewModel> SpecificationNames(string searchTerm, int pageSize, int pageNum)
        {
            return GetSpecNamesBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int SpecificationNamesCount(string searchTerm)
        {
            return GetSpecNamesBySearchString(searchTerm).Count();
        }

        public IQueryable<VehicleViewModel> getVehicleTypes()
        {
            return db.OrderVehicleTypes.AsNoTracking().Select(Mapper.Map).AsQueryable();
        }

        public IQueryable<SpecificationTypesViewModel> getSpecificationTypes()
        {
            return db.SpecificationTypes.AsNoTracking().Select(Mapper.Map).AsQueryable();
        }

        public IQueryable<ContractsViewModel> GetContractsBySearchString(string searchTerm, long? Id)
        {
            return db.Contracts
                           .AsNoTracking()
                             .Where(s => (((s.BalanceKeepers.BalanceKeeper.Contains(searchTerm) || s.BalanceKeepers.BalanceKeeper.StartsWith(searchTerm) || s.BalanceKeepers.BalanceKeeper.EndsWith(searchTerm))
                           || (s.BalanceKeepers.BalanceKeeper.ToString().Contains(searchTerm) || s.BalanceKeepers.BalanceKeeper.ToString().StartsWith(searchTerm) || s.BalanceKeepers.BalanceKeeper.ToString().EndsWith(searchTerm)))))
                            .Where(s => s.BalanceKeeperId != null)
                            .Select(Mapper.Map)
                              .AsQueryable();
        }

        public List<ContractsViewModel> GetContracts(string searchTerm, int pageSize, int pageNum, long? Id)
        {
            return GetContractsBySearchString(searchTerm, Id)
                        .Skip(pageSize* (pageNum - 1))
                         .Take(pageSize)
                           .ToList();
        }

        public int GetContractsCount(string searchTerm, long? Id)
        {
            return GetContractsBySearchString(searchTerm, Id).Count();
        }
    }
}
