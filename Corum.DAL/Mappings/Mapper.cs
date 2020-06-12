using System;
using System.Linq;
using Corum.DAL.Entity;
using Corum.Models;
using Corum.Common;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Orders;

namespace Corum.DAL.Mappings
{
    public static class Mapper
    {

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

            vm.AdressFrom              = o.AdressFrom;
            vm.AdressTo                = o.AdressTo; 
            vm.TripDescription         = o.TripDescription;

            vm.CarDetailInfo           = o.CarDetailInfo;
            vm.CarDriverFio            = o.CarDriverFio;
            vm.CarDriverContactInfo    = o.CarDriverContactInfo;
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
                AccessRoleName = o.AspNetRoles.Name
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
                ClientCity = o.ClientCity,
                ClientAddress = o.ClientAddress,
                AccessRoleId = o.AccessRoleId,
                RoleName = o.AspNetRoles.Name,
                CanBeDelete = o.OrdersBase.Count==0
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
                AllowExecuterData = o.AllowEditExecuterData??false
            };
        }

        public static OrderTypeViewModel Map(OrderTypesBase o)
        {
            return new OrderTypeViewModel()
            {
                Id = o.Id,
                TypeName = o.TypeName,
                CanBeDelete = !((o != null) && (o.OrdersBase.Count() > 0))
            };
        }


        public static OrderBaseViewModel Map (OrdersBase o)
        {
            return new OrderBaseViewModel()
            {
                Id                     = o.Id,
                OrderDate              = o.OrderDate.ToString("dd.MM.yyyy"),
                OrderDateRaw           = DateTimeConvertClass.getString(o.OrderDate),
                CreatedByUser          = o.CreatedByUser,
                CreatedByUserName      = o.AspNetUsers.DisplayName,
                CreateDatetime         = o.CreateDatetime,
                OrderType              = o.OrderType,
                OrderTypename          = o.OrderTypesBase.TypeName,
                CurrentOrderStatus     = o.CurrentOrderStatus,
                CurrentOrderStatusColor= o.OrderStatuses.Color,
                CurrentOrderStatusName = o.OrderStatuses.OrderStatusName,
                OrderDescription       = o.OrderDescription,
                ClientId               = o.ClientId,
                ClientName             = o.OrderClients.ClientName,
                CanBeDelete            = o.CurrentOrderStatus==1,
                Summ                   = o.Summ??0,
                UseNotifications       = o.UseNotifications??false,
                CreatorContact         = o.CreatorContact,
                CreatorPosition        = o.CreatorPosition,
                AllowRegData           = o.OrderStatuses.AllowEditRegData??false,
                AllowClientData        = o.OrderStatuses.AllowEditClientData??false,
                AllowExecuterData      = o.OrderStatuses.AllowEditExecuterData??false
            };
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


        public static UserViewModel Map(AspNetUsers user)
        {
            return new UserViewModel()
            {
                userId = user.Id,
                userEmail = user.Email,
                displayName = user.DisplayName ?? user.UserName,
                userPassword = "empty",
                isAdmin = user.AspNetRoles.Any(r => r.Id == GlobalConsts.GetAdminRoleId()),
                twoFactorEnabled = user.TwoFactorEnabled,
                CanBeDelete = true,
                contactPhone =user.ContactPhone,
                postName = user.PostName



            };
        }

        public static RoleViewModel Map(AspNetRoles role)
        {
            return new RoleViewModel()
            {
                roleId = role.Id,
                roleName = role.Name,
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
    }
}
