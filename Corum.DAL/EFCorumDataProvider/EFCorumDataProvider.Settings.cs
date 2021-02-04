using System;
using System.Linq;
using Corum.Models;
using Corum.DAL.Entity;
using Corum.DAL.Mappings;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels.Admin;
using Corum.Models.ViewModels.Cars;

namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {
        public int getCurrentPageSizeByUser(string userId)
        {
            var userSettings = db.UserSettings.SingleOrDefault(s => s.userId == userId);
            
            return (userSettings!=null)? userSettings.PageSize:5;
        }
        public bool setCurrentPageSizeForUser(string userId, int newPageSize)
        {
            try
            {
                var userSettings = db.UserSettings.SingleOrDefault(s => s.userId == userId);
                if (userSettings != null)
                {
                    userSettings.PageSize = newPageSize;
                    db.SaveChanges();
                }
                else
                {
                    var newUserSettings = new UserSettings()
                    {
                        userId = userId,
                        PageSize = newPageSize
                    };
                    db.UserSettings.Add(newUserSettings);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
            
        }
        public IQueryable<OrderTypeViewModel> getOrderTypes()
        {
            return db.OrderTypesBase.AsNoTracking().Select(Mapper.Map).AsQueryable();
        }

        public OrderTypeViewModel getOrderType(int Id)
        {
            return Mapper.Map(db.OrderTypesBase.AsNoTracking().FirstOrDefault(u => u.Id == Id));
        }


        public RouteTypesViewModel getTripType(int Id)
        {
            return Mapper.Map(db.RouteTypes.AsNoTracking().FirstOrDefault(u => u.Id == Id));
        }

        public void UpdateOrderType(OrderTypeViewModel model)
        {
            var dbInfo = db.OrderTypesBase.FirstOrDefault(u => u.Id == model.Id);
            if (dbInfo == null) return;

            dbInfo.TypeName                  = model.TypeName;
            dbInfo.ShortName                 = model.ShortName;
            dbInfo.UserRoleIdForClientData   = model.UserRoleIdForClientData;
            dbInfo.UserRoleIdForExecuterData = model.UserRoleIdForExecuterData;
            dbInfo.DefaultExecuterId         = model.DefaultExecuterId;
            dbInfo.UserForAnnonymousForm     = model.UserIdForAnonymousForm;
            dbInfo.TypeAccessGroupId         = model.UserRoleIdForTypeAccess;
            dbInfo.IsTransportType           = model.IsTransportType;
            dbInfo.UserRoleIdForCompetitiveList = model.UserRoleIdForCompetitiveList;
            dbInfo.IsActive = model.IsActive;

            db.SaveChanges();
        }

        public void AddOrderType(OrderTypeViewModel model)
        {
           var OrderTypeInfo = new OrderTypesBase()
            {
               TypeName = model.TypeName,
               UserRoleIdForClientData = model.UserRoleIdForClientData,
               UserRoleIdForExecuterData = model.UserRoleIdForExecuterData,               
               DefaultExecuterId = model.DefaultExecuterId,
               UserForAnnonymousForm = model.UserIdForAnonymousForm,
               IsActive = model.IsActive,
               IsTransportType = model.IsTransportType,
               UserRoleIdForCompetitiveList = model.UserRoleIdForCompetitiveList,
           };

            db.OrderTypesBase.Add(OrderTypeInfo);
            db.SaveChanges();
        }

        public void RemoveOrderType(int Id)
        {
            var dbInfo = db.OrderTypesBase.FirstOrDefault(u => u.Id == Id);
            if (dbInfo == null) return;

            db.OrderTypesBase.Remove(dbInfo);
            db.SaveChanges();
        }

        public IQueryable<FAQGroupesViewModel> getAvailableFAQGroupes()
        {
            return db.FAQGroupes
                            .AsNoTracking()
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.Id)
                               .AsQueryable();
        }

        public IQueryable<FAQAnswersViewModel> getFAQAnswers(int FAQGroupeId)
        {

            var query = (from a in db.FAQAnswers
                         join gr in db.FAQGroupes on a.GroupId equals gr.Id
                         where a.GroupId == FAQGroupeId
                         select a).Distinct();

            return query.Select(Mapper.Map).OrderBy(o => o.Id).AsQueryable();

        }

        public bool NewFAQAnswer(FAQAnswersViewModel model)
        {
            try
            {

                var faqInfo = new FAQAnswers();

                if (faqInfo != null)
                {
                    faqInfo.Answer = model.Answer;
                    faqInfo.Question = model.Question;
                    faqInfo.GroupId = model.GroupeId;

                    db.FAQAnswers.Add(faqInfo);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public FAQAnswersViewModel getFAQAnswer(int Id)
        {
            return Mapper.Map(db.FAQAnswers.FirstOrDefault(or => or.Id == Id));
        }

        public bool UpdateFAQAnswer(FAQAnswersViewModel model)
        {
            try
            {
                var faqAnswers = db.FAQAnswers.FirstOrDefault(p => p.Id == model.Id);

                if (faqAnswers != null)
                {
                    faqAnswers.Answer = model.Answer;
                    faqAnswers.Question = model.Question;
                    faqAnswers.GroupId = model.GroupeId;

                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }

        }

        public bool DeleteFAQAnswer(int id)
        {
            var faqAnswers = db.FAQAnswers.FirstOrDefault(o => o.Id == id);

            if (faqAnswers != null)
            {
                db.FAQAnswers.Remove(faqAnswers);
                db.SaveChanges();
            }
            return true;

        }
    }
}
