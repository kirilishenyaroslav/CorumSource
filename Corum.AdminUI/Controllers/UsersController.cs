
using System.Collections.Generic;
using System.Web.Mvc;
using Corum.Models;
using Corum.Models.ViewModels;
using CorumAdminUI.Common;
using System.Linq;
using Microsoft.AspNet.Identity;
using Corum.Models.ViewModels.Admin;
using CorumAdminUI.Helpers;
using Corum.Models.ViewModels.Orders;
using System.IO;

namespace CorumAdminUI.Controllers
{
    [Authorize]
    public class UsersController : CorumBaseController
    {

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [OnlyAdmins]
        public ActionResult EmulateUser(string userId)
        {
            System.Web.HttpContext.Current.Session["parentUser"]    = System.Web.HttpContext.Current.User.Identity.GetUserId();
            System.Web.HttpContext.Current.Session["emulateUser"]   = userId;

            this.userId              = userId;
            System.Web.HttpContext.Current.Session["emulationMode"] = true;

            return RedirectToAction("Index","Home");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult FinishEmulatinMode()
        {
            System.Web.HttpContext.Current.Session["emulationMode"] = false;

            if (System.Web.HttpContext.Current.Session["parentUser"] != null) System.Web.HttpContext.Current.Session.Remove("parentUser");
            if (System.Web.HttpContext.Current.Session["emulateUser"] != null) System.Web.HttpContext.Current.Session.Remove("emulateUser");

            this.userId              = System.Web.HttpContext.Current.User.Identity.GetUserId();

            return RedirectToAction("Index", "Home");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [OnlyAdmins]
        public ActionResult Users(NavigationInfo navInfo)
        {
            var model = new NavigationResult<UserViewModel>(navInfo, userId)
            {
                DisplayValues = context.getUsers(navInfo.SearchResult)

            };

            model.RequestParams.SearchResult = navInfo.SearchResult;

            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [OnlyAdmins]
        public ActionResult UpdateUser(string userId)
        {
            var userInfo = context.getUser(userId);

            var model = new UserViewModel()
            { 
                userId = userInfo.userId,
                displayName = userInfo.displayName,
                userEmail = userInfo.userEmail,
                isNewPassword = userInfo.isNewPassword,
                userPassword = userInfo.userPassword,
                isAdmin = userInfo.isAdmin,
                twoFactorEnabled = userInfo.twoFactorEnabled,
                postName = userInfo.postName,
                contactPhone = userInfo.contactPhone,
                roles = context.getUserRoles(userId).Where(t => t.assigned == true),
                Dismissed = userInfo.Dismissed
            };
            return View(model);
        }

        [HttpPost]
        [OnlyAdmins]
        public ActionResult UpdateUser(UserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            context.UpdateUser(model);
            return RedirectToAction("Users","Users");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [OnlyAdmins]
        public ActionResult NewUser(UserViewModel model)
        {
            if (model != null){
                model.isNewPassword = true;
                if (string.IsNullOrEmpty(model.displayName))
                {
                    model.displayName = "new user";
                }
                if (string.IsNullOrEmpty(model.userPassword))
                {
                    model.userPassword = "";
                }
                if (string.IsNullOrEmpty(model.userEmail))
                {
                    model.userEmail = "some e-mail";
                }

                if (string.IsNullOrEmpty(model.postName))
                {
                    model.postName = "some post";
                }

                if (string.IsNullOrEmpty(model.contactPhone))
                {
                    model.contactPhone = "some contact phone";
                }
                return View(model);
            }
            var newUser = new UserViewModel()
            {
                isNewPassword = true,
                userPassword = PasswordGenerator.GetUniquePassword(8)
            };
            return View(newUser);
        }

        [HttpGet]
        [OnlyAdmins]
        public ActionResult RemoveUser(string userId)
        {
            context.RemoveUser(userId);
            return RedirectToAction("Users", "Users");
        }

        public ActionResult Roles(string userId)
        {
            var model = new UserRolesViewModel()
            {
                userInfo = context.getUser(userId),
                roles = context.getUserRoles(userId)
            };

            return View(model);
        }

        [OnlyAdmins]
        public ActionResult AssignRoles(string userId, string[] handledItems)
        {
            context.AssignRoles(userId, handledItems);
            return RedirectToAction("Users", "Users");
        }    

        [HttpGet]
        public ActionResult GetUsers(string searchTerm, int pageSize, int pageNum)
        {
            var storages = context.GetUsers(searchTerm, pageSize, pageNum);
            var storagesCount = context.GetUserCount(searchTerm);

            var pagedAttendees = UsersCreatorsVmToSelect2Format(storages, storagesCount);

            return new JsonpResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static Select2PagedResult UsersCreatorsVmToSelect2Format(IEnumerable<UserViewModel> groupItems, int totalRecords)
        {
            var jsonGroupItems = new Select2PagedResult { Results = new List<Select2Result>() };
            foreach (var groupItem in groupItems)
            {
                jsonGroupItems.Results.Add(new Select2Result
                {
                    id = groupItem.userId,
                    text = groupItem.displayName
                });
            }
            jsonGroupItems.Total = totalRecords;
            return jsonGroupItems;
        }

        [HttpPost]
        public ActionResult CloneUser(string ReceiverId, string UserId)
        {            
            context.CloneRolesForUser(ReceiverId, UserId);

            return new JsonpResult
            {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UserProfile(string userId)
        {
            var userInfo = context.getUserProfile(userId);

            UserProfileViewModel model;
            if (userInfo != null)
            {
                 model = new UserProfileViewModel()
                {
                    UserId = userId,
                    UserName = userInfo.UserName,
                    City = userInfo.City,
                    CountryId = userInfo.CountryId,
                    Country = userInfo.Country,
                    AdressFrom = userInfo.AdressFrom,
                    isFinishStatuses = userInfo.isFinishStatuses
                 };
            }
            else
            {
                 model = new UserProfileViewModel()
                {
                    UserId = userId,
                    UserName = context.getUserName(userId)
                 };
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult UserProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            context.UpdateUserProfile(model);
            return RedirectToAction("Index", "Home", new { userId = model.UserId });
            }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UserMessages(NavigationInfo navInfo, bool? isMsgIn)
        {
            var userId = this.userId;
            if (isMsgIn == null)
            {
                isMsgIn = true;
            }

            var model = new UserMessagesNavigationResult<UserMessagesViewModel>(navInfo, userId)
            {
                IsMsgIn = isMsgIn,
                AvailiableMessagesIn = context.getUserMessagesIn(userId),
                AvailiableMessagesOut = context.getUserMessagesOut(userId)

            };
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewMessage(string UserToId, string messageSubject)
        {
            if(UserToId != null)
            {
                messageSubject = ">> " + messageSubject;
            }; 

            var model = new UserMessagesViewModel()
            {
                CreatedFromUser = this.userId,
                NameCreatedFromUser = context.getUserName(this.userId),
                CreatedToUser = UserToId,
                NameCreatedToUser = string.IsNullOrEmpty(UserToId) ? null : context.getUserName(UserToId),
                MessageSubject = messageSubject
            };

            return View(model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewMessage(UserMessagesViewModel model)
        {
            if (context.NewMessage(model))
            {
                var service = new CorumEmailService();
                var RecieverFullInfo = context.getUser(model.CreatedToUser);;
                var msgInfo = model;
                string emailTemplatePath = string.Empty;

                emailTemplatePath = Server.MapPath($"/Templates/NotificationForMessageIn.html");
             
                if ((service != null) && (RecieverFullInfo != null))
                {
                    var message = new OrderNotificationsMessage();

                    if (System.IO.File.Exists(emailTemplatePath))
                    {

                        using (StreamReader reader = new StreamReader(emailTemplatePath))
                        {
                            message.Body = reader.ReadToEnd();
                            message.Subject = "Новое сообщение  "+model.MessageSubject;
                        }

                        var receivers = new List<UserViewModel>();
                        receivers.Add(RecieverFullInfo);

                        Dictionary<string, string> EmailParams = null;

                        EmailParams = new Dictionary<string, string>()
                           {
                           {"{{Id}}",           model.Id.ToString()},
                           {"{{Subject}}",       model.MessageSubject},
                           {"{{MessageText}}",       model.MessageText},
                           {"{{NameCreatedFromUser}}", context.getUserName(model.CreatedFromUser)},
                           {"{{DateTimeCreate}}",        model.DateTimeCreate.ToString()},
                           #if DEBUG
                               { "{{PreviewLink}}",       $"http://uh218479-1.ukrdomen.com/Users/ViewMessage/{model.Id}" },
                           #else
                               { "{{PreviewLink}}",       $"https://corumsource.com/Users/ViewMessage/{model.Id}" },
                           #endif

                        };

                        
                        foreach (KeyValuePair<string, string> pair in EmailParams)
                        {
                            message.Body = message.Body.Replace(pair.Key, pair.Value);
                        }

                        service.SendRequestToEmailAsync(message, receivers);
                    }
                }


                return RedirectToAction("UserMessages", "Users", new { userId = this.userId, IsMsgIn = false });
            }

            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ViewMessage(int Id, bool MsgIn)
        {
            var model = context.getUserMessage(Id);
            model.IsMsgIn = MsgIn;
            if (MsgIn) {
                model.MsgViewIdUser = model.CreatedFromUser;
                model.MsgViewNameUser = model.NameCreatedFromUser;
                model.MsgViewLabelUser = "От";
            } else
            {
                model.MsgViewIdUser = model.CreatedToUser;
                model.MsgViewNameUser = model.NameCreatedToUser;
                model.MsgViewLabelUser = "Кому";
            }
            if ((model.DateTimeOpen == null) && (model.CreatedToUser == this.userId))
            {
                context.UpdateDateMessageOpen(model);
            }

            return View(model);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GetUserCountMessages()
        {
            var Id = this.userId;
            var CountMessages = context.GetUserCountMessages(Id);

            return Json(CountMessages, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult checkEmailExist(string userEmail, string userId)
        {

            var emailCnt = context.checkEmailExist(userEmail, userId);

            return Json(emailCnt < 1);
        }
    }
}