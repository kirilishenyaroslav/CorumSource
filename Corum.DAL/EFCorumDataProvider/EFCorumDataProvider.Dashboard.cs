using System;
using System.Collections.Generic;
using System.Linq;
using Corum.Models;
using Corum.DAL.Mappings;
using Corum.DAL.Entity;
using Corum.Models.ViewModels.Dashboard;

namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {
        public EFCorumDataProvider(Entities dbContext):base(dbContext)
        {

        }

        public List<DashboardViewModelItem> getBPInfoByUser(DateTime dateStart, string userId, bool isAdmin=false, bool isFinishStatuses=false)
        {
            
            var queryTr = db.GetOrdersPipelineV3(userId,
                                             isAdmin,
                                             true,                //IsTransport,
                                             false,               //UseStatusesFilter,
                                             false,               //UseOrderCreatorFilter,
                                             false,               //UseOrderExecuterFilter,
                                             false,               //UseOrderTypeFilter,
                                             false,               //UseOrderClientFilter,
                                             false,               //UseOrderPriorityFilter,
                                             true,                //UseOrderDateFilter,
                                             false,               //UseOrderExDateFilter,
                                             false,               //UseOrderEndDateFilter,
                                             false,               //UseOrderFinalStatusFilter
                                             false,               //UseOrderProjectFilter
                                             false,                 //UseOrderPayerFilter
                                             false,                 //UseOrderOrgFromFilter
                                             false,                  //UseOrderOrgToFilter
                                             "0",                   //_FilterStatusId,
                                             string.Empty,        //FilterOrderCreatorId,
                                             string.Empty,        //FilterOrderExecuterId,
                                             "0",                   //_FilterOrderTypeId,
                                             "0",                   //_FilterOrderClientId,
                                             0,                   //FilterOrderPriority,
                                             dateStart,           //FilterOrderDateBeg,
                                             DateTime.MaxValue,   //FilterOrderDateEnd,
                                             DateTime.MinValue,   //FilterOrderExDateBeg,
                                             DateTime.MaxValue,
                                             DateTime.MinValue,   //FilterOrderEndDateBeg,
                                             DateTime.MaxValue,   //FilterOrderEndDateEnd,
                                             false,"0",                                             
                                             "0",                   //FilterOrderPayerId
                                             "0",                   //FilterOrderOrgFromId                                             
                                             "0"                   //FilterOrderOrgToId                                             
                                             )   //FilterOrderExDateEnd).ToList().AsQueryable();
                                             .ToList();

            var queryBs = db.GetOrdersPipelineV3(userId,
                                             isAdmin,
                                             false,                //IsTransport,
                                             false,               //UseStatusesFilter,
                                             false,               //UseOrderCreatorFilter,
                                             false,               //UseOrderExecuterFilter,
                                             false,               //UseOrderTypeFilter,
                                             false,               //UseOrderClientFilter,
                                             false,               //UseOrderPriorityFilter,
                                             true,                //UseOrderDateFilter,
                                             false,               //UseOrderExDateFilter,
                                             false,               //UseOrderEndDateFilter,
                                             false,               //UseOrderFinalStatusFilter,
                                             false,               //UseOrderProjectFilter
                                             false,                 //UseOrderPayerFilter
                                             false,                 //UseOrderOrgFromFilter
                                             false,                  //UseOrderOrgToFilter
                                             "0",                   //_FilterStatusId,
                                             string.Empty,        //FilterOrderCreatorId,
                                             string.Empty,        //FilterOrderExecuterId,
                                             "0",                   //_FilterOrderTypeId,
                                             "0",                   //_FilterOrderClientId,
                                             0,                   //FilterOrderPriority,
                                             dateStart,           //FilterOrderDateBeg,
                                             DateTime.MaxValue,   //FilterOrderDateEnd,
                                             DateTime.MinValue,   //FilterOrderExDateBeg,
                                             DateTime.MaxValue,
                                             DateTime.MinValue,   //FilterOrderEndDateBeg,
                                             DateTime.MaxValue,
                                             false,
                                             "0",                                            
                                             "0",                   //FilterOrderPayerId
                                             "0",                   //FilterOrderOrgFromId                                             
                                             "0"                   //FilterOrderOrgToId                                             
                                             )   //FilterOrderExDateEnd).ToList().AsQueryable();
                                             .ToList();

            var query = queryTr.Union(queryBs).Where(x => x.CurrentOrderStatus != 17);

            var enabledTypes = query
                                 .GroupBy(x => new { x.OrderType, x.TypeName, x.TypeShortName, x.IsTransportType })
                                 .Select(x => new
                                 {
                                     OrderType = x.Key.OrderType,
                                     TypeName = x.Key.TypeName,
                                     TypeShortName = x.Key.TypeShortName,
                                     IsTransportType  = x.Key.IsTransportType
                                 }).ToList();


            

            var result = new List<DashboardViewModelItem>();

            foreach (var typeItem in enabledTypes)
            {

                var item = new DashboardViewModelItem()
                {
                    OrderType = typeItem.OrderType,
                    OrderTypeName = typeItem.TypeName,
                    OrderTypeShortName = typeItem.TypeShortName,
                    IsTransportType = typeItem.IsTransportType??false
                };


                if (isFinishStatuses)
                {
                    item.BPInfo =
                                  query
                                   .Where(x => x.OrderType == typeItem.OrderType)
                                    .Select(Mapper.Map)
                                     .Where(x=>x.IsFinishOfTheProcess==true)
                                     .GroupBy(x => new { x.CurrentOrderStatusName, x.CurrentOrderStatus, x.BackgroundColor })
                                       .Select(x => new BPItemInfoViewModel
                                       {
                                           ItemName = x.Key.CurrentOrderStatusName,
                                           StatusId = x.Key.CurrentOrderStatus,
                                           OrderCount = x.Count(),
                                           Color = x.Key.BackgroundColor
                                       }).ToList();
                }
                else
                {
                    item.BPInfo =
                                  query
                                   .Where(x => x.OrderType == typeItem.OrderType)
                                    .Select(Mapper.Map)
                                     .GroupBy(x => new { x.ReportStatusName, x.ReportStatusId, x.ReportColor })
                                       .Select(x => new BPItemInfoViewModel
                                       {
                                           ItemName = x.Key.ReportStatusName,
                                           StatusId = x.Key.ReportStatusId,
                                           OrderCount = x.Count(),
                                           Color = x.Key.ReportColor
                                       }).ToList();
                }


                var TotalCount = item.BPInfo.Count();

                foreach (var item_ in item.BPInfo)
                {
                    unchecked
                    {
                        item_.Percent = Convert.ToInt32(Math.Truncate((TotalCount > 0) ? (Convert.ToDecimal(item_.OrderCount) / Convert.ToDecimal(TotalCount)) * 100 : 0));
                    }
                }

                result.Add(item);
            }

            return result;
        }

        public bool getFinishStatusesByUserId(string userId)
        {
            bool isFinishStatuses = false;
            isFinishStatuses = Mapper.Map(db.UserProfile.FirstOrDefault(c => c.UserId == userId)).isFinishStatuses;            

            return isFinishStatuses;
        }

    }
}
