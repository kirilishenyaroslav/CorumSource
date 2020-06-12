using System;
using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels.Cars;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels;

namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        IEnumerable<CarOwnersAccessViewModel> GetCarOwnersTree();

        void AddNewCarOwner(CarOwnersAccessViewModel model);

        void RemoveCarOwner(int carOwnerId);

        CarOwnersAccessViewModel GetCarOwner(int? carOwnerId);

        void UpdateCarOwner(CarOwnersAccessViewModel model);

        List<CarsViewModel> GetCars(int? carOwnerId);

        void AddNewCar(CarsViewModel model);

        void RemoveCar(int carId);

        CarsViewModel getCar(int carId);

        void UpdateCar(CarsViewModel model);

        bool CheckCarNumber(string Number);

        bool CheckCarNumber2(string Number, int carId);

        void UpdateOwnerForCar(CarsGroupViewModel model);

        IEnumerable<CarOwnersAccessViewModel> GetLeafCarOwnersTree();

        IQueryable<CarsFuelTypeViewModel> GetCarsFuelType();

        int AddNewContract(ContractsViewModel model);
        ContractsViewModel getContract(int contractId);
        IQueryable<ContractsViewModel> GetCarOwnerContracts(int? carOwnerId);
        void UpdateContract(ContractsViewModel model);
        bool DeleteContract(int id);

        IQueryable<ContractSpecificationsViewModel> GetContractSpecifications(int groupeSpecId);
        void AddSpecification(ContractSpecificationsViewModel model);
        void UpdateSpecification(ContractSpecificationsViewModel model);
        bool DeleteSpecification(int id);
        ContractSpecificationsViewModel GetContractSpecification(int specId);

        IQueryable<RouteTypesViewModel> GetRouteTypes();
        IQueryable<CarryCapacitiesViewModel> GetCarryCapacities();
        IQueryable<RouteIntervalTypesViewModel> GetRouteIntervals();

        IQueryable<GroupesSpecificationsViewModel> GetGroupesSpecifications(int contractId);
        int AddGroupeSpecification(GroupesSpecificationsViewModel model);
        GroupesSpecificationsViewModel GetGroupeSpecification(int groupeSpecId);
        bool DeleteGroupeSpecification(int id);
        void UpdateGroupeSpecification(GroupesSpecificationsViewModel model);

        IQueryable<ContractsViewModel> GetAllContractsExpBK();
        IQueryable<CarryCapacitiesViewModel> getCarryCapacities();
        void AddCarryCapacity(CarryCapacitiesViewModel model);
        bool DeleteCarryCapacity(int id);
        void UpdateCarryCapacity(CarryCapacitiesViewModel model);
        CarryCapacitiesViewModel GetCarryCapacity(int capacityId);

        IQueryable<RouteIntervalTypesViewModel> getRouteIntervalTypes();
        void AddRouteIntervalType(RouteIntervalTypesViewModel model);
        bool DeleteRouteIntervalType(int id);
        void UpdateRouteIntervalType(RouteIntervalTypesViewModel model);
        RouteIntervalTypesViewModel GetRouteIntervalType(int intervalId);

    
        IQueryable<SpecificationNamesViewModel> getSpecificationNames();
        void AddSpecificationName(SpecificationNamesViewModel model);
        bool DeleteSpecificationName(int id);
        void UpdateSpecificationName(SpecificationNamesViewModel model);
        SpecificationNamesViewModel GetSpecificationName(int nameId);
        IQueryable<SpecificationNamesViewModel> GetSpecNamesBySearchString(string searchTerm);
        List<SpecificationNamesViewModel> SpecificationNames(string searchTerm, int pageSize, int pageNum);
        int SpecificationNamesCount(string searchTerm);
        IQueryable<VehicleViewModel> getVehicleTypes();
        IQueryable<SpecificationTypesViewModel> getSpecificationTypes();

        IQueryable<ContractsViewModel> GetContractsBySearchString(string searchTerm, long? Id);
        List<ContractsViewModel> GetContracts(string searchTerm, int pageSize, int pageNum, long? Id);
        int GetContractsCount(string searchTerm, long? Id);
    }
}
