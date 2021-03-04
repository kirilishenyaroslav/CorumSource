using System;
using System.Linq;
using Corum.DAL.Entity;
using Corum.Models;
using Corum.Common;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels.Admin;
using Corum.Models.ViewModels.Cars;
using Corum.Models.ViewModels.Customers;
using Corum.Models.ViewModels.OrderConcurs;
using System.Globalization;
using Corum.Models.ViewModels.Bucket;

namespace Corum.DAL.Mappings
{
    public static class Mapper
    {

        public static CompetetiveListStepsInfoViewModel Map(OrderConcursListsSteps o)
        {
            return new CompetetiveListStepsInfoViewModel()
            {
                Id = o.Id,
                StepId = o.StepId,
                StepShortCode =o.OrderConcursSteps.StepNameShort,
                StepFullCode =o.OrderConcursSteps.StepNameFull,
                OrderId = o.OrderId??0,
                userId =o.UserId,
                userName = o.AspNetUsers.DisplayName ?? o.AspNetUsers.UserName,                
                timestamp =o.Datetimevalue,
                timestampRaw = o.Datetimevalue == null ? null : o.Datetimevalue.Value.ToString("dd.MM.yyyy H:mm:ss"),
            };
        }


        public static BucketDocument Map(BucketDocuments bd)
        {
            return new BucketDocument()
            {
                Id = bd.IdBucketDocument,
                Number = bd.DocNumber,
                CreatedBy = bd.CreatedBy
            };
        }

        public static CompetitiveListStepViewModel Map(OrderConcursSteps o)
        {
            return new CompetitiveListStepViewModel()
            {
                Id=o.Id,
                StepShortName = o.StepNameShort,
                StepFullName = o.StepNameFull
            };
        }


        public static OrderProjectViewModel Map(Projects o)
        {
            return new OrderProjectViewModel()
            {
                Id          = o.Id,
                Code        = o.Code,
                Description = o.Description,
                ProjectCFOId = o.ProjectCFOId??0,
                ProjectCFOName =o.Centers?.Center,
                ProjectTypeId = o.ProjectTypeId??0,
                ProjectTypeName = o.ProjectTypes?.Name,
                ConstructionDesc = o.ConstructionDesc,
                PlanCount = o.PlanCount??0,
                isActive = o.isActive??false,
                ProjectOrderer = o.ProjectOrderer,
                Comments = o.Comments,
                CanBeDelete = o.OrdersBase.Count()==0,
                CanShowManufacture = (o.ProjectTypeId ?? 0) == 1 ? true : false,
                ManufacturingEnterprise = o.ManufacturingEnterprise,
                NumOrder = o.NumOrder ?? 0,               
                DateOpenOrder = o.DateOpenOrder == null ? null : o.DateOpenOrder.Value.ToString("dd.MM.yyyy"),
                DateOpenOrderRaw = o.DateOpenOrder == null ? null : DateTimeConvertClass.getString(o.DateOpenOrder.Value),
                PlanPeriodForMP = o.PlanPeriodForMP == null ? null : o.PlanPeriodForMP.Value.ToString("dd.MM.yyyy"),
                PlanPeriodForMPRaw = o.PlanPeriodForMP == null ? null : DateTimeConvertClass.getString(o.PlanPeriodForMP.Value),
                PlanPeriodForComponents = o.PlanPeriodForComponents == null ? null : o.PlanPeriodForComponents.Value.ToString("dd.MM.yyyy"),
                PlanPeriodForComponentsRaw = o.PlanPeriodForComponents == null ? null : DateTimeConvertClass.getString(o.PlanPeriodForComponents.Value),
                PlanPeriodForSGI = o.PlanPeriodForSGI == null ? null : o.PlanPeriodForSGI.Value.ToString("dd.MM.yyyy"),
                PlanPeriodForSGIRaw = o.PlanPeriodForSGI == null ? null : DateTimeConvertClass.getString(o.PlanPeriodForSGI.Value),
                PlanPeriodForTransportation = o.PlanPeriodForTransportation == null ? null : o.PlanPeriodForTransportation.Value.ToString("dd.MM.yyyy"),
                PlanPeriodForTransportationRaw = o.PlanPeriodForTransportation == null ? null : DateTimeConvertClass.getString(o.PlanPeriodForTransportation.Value),
                PlanDeliveryToConsignee = o.PlanDeliveryToConsignee == null ? null : o.PlanDeliveryToConsignee.Value.ToString("dd.MM.yyyy"),
                PlanDeliveryToConsigneeRaw = o.PlanDeliveryToConsignee == null ? null : DateTimeConvertClass.getString(o.PlanDeliveryToConsignee.Value),
                DeliveryBasic = o.DeliveryBasic,
                Shipper = o.Shipper ?? 0,
                ShipperName = o.Organization == null ? null : o.Organization.Name,
                Consignee = o.Consignee ?? 0,
                ConsigneeName = o.Organization1 == null ? null : o.Organization1.Name,
            };
        }

        public static ProjectTypeViewModel Map(ProjectTypes o)
        {
            return new ProjectTypeViewModel()
            {
                Id = o.Id,
                Name = o.Name
            };
        }


        public static UnloadingTypeViewModel Map(OrderUnloadingTypes o)
        {
            return new UnloadingTypeViewModel()
            {
                Id = o.Id,
                UnloadingTypeName = o.UnloadingTypeName
            };
        }

        public static LoadingTypeViewModel Map(OrderLoadingTypes o)
        {
            return new LoadingTypeViewModel()
            {
                Id = o.Id,
                LoadingTypeName = o.LoadingTypeName
            };
        }

        public static VehicleViewModel Map(OrderVehicleTypes o)
        {
            return new VehicleViewModel()
            {
                Id = o.Id,
                VehicleTypeName = o.VehicleTypeName
            };
        }

        public static TruckTypeViewModel Map(OrderTruckTypes o)
        {
            return new TruckTypeViewModel()
            {
                Id = o.Id,
                TruckTypeName = o.TruckTypeName
            };
        }

        public static OrderUsedCarViewModel Map(OrderUsedCars o)
        {
            return new OrderUsedCarViewModel()
            {
                Id = o.Id,
                OrderId = o.OrderId,
                ContractId = o.ContractId ?? 0,
                ContractExpBkId = o.ContractExpBkId ?? 0,
                ContractInfo = o.ContractInfo,
                ContractExpBkInfo = o.Contracts1?.ContractNumber + " от " + o.Contracts1?.ContractDate.Value.ToString("dd.MM.yyyy") + 
                                     "(с " + o.Contracts1?.DateBeg.Value.ToString("dd.MM.yyyy")  + " по " + o.Contracts1?.DateEnd.Value.ToString("dd.MM.yyyy") + ")",
                ExpeditorId = o.ExpeditorId ?? 0,
                ExpeditorName = o.CarOwners?.CarrierName,
                CarOwnerInfo = o.CarOwnerInfo,
                CarModelInfo = o.CarModelInfo,
                CarRegNum = o.CarRegNum,
                CarCapacity = o.CarCapacity,
                CarDriverInfo = o.CarDriverInfo,
                DriverContactInfo = o.DriverContactInfo,
                CarrierInfo = o.CarrierInfo,
                CarId = o.CarId??0,
                Summ  = o.Summ??0,
                DriverCardInfo = o.DriverCardInfo,
                Comments = o.Comments,
                PlanDistance = (o.PlanDistance ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                PlanTimeWorkDay = o.PlanTimeWorkDay,
                PlanTimeHoliday = o.PlanTimeHoliday,
                BaseRate = (o.BaseRate ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                BaseRateWorkDay = (o.BaseRateWorkDay ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                BaseRateHoliday = (o.BaseRateHoliday ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                DelayDays = o.DelayDays,
                FactShipperDateTime = o.FactShipperDateTime,
                FactConsigneeDateTime = o.FactConsigneeDateTime,
                
                FactShipperDate = o.FactShipperDateTime != null ? o.FactShipperDateTime.Value.ToString("dd.MM.yyyy") : "",
                FactShipperDateRaw = o.FactShipperDateTime != null ? DateTimeConvertClass.getString(o.FactShipperDateTime.Value) : "",
                FactConsigneeDate = o.FactConsigneeDateTime != null ? o.FactConsigneeDateTime.Value.ToString("dd.MM.yyyy") : "",
                FactConsigneeDateRaw = o.FactConsigneeDateTime != null ? DateTimeConvertClass.getString(o.FactConsigneeDateTime.Value) : "",

                FactShipperTime = o.FactShipperDateTime != null ? o.FactShipperDateTime.Value.ToString("HH:mm") : "",
                FactShipperTimeRaw = o.FactShipperDateTime != null ? DateTimeConvertClass.getString(o.FactShipperDateTime.Value) : "",
                FactConsigneeTime = o.FactConsigneeDateTime != null ? o.FactConsigneeDateTime.Value.ToString("HH:mm") : "",
                FactConsigneeTimeRaw =  o.FactConsigneeDateTime != null ? DateTimeConvertClass.getString(o.FactConsigneeDateTime.Value) : "",

                RealFactShipperDate = o.FactShipper != null ? o.FactShipper.Value.ToString("dd.MM.yyyy") : "",
                RealFactShipperDateRaw = o.FactShipper != null ? DateTimeConvertClass.getString(o.FactShipper.Value) : "",
                RealFactConsigneeDate = o.FactConsignee != null ? o.FactConsignee.Value.ToString("dd.MM.yyyy") : "",
                RealFactConsigneeDateRaw = o.FactConsignee != null ? DateTimeConvertClass.getString(o.FactConsignee.Value) : "",

                RealFactShipperTime = o.FactShipper != null ? o.FactShipper.Value.ToString("HH:mm") : "",
                RealFactShipperTimeRaw = o.FactShipper != null ? DateTimeConvertClass.getString(o.FactShipper.Value) : "",
                RealFactConsigneeTime = o.FactConsignee != null ? o.FactConsignee.Value.ToString("HH:mm") : "",
                RealFactConsigneeTimeRaw =  o.FactConsignee != null ? DateTimeConvertClass.getString(o.FactConsignee.Value) : "",

            };
        }

        public static OrderNotificationViewModel Map(OrderNotifications o)
        {
            return new OrderNotificationViewModel()
            {
                Datetime      = o.Datetime,
                Id            = o.Id,
                Body          = o.Body,
                OrderId       = o.OrderId,
                CreatedBy     = o.CreatedBy,
                CreatedByName = o.AspNetUsers1.DisplayName,
                Receiver      = o.Reciever,
                ReceiverEmail = o.AspNetUsers.Email,
                ReceiverName  = o.AspNetUsers.DisplayName,
                TypeId        = o.TypeId,
                TypeName      = o.OrderNotificationTypes.Name
            };

        }

        public static OrderNotificationTypesViewModel Map(OrderNotificationTypes o)
        {
            return new OrderNotificationTypesViewModel()
            {
                Id = o.Id,
                TypeName = o.Name
            };

        }

        public static OrganizationViewModel Map(Organization o)
        {
            return new OrganizationViewModel()
            {
                Id = o.Id,
                Name = o.Name,
                Address = o.Address,
                City = o.City,
                CanBeDelete = !((o.Routes.Count() > 0) || (o.Routes1.Count() > 0) || (o.AdditionalRoutePoints.Count > 0) || (o.Projects.Count>0)||(o.Projects1.Count>0)),
                CountryId = o.CountryId ?? 0,
                Country = (o.CountryId != null) ? o.Countries.Name : "",
                IsTruck = o.IsTruck??false,
                Latitude = (o.Latitude ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                Longitude = (o.Longitude ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                IsAuto = o.IsAuto,
                IsSystemOrg = o.IsSystemOrg
            };

        }

        public static OrderCountriesViewModel Map(Countries o)
        {
            return new OrderCountriesViewModel()
            {
                Id = o.Сode,
                CountryName = o.Name,
                IsDefault = o.IsDefault??false
            };

        }


        public static OrderClientCFOViewModel Map(Centers o)
        {
            return new OrderClientCFOViewModel()
            {
                Id = o.Id,
                CFOName = o.Center
            };
        }

        public static OrderClientBalanceKeeperViewModel Map(BalanceKeepers o)
        {
            return new OrderClientBalanceKeeperViewModel()
            {
                Id = o.Id,
                BalanceKeeperName = o.BalanceKeeper
            };
        }


        public static LoginHistoryViewModel Map(LoginHistory o)
        {
            return new LoginHistoryViewModel()
            {
                UserName = o.AspNetUsers.DisplayName,
                UserEmail = o.AspNetUsers.Email,
                Datetime = o.Datetime.ToString("yyyy.MM.dd HH:mm:ss"),
                IP       = o.IP,
                UserAgent= o.userAgent
            };
        }

        public static void Map(OrderTruckTransport o, ref OrdersTruckTransportViewModel vm)
        {
            vm.OrderId              = o.OrderId;
            vm.Shipper              = o.Shipper;
            vm.ShipperCountryId     = o.ShipperCountryId??0;
            vm.ShipperCountryName   = o.Countries1?.Name;
            vm.ShipperCity          = o.ShipperCity;
            vm.ShipperAdress        = o.ShipperAdress;
            vm.FromShipperDate      = o.FromShipperDatetime.Value.ToString("dd.MM.yyyy");
            vm.FromShipperDateRaw   = DateTimeConvertClass.getString(o.FromShipperDatetime.Value);
            vm.FromShipperTime      = o.FromShipperDatetime.Value.ToString("HH:mm");
            vm.FromShipperTimeRaw   = DateTimeConvertClass.getString(o.FromShipperDatetime.Value);

            vm.Consignee            = o.Consignee;
            vm.ConsigneeCountryId   = o.ConsigneeCountryId ?? 0;
            vm.ConsigneeCountryName = o.Countries?.Name;
            vm.ConsigneeCity        = o.ConsigneeCity;
            vm.ConsigneeAdress      = o.ConsigneeAdress;
            vm.ToConsigneeDate      = o.ToConsigneeDatetime.Value.ToString("dd.MM.yyyy");
            vm.ToConsigneeDateRaw   = DateTimeConvertClass.getString(o.ToConsigneeDatetime.Value);
            vm.ToConsigneeTime      = o.ToConsigneeDatetime.Value.ToString("HH:mm");
            vm.ToConsigneeTimeRaw   = DateTimeConvertClass.getString(o.ToConsigneeDatetime.Value);

            vm.BoxingDescription    = o.BoxingDescription;
            vm.TruckDescription     = o.TruckDescription;
            vm.TripType             = o.TripType??0;            
            vm.NameRouteType = o.RouteTypes == null ? null : o.RouteTypes.NameRouteType;
            vm.Weight               = (o.Weight ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA"));
            vm.Volume               = Convert.ToDouble(o.Volume ?? 0);
            vm.DimenssionL          = Convert.ToDouble(o.DimenssionL ?? 0);
            vm.DimenssionW          = Convert.ToDouble(o.DimenssionW ?? 0);
            vm.DimenssionH          = Convert.ToDouble(o.DimenssionH ?? 0);
            vm.TruckTypeId          = o.TruckTypeId; 
            vm.TruckTypeName        = o.OrderTruckTypes?.TruckTypeName;
            vm.VehicleTypeId        = o.VehicleTypeId??0;
            vm.VehicleTypeName      = o.OrderVehicleTypes?.VehicleTypeName;
            vm.LoadingTypeId        = o.LoadingTypeId??0; 
            vm.LoadingTypeName      = o.OrderLoadingTypes?.LoadingTypeName;
            vm.UnloadingTypeId      = o.UnloadingTypeId ?? 0;
            vm.UnloadingTypeName    = o.OrderUnloadingTypes?.UnloadingTypeName;

            vm.ShipperContactPerson = o.ShipperContactPerson;
            vm.ShipperContactPersonPhone = o.ShipperContactPersonPhone;
            vm.ConsigneeContactPerson = o.ConsigneeContactPerson;
            vm.ConsigneeContactPersonPhone = o.ConsigneeContactPersonPhone;

            vm.ShipperId = o.ShipperId??0;
            vm.ConsigneeId = o.ConsigneeId ?? 0;
            vm.LatitudeShipper = o.Organization1?.Latitude ?? 0;
            vm.LongitudeShipper = o.Organization1?.Longitude ?? 0;
            vm.LatitudeConsignee = o.Organization?.Latitude ?? 0;
            vm.LongitudeConsignee = o.Organization?.Longitude ?? 0;

        }

        public static void Map (OrdersPassengerTransport o, ref OrdersPassTransportViewModel vm)
        {
            vm.OrderId                 = o.OrderId;

            vm.StartDateTimeOfTrip     = o.StartDateTimeOfTrip.ToString("dd.MM.yyyy");
            vm.FinishDateTimeOfTrip    = o.FinishDateTimeOfTrip.ToString("dd.MM.yyyy");

            vm.StartDateTimeExOfTrip   = o.StartDateTimeOfTrip.ToString("HH:mm");
            vm.FinishDateTimeExOfTrip  = o.FinishDateTimeOfTrip.ToString("HH:mm");

            vm.StartDateTimeOfTripRaw  = DateTimeConvertClass.getString(o.StartDateTimeOfTrip);
            vm.FinishDateTimeOfTripRaw = DateTimeConvertClass.getString(o.FinishDateTimeOfTrip);

            vm.StartDateTimeExOfTripRaw = DateTimeConvertClass.getString(o.StartDateTimeOfTrip);
            vm.FinishDateTimeExOfTripRaw = DateTimeConvertClass.getString(o.FinishDateTimeOfTrip);

            vm.NeedReturn = o.NeedReturn;

            if (o.ReturnStartDateTimeOfTrip!=null) vm.ReturnStartDateTimeOfTrip = o.ReturnStartDateTimeOfTrip.Value.ToString("dd.MM.yyyy");
            if (o.ReturnFinishDateTimeOfTrip!=null) vm.ReturnFinishDateTimeOfTrip = o.ReturnFinishDateTimeOfTrip.Value.ToString("dd.MM.yyyy");

            if (o.ReturnStartDateTimeOfTrip!=null) vm.ReturnStartDateTimeExOfTrip = o.ReturnStartDateTimeOfTrip.Value.ToString("HH:mm");
            if (o.ReturnFinishDateTimeOfTrip!=null) vm.ReturnFinishDateTimeExOfTrip = o.ReturnFinishDateTimeOfTrip.Value.ToString("HH:mm");

            if (o.ReturnStartDateTimeOfTrip!=null) vm.ReturnStartDateTimeOfTripRaw = DateTimeConvertClass.getString(o.ReturnStartDateTimeOfTrip.Value);
            if (o.ReturnFinishDateTimeOfTrip!=null) vm.ReturnFinishDateTimeOfTripRaw = DateTimeConvertClass.getString(o.ReturnFinishDateTimeOfTrip.Value);

            if (o.ReturnStartDateTimeOfTrip!=null) vm.ReturnStartDateTimeExOfTripRaw = DateTimeConvertClass.getString(o.ReturnStartDateTimeOfTrip.Value);
            if (o.ReturnFinishDateTimeOfTrip!=null) vm.ReturnFinishDateTimeExOfTripRaw = DateTimeConvertClass.getString(o.ReturnFinishDateTimeOfTrip.Value);

            if ((o.FinishDateTimeOfTrip!=null) && (o.ReturnStartDateTimeOfTrip != null))
            {
                vm.ReturnWaitingTime = TimeSpan.FromHours((o.ReturnStartDateTimeOfTrip.Value - o.FinishDateTimeOfTrip).TotalHours).ToString(@"hh\:mm\:ss");
            }
            
            vm.AdressFrom              = o.AdressFrom;
            vm.AdressTo                = o.AdressTo;

            vm.OrgFrom                 = o.OrgFrom;
            vm.OrgTo                   = o.OrgTo;

            vm.CountryFrom             = o.FromCountry??0;
            vm.CountryFromName         = o.Countries == null ? null : o.Countries.Name;

            vm.CountryTo               = o.ToCountry??0;
            vm.CountryToName           = o.Countries1 == null ? null : o.Countries1.Name;

            vm.CityFrom                = o.FromCity;
            vm.CityTo                  = o.ToCity;

            vm.TripDescription         = o.TripDescription;
            vm.PassInfo                = o.PassInfo;

            vm.CarDetailInfo           = o.CarDetailInfo;
            vm.CarDriverFio            = o.CarDriverFio;
            vm.CarDriverContactInfo    = o.CarDriverContactInfo;

            vm.TripType                = o.TripType??0;
            vm.NameRouteType = o.RouteTypes == null ? null : o.RouteTypes.NameRouteType;

            vm.OrgFromId = o.OrgFromId ?? 0;
            vm.OrgToId = o.OrgToId ?? 0;
            vm.LatitudeOrgTo = o.Organization1?.Latitude ?? 0;
            vm.LongitudeOrgTo = o.Organization1?.Longitude ?? 0;
            vm.LatitudeOrgFrom = o.Organization?.Latitude ?? 0;
            vm.LongitudeOrgFrom = o.Organization?.Longitude ?? 0;

        }

        public static OrderPipelineStepViewModel Map(OrderPipelineSteps o)
        {
            return new OrderPipelineStepViewModel()
            {
                Id = o.Id,
                FromStatus = o.FromStatus,
                FromStatusName = o.OrderStatuses.OrderStatusName,
                ToStatus = o.ToStatus,
                ToStatusName = o.OrderStatuses1.OrderStatusName,
                AccessRoleId = o.AccessRoleId,
                AccessRoleName = o.AspNetRoles.Name,
                StartDateForClientLayer = o.StartDateForClient ?? false,
                StartDateForExecuterLayer = o.StartDateForExecuter ?? false,
                OrderTypeId = o.OrderTypeId ?? 0,
                OrderTypeName = o.OrderTypesBase == null ? null : o.OrderTypesBase.TypeName,
                FromStatusColor = o.OrderStatuses.Color,
                ToStatusColor = o.OrderStatuses1.Color,
                FinishStatusForBP = o.FinishOfTheProcess??false

            };
        }

        public static OrderAttachmentViewModel Map(OrderAttachments o)
        {
            return new OrderAttachmentViewModel()
            {
                Id = o.Id,
                OrderId = o.OrderId,
                DocDescription = o.DocDescription,
                AddedByUser = o.AddedByUser,
                AddedByUserName= o.AspNetUsers.DisplayName,
                AddedDateTime = o.AddedDateTime,
                DocType = o.DocType,
                DocTypeName = o.OrdersDocTypes.DocType,
                RealFileName =o.RealFileName
            };
        }

        public static OrderAttachmentViewModel MapWithBody(OrderAttachments o)
        {
            return new OrderAttachmentViewModel()
            {
                Id = o.Id,
                OrderId = o.OrderId,
                DocDescription = o.DocDescription,
                AddedByUser = o.AddedByUser,
                AddedByUserName = o.AspNetUsers.DisplayName,
                AddedDateTime = o.AddedDateTime,
                DocType = o.DocType,
                DocTypeName = o.OrdersDocTypes.DocType,
                RealFileName = o.RealFileName,
                DocBody = o.DocBody
            };
        }

        public static OrderStatusHistoryViewModel Map(OrderStatusHistory o)
        {
            return new OrderStatusHistoryViewModel()
            {
                Id = o.Id,
                OrderId = o.OrderId,
                EditedDateTime =o.ChangeDateTime,
                OldStatusId = o.OldStatus??0,
                OldStatusName = (o.OldStatus!=null)? o.OrderStatuses.OrderStatusName : string.Empty,
                NewStatusId = o.NewStatus??0,
                NewStatusName= (o.NewStatus != null) ? o.OrderStatuses1.OrderStatusName : string.Empty,
                StatusChangeComment = o.StatusChangeComment,
                CreatedByUser = o.ChangedByUser,
                CreateByUserName = o.AspNetUsers.DisplayName
            };
        }


        public static OrderClientsViewModel Map(OrderClients o)
        {
            return new OrderClientsViewModel()
            {
                Id = o.Id,
                ClientName = o.ClientName,                
                AccessRoleId = o.AccessRoleId,
                RoleName = o.AspNetRoles.Name,
                CanBeDelete = o.OrdersBase.Count==0,                
                ClientCFOId = o.ClientCFOId??0,
                ClientCFOName = o.Centers == null ? null : o.Centers.Center
            };
        }

        public static OrderObserverViewModel Map(OrderObservers o)
        {
            return new OrderObserverViewModel()
            {
                Id = o.Id,
                observerId = o.userId,
                observerName = o.AspNetUsers.DisplayName,
                OrderId = o.OrderId,
                observerEmail = o.AspNetUsers.Email
            };


        }


        public static OrderDocTypeViewModel Map(OrdersDocTypes o)
        {
            return new OrderDocTypeViewModel()
            {
                Id = o.Id,
                DocTypeName = o.DocType
            };
        }


        public static OrderStatusViewModel Map(OrderStatuses o)
        {
            return new OrderStatusViewModel()
            {
                Id = o.Id,
                StatusName = o.OrderStatusName,
                StatusColor = o.Color,
                AllowRegData = o.AllowEditRegData??false,
                AllowClientData = o.AllowEditClientData??false,
                AllowExecuterData = o.AllowEditExecuterData??false,
                ActionName = o.ActionName,
                IconFile = o.IconFile,
                IconDescription = o.IconDescription,
                ShortName = o.ShortName,
                FontColor = o.FontColor,
                BackgroundColor = o.BackgroundColor,
                CanBeDelete = !((o != null) && (o.OrdersBase.Count() > 0) || (o.OrderPipelineSteps.Count() > 0) || (o.OrderPipelineSteps1.Count() > 0))
            };
        }

        public static OrderTypeViewModel Map(OrderTypesBase o)
        {
            return new OrderTypeViewModel()
            {
                Id = o.Id,
                TypeName = o.TypeName,
                ShortName = o.ShortName,
                CanBeDelete = !((o != null) && (o.OrdersBase.Count() > 0)),
                UserRoleIdForClientData = o.UserRoleIdForClientData,
                UserRoleIdForExecuterData = o.UserRoleIdForExecuterData,
                UserRoleIdForTypeAccess = o.TypeAccessGroupId,
                UserRoleIdForClientDataName = o.AspNetRoles1 == null ? null : o.AspNetRoles1.Name,
                UserRoleIdForExecuterDataName = o.AspNetRoles2 == null ? null : o.AspNetRoles2.Name,
                UserRoleNameTypeAccess = o.AspNetRoles == null ? null : o.AspNetRoles.Name,
                DefaultExecuterId = o.DefaultExecuterId,
                DefaultExecuterName = o.AspNetUsers1?.DisplayName,
                UserIdForAnonymousForm = o.UserForAnnonymousForm,
                UserIdForAnonymousFormName = o.AspNetUsers?.DisplayName,
                IsTransportType = o.IsTransportType??false,
                UserRoleIdForCompetitiveList = o.AspNetRoles3 == null ? null : o.AspNetRoles3.Id,
                UserRoleIdForCompetitiveListName = o.AspNetRoles3 == null ? null : o.AspNetRoles3.Name,
                IsActive = o.IsActive
            };
        }

        public static OrderBaseViewModel Map (OrdersBase o)
        {
            var TotalDistanceLength = o.TotalDistanceLength ?? 0;

            var result = new OrderBaseViewModel()
            {
                Id                     = o.Id,
                OrderDate              = o.OrderDate.ToString("dd.MM.yyyy"),
                OrderDateRaw           = DateTimeConvertClass.getString(o.OrderDate),
                CreatedByUser          = o.CreatedByUser,
                CreatedByUserName      = o.AspNetUsers.DisplayName,
                CreateDatetime         = o.CreateDatetime,
                OrderType              = o.OrderType,
                OrderTypename          = o.OrderTypesBase.TypeName,
                OrderTypeShortName     = o.OrderTypesBase.ShortName,
                CurrentOrderStatus     = o.CurrentOrderStatus,
                CurrentOrderStatusColor= o.OrderStatuses.Color,
                CurrentOrderStatusName = o.OrderStatuses.OrderStatusName,
                CurrentStatusShortName = o.OrderStatuses.ShortName,
                FontColor              = o.OrderStatuses.FontColor,
                BackgroundColor        = o.OrderStatuses.BackgroundColor,
                OrderDescription       = o.OrderDescription,
                ClientId               = o.ClientId,
                ClientName             = o.OrderClients.ClientName,
                ClientCenterName       = o.OrderClients.Centers.Center,
                CanBeDelete            = (o.CurrentOrderStatus==1)||(o.CurrentOrderStatus == 17),
                Summ                   = o.Summ??0,
                UseNotifications       = o.UseNotifications ?? false,
                CreatorContact         = o.CreatorContact,
                CreatorPosition        = o.CreatorPosition,
                PriorityType           = o.PriotityType,
                OrderServiceDatetime   = o.OrderServiceDateTime??DateTime.Now,
                IconFile               = o.OrderStatuses.IconFile,
                IconDescription        = o.OrderStatuses.IconDescription,
                OrderExecuter          = o.OrderExecuter,
                OrderExecuterName      = o.AspNetUsers1 == null ? null : o.AspNetUsers1.DisplayName,
                PayerId                = o.PayerId??0,
                PayerName              = (o.PayerId!=null)? o.BalanceKeepers.BalanceKeeper: string.Empty,
                ProjectId              = o.ProjectId??0,
                ProjectNum             = o.Projects?.Code,
                ProjectDescription     = o.Projects?.Description,
                CarNumber              = o.CarNumber??0,
                TotalDistanceDescription = o.DistanceDescription,
                TotalCost              = (o.TotalPrice??0).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                TotalDistanceLenght    = TotalDistanceLength.ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                IsTransport            = o.OrderTypesBase.IsTransportType??false,
                IsPrivateOrder         = o.IsPrivateOrder??false,
                ExecuterNotes          = o.ExecuterNotes,
                TypeSpecId = o.TypeSpecId ?? 0,               
                TimeRoute = DateTimeConvertClass.getHoursFormat(o.TimeRoute ?? 0),
                TimeSpecialVehicles = DateTimeConvertClass.getTimeFormat(o.TimeSpecialVehicles ?? 0),
                SpecificationType = o.SpecificationTypes == null ? null : o.SpecificationTypes.SpecificationType,
                IsAdditionalRoutePoints = o.IsAdditionalRoutePoints ??false,
                RouteId = o.RouteId??0,
                ShortName = o.Routes == null ? "" : o.Routes.ShortName,

            };

            return result;
        }

        public static OrderBaseViewModel Map(GetOrdersPipelineV3_Result o)
        {
            var TotalDistanceLength = o.TotalDistanceLength ?? 0;
            var result = new OrderBaseViewModel()
            {
                Id = o.Id,
                OrderDate = o.OrderDate.ToString("dd.MM.yyyy"),
                OrderDateRaw = DateTimeConvertClass.getString(o.OrderDate),
                CreatedByUser = o.CreatedByUser,
                CreatedByUserName = o.CreatorDispalyName,
                CreateDatetime = o.CreateDatetime,
                OrderType = o.OrderType,
                OrderTypename = o.TypeName,
                OrderTypeShortName = o.TypeShortName,
                CurrentOrderStatus = o.CurrentOrderStatus,
                CurrentOrderStatusColor = o.Color,
                CurrentOrderStatusName = o.OrderStatusName,
                CurrentStatusShortName = o.OrderStatusShortName,
                FontColor = o.FontColor,
                BackgroundColor = o.BackgroundColor,
                OrderDescription = o.OrderDescription,
                ClientId = o.ClientId,
                ClientName = o.ClientName,
                ClientCenterName = o.CenterName,
                CanBeDelete = (o.CurrentOrderStatus == 1) || (o.CurrentOrderStatus == 17),
                Summ = o.Summ ?? 0,
                UseNotifications = o.UseNotifications ?? false,
                CreatorContact = o.CreatorContact,
                CreatorPosition = o.CreatorPosition,
                PriorityType = o.PriotityType,
                OrderServiceDatetime = o.OrderServiceDateTime ?? DateTime.Now,
                IconFile = o.IconFile,
                IconDescription = o.IconDescription,
                OrderExecuter = o.OrderExecuter,
                OrderExecuterName = o.ExecutorDisplayName,
                ExecuterNotes = o.ExecuterNotes,
                PayerId = o.PayerId ?? 0,
                PayerName = o.PayerName,
                ProjectNum = o.ProjectCode,
                ProjectId = o.ProjectId??0,
                ProjectDescription = o.ProjectDescription,
                CarNumber = o.CarNumber ?? 0,
                TotalDistanceDescription = o.DistanceDescription,
                TotalCost = (o.TotalPrice ?? 0).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                TotalDistanceLenght = TotalDistanceLength.ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                IsTransport = o.IsTransportType ?? false,
                IsPrivateOrder = o.IsPrivateOrder ?? false,
                IsFinishOfTheProcess = o.isFinishOfTheProcess ?? false,
                ReportStatusName = (o.isFinishOfTheProcess ?? false) ? "Финальный статус" : o.OrderStatusName,
                ReportStatusId = (o.isFinishOfTheProcess ?? false) ? 0 : o.CurrentOrderStatus,
                ReportColor = (o.isFinishOfTheProcess ?? false) ? "#666666" : o.BackgroundColor
            };

            return result;
        }

        public static MenuAccessViewModel Map(MenuStructure menu)
        {
            return new MenuAccessViewModel()
            {
                Id  = menu.Id,
                menuName = menu.menuName,
                menuHtmlId = menu.menuId,
                parentId = menu.parentId
           };


        }
     
        public static ImportError Map(LogImportErrors error)
        {
            return new ImportError()
            {
                ColumnName=error.ColumnName,
                CommentError=error.CommentError,
                IsCommentType=error.isCommentType,
                NumRow=error.NumRow.ToString()
            };
        }


        public static UserViewModel Map(GetOrderExecuters_Result user)
        {
            return new UserViewModel()
            {
                userId = user.Id,
                userEmail = user.Email,
                displayName = user.DisplayName ?? user.UserName,
                userPassword = "empty",                
                twoFactorEnabled = user.TwoFactorEnabled,
                CanBeDelete = true,
                contactPhone = user.ContactPhone,
                postName = user.PostName
            };
        }

        public static UserViewModel Map(AspNetUsers user)
        {
            return new UserViewModel()
            { 
                userId = user.Id,
                userEmail = user.Email,
                displayName = user.DisplayName ?? user.UserName,
                userPassword = "empty",
                isAdmin = user.AspNetRoles.Count(r => r.Id == GlobalConsts.GetAdminRoleId())>0,
                twoFactorEnabled = user.TwoFactorEnabled,
                CanBeDelete = true,
                contactPhone = user.ContactPhone,
                postName = user.PostName,
                Dismissed = user.Dismissed

            };
        }

        public static RoleViewModel Map(AspNetRoles role)
        {
            return new RoleViewModel()
            {
                roleId = role.Id,
                roleName = role.Name,
                roleDescription = role.RoleDescription,
                CanBeDelete = !((role != null) && (role.AspNetUsers.Count() > 0)||(role.Id=="1000"))
            };
        }

        public static SnapshotInfoViewModel Map(LogisticSnapshots snapshots)
        {
            return new SnapshotInfoViewModel()
            {
                Id = snapshots.id_spanshot,
                DateOfImport = snapshots.shapshot_data,
                IsRestsWereImported = snapshots.isRestsLoaded != null && snapshots.isRestsLoaded.Value,
                IsDocsWereImported = snapshots.isDocsLoaded != null && snapshots.isDocsLoaded.Value,
                ActualDateBeg = snapshots.ActualDateBeg,
                ActualDateEnd = snapshots.ActualDateEnd,
                DefaultForReports = ((snapshots.isDefaultForReports ?? 0) == 1)
            };
        }

        public static RestViewModel Map(RestsSnapshot rest)
        {
            return new RestViewModel()
            {
                idrow = rest.idrow,
                id_snapshot = rest.id_snapshot,
                InnerPartyKey = rest.InnerPartyKey,
                Producer = rest.Producer,
                Product = rest.Product,
                Shifr = rest.Shifr,
                Figure = rest.Figure,
                Measure = rest.Measure,
                Weight = rest.Weight,
                pType = rest.pType,
                pGroup = rest.pGroup,
                pRecieverPlan = rest.pRecieverPlan,
                pRecieverFact = rest.pRecieverFact,
                RecieverGroupPlan = rest.RecieverGroupPlan,
                InnerOrderNum = rest.InnerOrderNum,
                OrderedBy = rest.OrderedBy,
                OrderNum = rest.OrderNum,
                QuantityBefore = rest.QuantityBefore??0,
                PE_Before = rest.PE_Before??0,
                PF_Before = rest.PF_Before??0,
                PCP_Before = rest.PCP_Before??0,
                PCPC_Before = rest.PCPC_Before??0,
                FCP_Before = rest.FCP_Before??0,
                FCPC_Before = rest.FCPC_Before??0,
                BP_Before = rest.BP_Before??0,
                PE_After = rest.PE_After??0,
                PF_After = rest.PF_After??0,
                PCP_After = rest.PCP_After??0,
                PCPC_After = rest.PCPC_After??0,
                FCP_After = rest.FCP_After??0,
                FCPC_After = rest.FCPC_After??0,
                BP_After = rest.BP_After??0,
                QuantityAfter = rest.QuantityAfter??0,
                Storage = rest.Storage,
                StorageCity = rest.StorageCity,
                Сenter = rest.Сenter,
                BalanceKeeper = rest.BalanceKeeper,
                ReadyForSaleStatus = rest.ReadyForSaleStatus,
                ReserveStatus = rest.ReserveStatus,
                ProduceDate = rest.ProduceDate,
                ReconcervationDate = rest.ReconcervationDate,
                TermOnStorage = rest.TermOnStorage,
                PrihodDocType = rest.PrihodDocType,
                PrihodDocNum = rest.PrihodDocNum,
                PrihodDocDate = rest.PrihodDocDate,
                BalanceCurrency = rest.BalanceCurrency,
                CurrencyIndexToUAH = rest.CurrencyIndexToUAH
            };
        }
        public static CarsViewModel Map(Cars car)
        {
            return new CarsViewModel()
            {
                CarId = car.Id,
                CarModel = car.Model,
                Number = car.Number,
                Driver = car.Driver,
                DriverLicenseSeria = car.DriverLicenseSeria,
                DriverLicenseNumber = car.DriverLicenseNumber,
                FuelTypeId = car.FuelTypeId,
                ConsumptionCity = car.ConsumptionCity,
                ConsumptionHighway = car.ConsumptionHighway,
                PassNumber = car.PassNumber,
                CarOwnersId = car.CarOwnersId
            };
        }

        public static CarsFuelTypeViewModel Map(CarsFuelType carsFuelType)
        {
            return new CarsFuelTypeViewModel()
            {
                FuelTypeId = carsFuelType.Id,
                FuelType = carsFuelType.FuelType
            };
        }

        public static ContractsViewModel Map(Contracts contract)
        {
            string ContractDate = "";
            string DateBeg = "";
            string DateEnd = "";
            string ReceiveDateReal = "";
            string ReceiveDateRaw = "";
            string backgroundColor = "#a2a7af";
            if (contract.ContractDate!=null) ContractDate = contract.ContractDate.Value.ToString("dd.MM.yyyy");
            if (contract.DateBeg != null) DateBeg = contract.DateBeg.Value.ToString("dd.MM.yyyy");
            if (contract.DateEnd != null) DateEnd = contract.DateEnd.Value.ToString("dd.MM.yyyy");
            if (contract.ReceiveDateReal != null)
            {
                ReceiveDateReal = contract.ReceiveDateReal.Value.ToString("dd.MM.yyyy");
                ReceiveDateRaw = DateTimeConvertClass.getString(contract.ReceiveDateReal.Value);
            }

            DateTime dateTimeNow = DateTime.Now;
            DateTime dateEnd = contract.DateEnd ?? DateTime.MinValue;
            DateTime dateTimeInMonth = DateTime.Now.AddDays(30);
            if (contract.DateEnd < DateTime.Now) {
                backgroundColor = "#FF0000";
            } else
            if((dateEnd - dateTimeInMonth).TotalDays <= 30)
            {
                backgroundColor = "#FFCC99";
            } else
            {
                backgroundColor = "#ffffff";
            }

            return new ContractsViewModel()
            {
                Id = contract.Id,
                CarOwnersId = contract.CarOwnersId,
                CarOwnersName = (contract.CarOwnersId != null) ? contract.CarOwners1.CarrierName : "",
                BalanceKeeperId = contract.BalanceKeeperId ?? 0,
                BalanceKeeperName = (contract.BalanceKeeperId != null) ? contract.BalanceKeepers.BalanceKeeper : "",
                ExpeditorId = contract.ExpeditorId,
                ExpeditorName = (contract.ExpeditorId != null) ? contract.CarOwners1.CarrierName : "",
                ContractNumber = contract.ContractNumber,
                ContractDate = ContractDate,
                ContractDateRaw = DateTimeConvertClass.getString(contract.ContractDate.Value),
                DateBegRaw = DateTimeConvertClass.getString(contract.DateBeg.Value),
                DateEndRaw = DateTimeConvertClass.getString(contract.DateEnd.Value),
                DateBeg = DateBeg,
                DateEnd = DateEnd,
                IsActive = contract.IsActive ?? true,
                DaysDelay = contract.DaysDelay ?? 0,
                ReceiveDateReal = ReceiveDateReal,
                ReceiveDateRealRaw = ReceiveDateRaw,
                IsForwarder = (contract.BalanceKeeperId != null) ? true : false,
                CanBeDelete = !((contract.OrderUsedCars.Count() > 0) || (contract.OrderUsedCars1.Count() > 0) || (contract.ContractGroupesSpecifications.Count() > 0)),
                CountGroupeSpecifications = contract.ContractGroupesSpecifications.Count(),
                ContractRevision = contract.ContractRevision,
                NDSTax = (contract.NDSTax ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                BackgroundColor = backgroundColor

            };
        }

        public static OrderFilterSettingsModel Map(OrderFilterSettings2 filter)
        {
            return new OrderFilterSettingsModel()
            {
                Id = filter.Id,
                NameFilter = filter.NameFilter,
                PriorityType = filter.PriorityType,
                DeltaDateBeg = filter.DeltaDateBeg,
                DeltaDateEnd = filter.DeltaDateEnd,
                DeltaDateBegEx = filter.DeltaDateBegEx,
                DeltaDateEndEx = filter.DeltaDateEndEx,
                UseStatusFilter = filter.UseStatusFilter,
                UseCreatorFilter = filter.UseCreatorFilter,
                UseExecuterFilter = filter.UseExecuterFilter,
                UseClientFilter = filter.UseClientFilter,
                UseTypeFilter = filter.UseTypeFilter,
                UsePriorityFilter = filter.UsePriorityFilter,
                UseDateFilter = filter.UseDateFilter,
                UseExDateFilter = filter.UseExDateFilter,
                UserCurrentId = filter.IdCurrentUser,
                UseOrderPayerFilter = filter.UseOrderPayerFilter ?? false,
                UseOrderOrgFromFilter = filter.UseOrderOrgFromFilter ?? false,
                UseOrderOrgToFilter = filter.UseOrderOrgToFilter ?? false,
            };
        }
        
        public static UserProfileViewModel Map(UserProfile filter)
        {
            if (filter != null)
            {
                return new UserProfileViewModel()
                {
                    Id = filter.Id,
                    UserId = filter.UserId,
                    UserName = filter.AspNetUsers?.DisplayName,
                    CountryId = filter.CountryId,
                    Country = filter.Countries == null ? null : filter.Countries.Name,
                    City = filter.City,
                    Photo = filter.Photo,
                    LanguageId = filter.LanguageId,
                    AdressFrom = filter.AdressFrom,
                    isFinishStatuses = filter.isFinishStatuses
                };
            }
            else return new UserProfileViewModel();
        }

        public static FAQGroupesViewModel Map(FAQGroupes groupes)
        {
            return new FAQGroupesViewModel()
            {
                Id = groupes.Id,
                NameFAQGroup = groupes.NameFAQGroup
            };
        }

        public static FAQAnswersViewModel Map(FAQAnswers answers)
        {
            return new FAQAnswersViewModel()
            {
                Id = answers.Id,
                Question = answers.Question,
                Answer = answers.Answer,
                GroupeId = answers.GroupId,
                NameFAQGroup = answers.FAQGroupes == null ? null : answers.FAQGroupes.NameFAQGroup
            };
        }

        public static UserMessagesViewModel Map(UserMessages mes)
        {
            return new UserMessagesViewModel()
            {
                Id = mes.Id,
                MessageText = mes.MessageText,
                MessageSubject = mes.MessageSubject,
                DateTimeCreate = mes.DateTimeCreate,
                CreatedFromUser = mes.CreatedFromUser,
                CreatedToUser = mes.CreatedToUser,
                NameCreatedFromUser = mes.AspNetUsers.DisplayName,
                NameCreatedToUser = mes.AspNetUsers1.DisplayName,
                DateTimeOpen = mes.DateTimeOpen,
                IsOpened = mes.DateTimeOpen == null ? false : true,
                OrderId = mes.OrderId
            };
        }

        public static CarOwnersAccessViewModel Map(CarOwners carOwners)
        {
            return new CarOwnersAccessViewModel()
            {
                Id = carOwners.Id,
                CarrierName = carOwners.CarrierName
            };
        }

        public static BaseReportViewModel Map(GetBaseReport_Result o)
       {
           var result = new BaseReportViewModel()
           {
               Id = o.Id,
               OrderDate = o.OrderDate.ToString("dd.MM.yyyy"),
               OrderDateRaw = DateTimeConvertClass.getString(o.OrderDate),
               CreatedByUser = o.CreatedByUser,
               CreatedByUserName = o.CreatorDispalyName,
               CreateDatetime = o.CreateDatetime,
               OrderType = o.OrderType,
               OrderTypename = o.TypeName,
               OrderTypeShortName = o.TypeShortName,
               CurrentOrderStatus = o.CurrentOrderStatus,
               CurrentOrderStatusColor = o.Color,
               CurrentOrderStatusName = o.OrderStatusName,
               CurrentStatusShortName = o.OrderStatusShortName,
               FontColor = o.FontColor,
               BackgroundColor = o.BackgroundColor,
               OrderDescription = o.OrderDescription,
               ClientId = o.ClientId,
               ClientName = o.ClientName,
               ClientCenterName = o.CenterName,
               CanBeDelete = (o.CurrentOrderStatus == 1) || (o.CurrentOrderStatus == 17),
               Summ = o.Summ ?? 0,
               UseNotifications = o.UseNotifications ?? false,
               CreatorContact = o.CreatorContact,
               CreatorPosition = o.CreatorPosition,
               PriorityType = o.PriotityType,
               OrderServiceDatetime = o.OrderServiceDateTime ?? DateTime.Now,
               IconFile = o.IconFile,
               IconDescription = o.IconDescription,
               OrderExecuter = o.OrderExecuter,
               OrderExecuterName = o.ExecutorDisplayName,
               PayerId = o.PayerId ?? 0,
               PayerName = o.PayerName,
               ProjectNum = o.ProjectNum,
               CarNumber = o.CarNumber ?? 0,
               TotalDistanceDescription = o.DistanceDescription,
               TotalCost = (o.TotalPrice ?? 0).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
               TotalDistanceLenght = Convert.ToInt32(o.TotalDistanceLength ?? 0),
               IsTransport = o.IsTransportType ?? false,
               IsPrivateOrder = o.IsPrivateOrder ?? false,
               IsFinishOfTheProcess = o.isFinishOfTheProcess ?? false,
               ReportStatusName = (o.isFinishOfTheProcess ?? false) ? "Финальный статус" : o.OrderStatusName,
               ReportStatusId = (o.isFinishOfTheProcess ?? false) ? 0 : o.CurrentOrderStatus,
               ReportColor = (o.isFinishOfTheProcess ?? false) ? "#666666" : o.BackgroundColor,

                OrderId = o.Id,
             Shipper = o.Shipper,
             ShipperCountryId = o.ShipperCountryId ?? 0,
             //ShipperCountryName = o.Countries1?.Name,
             ShipperCity = o.ShipperCity,
             ShipperAdress = o.ShipperAdress,
             FromShipperDate = o.FromShipperDatetime.Value.ToString("dd.MM.yyyy"),
             FromShipperDateRaw = DateTimeConvertClass.getString(o.FromShipperDatetime.Value),
             FromShipperTime = o.FromShipperDatetime.Value.ToString("HH:mm"),
             FromShipperTimeRaw = DateTimeConvertClass.getString(o.FromShipperDatetime.Value),

             Consignee = o.Consignee,
             ConsigneeCountryId = o.ConsigneeCountryId ?? 0,
             //ConsigneeCountryName = //o.Countries?.Name,
             ConsigneeCity = o.ConsigneeCity,
             ConsigneeAdress = o.ConsigneeAdress,
             ToConsigneeDate = o.ToConsigneeDatetime.Value.ToString("dd.MM.yyyy"),
             ToConsigneeDateRaw = DateTimeConvertClass.getString(o.ToConsigneeDatetime.Value),
             ToConsigneeTime = o.ToConsigneeDatetime.Value.ToString("HH:mm"),
             ToConsigneeTimeRaw = DateTimeConvertClass.getString(o.ToConsigneeDatetime.Value),

             BoxingDescription = o.BoxingDescription,
             TruckDescription = o.TruckDescription,
             TripType = o.TripType ?? 0,
             Weight = (o.Weight ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
             Volume = Convert.ToDouble(o.Volume ?? 0),
             DimenssionL = Convert.ToDouble(o.DimenssionL ?? 0),
             DimenssionW = Convert.ToDouble(o.DimenssionW ?? 0),
             DimenssionH = Convert.ToDouble(o.DimenssionH ?? 0),
             TruckTypeId = o.TruckTypeId ?? 0,
               //TruckTypeName = o.OrderTruckTypes?.TruckTypeName,
               VehicleTypeId = o.VehicleTypeId ?? 0,
             //VehicleTypeName = o.OrderVehicleTypes?.VehicleTypeName,
             LoadingTypeId = o.LoadingTypeId ?? 0,
             //LoadingTypeName = o.OrderLoadingTypes?.LoadingTypeName,
             UnloadingTypeId = o.UnloadingTypeId ?? 0,
             //UnloadingTypeName = o.OrderUnloadingTypes?.UnloadingTypeName,

             ShipperContactPerson = o.ShipperContactPerson,
             ShipperContactPersonPhone = o.ShipperContactPersonPhone,
             ConsigneeContactPerson = o.ConsigneeContactPerson,
             ConsigneeContactPersonPhone = o.ConsigneeContactPersonPhone

        };

           return result;
       }

        public static ContractSpecificationsViewModel Map(ContractSpecifications spec)
        {
            return new ContractSpecificationsViewModel()
            {
                Id = spec.Id,
                GroupeSpecId = spec.GroupeSpecId,
                CreatedByUser = spec.CreatedByUser,
                CreatedByUserName = spec.AspNetUsers?.DisplayName,
                CreateDate = spec.CreateDate.ToString("dd.MM.yyyy"),
                CreateDateRaw = DateTimeConvertClass.getString(spec.CreateDate),
                CarryCapacityId = spec.CarryCapacityId,
                CarryCapacityVal = spec.CarCarryCapacity?.CarryCapacity,
                DeparturePoint = spec.DeparturePoint,
                ArrivalPoint = spec.ArrivalPoint,
                RouteLength = (spec.RouteLength ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                MovingType = spec.MovingType,
                MovingTypeName = spec.MovingType == 1 ? "Фиксированный" : "Свободный",
                RouteTypeId = spec.RouteTypeId,
                RouteTypeName = spec.RouteTypes?.NameRouteType,
                IntervalTypeId = spec.IntervalTypeId,
                IntervalTypeName = spec.RouteIntervalType?.NameIntervalType,
                RateKm = (spec.RateKm ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                RateHour = (spec.RateHour ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                RateMachineHour = (spec.RateMachineHour ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                RateTotalFreight = (spec.RateTotalFreight ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                NDSTax = (spec.NDSTax ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                RouteId = spec.RouteId,
                IsTruck = spec.IsTruck ?? false,
                IsPriceNegotiated = spec.IsPriceNegotiated ?? false,
                TypeSpecId = spec.TypeSpecId,
                TypeSpecName = spec.SpecificationTypes?.SpecificationType,
                NameId = spec.NameId,
                NameSpecification = spec.SpecificationNames?.SpecName,
                TypeVehicleId = spec.TypeVehicleId,
                TypeVehicleName = spec.OrderVehicleTypes?.VehicleTypeName,
                RouteName = spec.RouteName,
                ContractId = spec.ContractGroupesSpecifications.ContractId,
                GenId = 1
            };
        }

        public static CarryCapacitiesViewModel Map(CarCarryCapacity o)
        {
            return new CarryCapacitiesViewModel()
            {
                Id = o.Id,
                CarryCapacity = o.CarryCapacity.ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                CommentCapacity = o.CapacityComment,
                MaxCapacity = (o.MaxCapacity ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA"))
            };
        }

        public static RouteTypesViewModel Map(RouteTypes o)
        {
            return new RouteTypesViewModel()
            {
                Id = o.Id,
                RouteTypeName = o.NameRouteType
            };
        }

        public static RouteIntervalTypesViewModel Map(RouteIntervalType o)
        {
            return new RouteIntervalTypesViewModel()
            {
                Id = o.Id,
                IntervalTypeName = o.NameIntervalType,
                MaxInterval= o.MaxInterval ?? 0
            };
        }

        public static GroupesSpecificationsViewModel Map(ContractGroupesSpecifications spec)
        {
            string backgroundColor = "#a2a7af";
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateEnd = spec.DateEnd;
            DateTime dateTimeInMonth = DateTime.Now.AddDays(30);
            if (spec.DateEnd < DateTime.Now)
            {
                backgroundColor = "#FF0000";
            }
            else
            if ((dateEnd - dateTimeInMonth).TotalDays <= 30)
            {
                backgroundColor = "#FFCC99";
            }
            else
            {
                backgroundColor = "#ffffff";
            }
            return new GroupesSpecificationsViewModel()
            {
                Id = spec.Id,
                CreatedByUser = spec.CreatedByUser,
                CreatedByUserName = spec.AspNetUsers?.DisplayName,
                CreateDate = spec.CreateDate.ToString("dd.MM.yyyy"),
                ContractId = spec.ContractId,
                DateBeg = spec.DateBeg.ToString("dd.MM.yyyy"),
                DateEnd = spec.DateEnd.ToString("dd.MM.yyyy"),
                DateBegRaw = DateTimeConvertClass.getString(spec.DateBeg),
                DateEndRaw = DateTimeConvertClass.getString(spec.DateEnd),
                CreateDateRaw = DateTimeConvertClass.getString(spec.CreateDate),
                NameGroupeSpecification = spec.NameGroupSpec,
                DaysDelay = spec.DaysDelay ?? 0,
                IsActive = spec.IsActive,
                CanBeDelete = !(spec.ContractSpecifications.Count() > 0),
                CountSpecifications = spec.ContractSpecifications.Count(),
                FuelPrice = (spec.FuelPrice ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                ExchangeRateUahRub = (spec.ExchangeRateUahRub ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                NDSTax = (spec.NDSTax ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                BackgroundColor = backgroundColor
            };
        }

        public static RouteViewModel Map(Routes r)
        {           
            return new RouteViewModel()
            {
                Id = r.Id,
                OrgFromId = r.OrgFromId,
                OrgToId = r.OrgToId,
                OrgFromName = r.Organization?.Name,
                OrgToName = r.Organization1.Name,
                RouteDistance = r.RouteDistance.ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                RouteTime = DateTimeConvertClass.getHoursFormat(r.RouteTime),
                OrgFromCity = r.Organization?.City,
                OrgFromCountry = r.Organization?.Countries?.Name,
                OrgToCountry = r.Organization1?.Countries?.Name,
                OrgToCity = r.Organization1?.City,
                OrgFromAddress = r.Organization?.Address,
                OrgToAddress = r.Organization1?.Address,
                OrgFromIsTruck = r.Organization1?.IsTruck ?? false,
                OrgToIsTruck = r.Organization1?.IsTruck ?? false,
                ShortName = r.ShortName
            };
        }
        
         public static OrderCompetitiveListViewModel Map(OrderCompetitiveList o)
        {
            return new OrderCompetitiveListViewModel()
            { 
                Id = o.Id,
                OrderId = o.OrderId,
                ExpeditorName = o.ExpeditorName,
                CarryCapacity = (o.CarryCapacity ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                CarsOffered = o.CarsOffered ?? 0,
                CarsAccepted = o.CarsAccepted ?? 0,
                NDS = (o.NDS ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                CarCostDog = (o.CarCostDog).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                CarCost = o.CarCost.ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                CarCost7 = (o.CarCost7 ?? 0).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                DaysDelay = o.DaysDelay ?? 0,
                DaysDelayStep1 = o.DaysDelayStep1 ?? 0,
                DaysDelayStep2 = o.DaysDelayStep2 ?? 0,
                Prepayment = o.Prepayment ?? 0,
                Prepayment2 = (o.Prepayment2 ?? 0).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                PrepaymentEffect2 = (o.PrepaymentEffect2 ?? 0).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                Comments = o.Comments,
                IsSelectedId = o.IsSelectedId ?? false,
                SpecificationId = o.SpecificationId ?? 0,
                IsChange = o.IsChange ?? false,
                GenId = o.GenId ?? 0               
            };
        }

        public static SpecificationNamesViewModel Map(SpecificationNames o)
        {
            return new SpecificationNamesViewModel()
            {
                Id = o.Id,
                SpecCode = o.SpecCode,
                SpecName = o.SpecName
            };
        }

        public static SpecificationTypesViewModel Map(SpecificationTypes o)
        {
            return new SpecificationTypesViewModel()
            {
                Id = o.Id,
                SpecificationType = o.SpecificationType
            };
        }

        public static OrderBaseProjectsViewModel Map(OrderBaseProjects o)
        {
            return new OrderBaseProjectsViewModel()
            {
                Id = o.Id,
                ProjectId = o.ProjectId,
                OrderId = o.OrderId
            };
        }

        public static OrderAdditionalRoutePointModel Map(AdditionalRoutePoints o)
        {
            return new OrderAdditionalRoutePointModel()
            {
                Id = o.Id,
                RoutePointId = o.RoutePointId,
                OrderId = o.OrderId,
                IsLoading = o.IsLoading,
                NamePoint = o.Organization?.Name,
                CountryPoint = o.Organization?.Countries?.Name,
                CityPoint = o.Organization?.City,
                AddressPoint = o.Organization?.Address,
                IsSaved = true,
                ContactPerson = o.ContactPerson,
                ContactPersonPhone = o.ContactPersonPhone,
                Contacts = o.ContactPerson + " " + o.ContactPersonPhone,
                CityAddress = o.Organization?.City + ", " + o.Organization?.Address,
                NumberPoint = o.NumberPoint??1,
                Latitude = o.Organization?.Latitude ?? 0,
                Longitude =o.Organization?.Longitude ?? 0

        };
        }


        public static ConcursDiscountRateModel Map(ConcursDiscountRate o)
        {
            return new ConcursDiscountRateModel()
            {
                Id = o.Id,
                DiscountRate = (o.DiscountRate ?? 0).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                DateBeg = o.DateBeg == null ? null : o.DateBeg.Value.ToString("dd.MM.yyyy"),
                DateBegRaw = o.DateBeg == null ? null : DateTimeConvertClass.getString(o.DateBeg.Value),                
                DateEnd = o.DateEnd == null ? null : o.DateEnd.Value.ToString("dd.MM.yyyy"),
                DateEndRaw = o.DateEnd == null ? null : DateTimeConvertClass.getString(o.DateEnd.Value)
            };
        }

        public static RoutePointsViewModel Map(RoutePoints o)
        {
            return new RoutePointsViewModel()
            {
                Id = o.Id,
                RoutePointId = o.RoutePointId ?? 0,
                RoutePointTypeId = o.RoutePointTypeId,
                FullNamePointType = o.RoutePointType?.FullNamePointType,
                ShortNamePointType = o.RoutePointType?.ShortNamePointType,
                ContactPerson = o.ContactPerson,
                ContactPersonPhone = o.ContactPersonPhone,                                
                NumberPoint = o.NumberPoint ?? 1,                
                NamePoint = o.Organization?.Name,
                CountryPoint = o.Organization?.Countries?.Name,
                CityPoint = o.Organization?.City,
                AddressPoint = o.Organization?.Address,
                IsSaved = true,
                Contacts = o.ContactPerson + " " + o.ContactPersonPhone,
                CityAddress = o.Organization?.City + ", " + o.Organization?.Address,
                Latitude = o.Organization?.Latitude ?? 0,
                Longitude = o.Organization?.Longitude ?? 0,

        };
        }

        public static RoutePointTypeViewModel Map(RoutePointType o)
        {
            return new RoutePointTypeViewModel()
            {
                Id = o.Id,
                FullNamePointType = o.FullNamePointType,
                ShortNamePointType = o.ShortNamePointType
            };
        }


        public static OrderUsedCarViewModel Map(GetFactCars_Result o)
        {                       
            return new OrderUsedCarViewModel()
            {
                Id = o.Id,
                OrderId = o.OrderId,
                ContractId = o.ContractId ?? 0,
                ContractExpBkId = o.ContractExpBkId ?? 0,
                ContractInfo = o.ContractInfo,
                ContractExpBkInfo = o.ContractNumber + " от " + ((o.ContractDate != null) ? o.ContractDate.Value.ToString("dd.MM.yyyy") : "") + 
                                     "(с " +((o.DateBeg != null) ? o.DateBeg.Value.ToString("dd.MM.yyyy") : "") + 
                                     " по " + ((o.DateEnd != null) ? o.DateEnd.Value.ToString("dd.MM.yyyy") : "") + ")",
                ExpeditorId = o.ExpeditorId ?? 0,
                ExpeditorName = o.CarrierName,
                CarOwnerInfo = o.CarOwnerInfo,
                CarModelInfo = o.CarModelInfo,
                CarRegNum = o.CarRegNum,
                CarCapacity = o.CarCapacity,
                CarDriverInfo = o.CarDriverInfo,
                DriverContactInfo = o.DriverContactInfo,
                CarrierInfo = o.CarrierInfo,
                CarId = o.CarId??0,
                Summ  = o.Summ??0,
                DriverCardInfo = o.DriverCardInfo,
                Comments = o.Comments,
                PlanDistance = (o.PlanDistance ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                PlanTimeWorkDay = o.PlanTimeWorkDay,
                PlanTimeHoliday = o.PlanTimeHoliday,
                BaseRate = (o.BaseRate ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                BaseRateWorkDay = (o.BaseRateWorkDay ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                BaseRateHoliday = (o.BaseRateHoliday ?? 00).ToString(CultureInfo.CreateSpecificCulture("uk-UA")),
                DelayDays = o.DelayDays,
                FactShipperDateTime = o.FactShipperDateTime,
                FactConsigneeDateTime = o.FactConsigneeDateTime,
                FactShipperDate = o.FactShipperDateTime != null ? o.FactShipperDateTime.Value.ToString("dd.MM.yyyy") : "",
                FactShipperDateRaw =  o.FactShipperDateTime != null ? DateTimeConvertClass.getString(o.FactShipperDateTime.Value) : "",
                FactConsigneeDate = o.FactConsigneeDateTime != null ? o.FactConsigneeDateTime.Value.ToString("dd.MM.yyyy") : "",
                FactConsigneeDateRaw =   o.FactConsigneeDateTime != null ? DateTimeConvertClass.getString(o.FactConsigneeDateTime.Value) : "",

                FactShipperTime = o.FactShipperDateTime != null ? o.FactShipperDateTime.Value.ToString("HH:mm") : "",
                FactShipperTimeRaw = o.FactShipperDateTime != null ?  DateTimeConvertClass.getString(o.FactShipperDateTime.Value) : "",
                FactConsigneeTime = o.FactConsigneeDateTime != null ? o.FactConsigneeDateTime.Value.ToString("HH:mm") : "",
                FactConsigneeTimeRaw = o.FactConsigneeDateTime != null ? DateTimeConvertClass.getString(o.FactConsigneeDateTime.Value) : "",
                OrderTypeShortName = o.ShortName,

                RealFactShipperDate = o.FactShipperDateTime != null ? o.FactShipperDateTime.Value.ToString("dd.MM.yyyy") : "",
                RealFactShipperDateRaw =  o.FactShipperDateTime != null ? DateTimeConvertClass.getString(o.FactShipperDateTime.Value) : "",
                RealFactConsigneeDate = o.FactConsigneeDateTime != null ? o.FactConsigneeDateTime.Value.ToString("dd.MM.yyyy") : "",
                RealFactConsigneeDateRaw =   o.FactConsigneeDateTime != null ? DateTimeConvertClass.getString(o.FactConsigneeDateTime.Value) : "",

                RealFactShipperTime = o.FactShipperDateTime != null ? o.FactShipperDateTime.Value.ToString("HH:mm") : "",
                RealFactShipperTimeRaw = o.FactShipperDateTime != null ?  DateTimeConvertClass.getString(o.FactShipperDateTime.Value) : "",
                RealFactConsigneeTime = o.FactConsigneeDateTime != null ? o.FactConsigneeDateTime.Value.ToString("HH:mm") : "",
                RealFactConsigneeTimeRaw = o.FactConsigneeDateTime != null ? DateTimeConvertClass.getString(o.FactConsigneeDateTime.Value) : "",
                        
            };
        }

           public static OrderUsedCarViewModel Map(GetCarModelInfoFilter_Result o)
        {
            return new OrderUsedCarViewModel()
            {
                CarModelInfo = o.ResultFilter,
                Id = o.Id ?? 0
                
            };
        }

             public static OrderUsedCarViewModel Map(GetCarRegNumFilter_Result o)
        {
            return new OrderUsedCarViewModel()
            {
                CarRegNum = o.ResultFilter,
                Id = o.Id ?? 0
                
            };
        }

              public static OrderUsedCarViewModel Map(GetCarCapacityFilter_Result o)
        {
            return new OrderUsedCarViewModel()
            {
                CarCapacity= Int32.Parse(o.ResultFilter),
                Id = o.Id ?? 0
                
            };
        }

           public static OrderUsedCarViewModel Map(GetCarDriverInfoFilter_Result o)
        {
            return new OrderUsedCarViewModel()
            {
                CarDriverInfo= o.ResultFilter,
                Id = o.Id ?? 0
                
            };
        }

          public static OrderUsedCarViewModel Map(GetDriverContactInfoFilter_Result o)
        {
            return new OrderUsedCarViewModel()
            {
                DriverContactInfo = o.ResultFilter,
                Id = o.Id ?? 0
                
            };
        }

          public static OrderUsedCarViewModel Map(GetDriverCardInfoFilter_Result o)
        {
            return new OrderUsedCarViewModel()
            {
                DriverCardInfo = o.ResultFilter,
                Id = o.Id ?? 0
                
            };
        }

          public static OrderUsedCarViewModel Map(GetCommentsFilter_Result o)
        {
            return new OrderUsedCarViewModel()
            {
                Comments = o.ResultFilter,
                Id = o.Id ?? 0
                
            };
        }

        
          public static OrderUsedCarViewModel Map(GetContractInfoFilter_Result o)
        {
            return new OrderUsedCarViewModel()
            {
                ContractInfo = o.ResultFilter,
                Id = o.Id ?? 0
                
            };
        }

        public static OrderUsedCarViewModel Map(GetCarrierInfoFilter_Result o)
        {
            return new OrderUsedCarViewModel()
            {
                CarrierInfo = o.ResultFilter,
                Id = o.Id ?? 0
                
            };
        }

        public static RestViewModel Map(GetInnerOrderNumFilter_Result o)
        {
            return new RestViewModel()
            {
                InnerOrderNum = o.InnerOrderNum,
                idrow = o.idrow ?? 0,
               
            };
        }

        public static RestViewModel Map(GetProductBarcodeFilter_Result o)
        {
            return new RestViewModel()
            {
                BacodesAll = o.ProductBarcode,
                idrow = o.idrow ?? 0,
            };
        }

        public static ProjectTypeViewModel Map(GetOrderProjects_Result o)
        {
            return new ProjectTypeViewModel()
            {
                Id = o.Id,
                Name = o.Code
            };
        }


    }
}
