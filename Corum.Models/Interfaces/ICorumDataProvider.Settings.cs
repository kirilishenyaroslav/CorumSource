
using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels;
using System;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels.Admin;
using Corum.Models.ViewModels.Cars;

namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        int  getCurrentPageSizeByUser(string userId);
        bool setCurrentPageSizeForUser(string userId, int newPageSize);
        IQueryable<OrderTypeViewModel> getOrderTypes();
        OrderTypeViewModel getOrderType(int Id);
        RouteTypesViewModel getTripType(int Id);
        void UpdateOrderType(OrderTypeViewModel model);
        void AddOrderType(OrderTypeViewModel model);
        void RemoveOrderType(int Id);
        IQueryable<FAQGroupesViewModel> getAvailableFAQGroupes();
        IQueryable<FAQAnswersViewModel> getFAQAnswers(int FAQGroupeId);
        bool NewFAQAnswer(FAQAnswersViewModel model);
        FAQAnswersViewModel getFAQAnswer(int Id);
        bool UpdateFAQAnswer(FAQAnswersViewModel model);
        bool DeleteFAQAnswer(int id);
    }
}
