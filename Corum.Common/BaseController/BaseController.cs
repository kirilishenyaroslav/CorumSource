using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using System.Reflection;
using Microsoft.AspNet.Identity;
using Corum.Models;
using Corum.Models.Toastr;


namespace CorumAdminUI.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }

    public class RequireRequestValueAttribute : ActionMethodSelectorAttribute
    {
        public RequireRequestValueAttribute(string valueName)
        {
            ValueName = valueName;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return (controllerContext.HttpContext.Request[ValueName] != null);
        }
        public string ValueName { get; private set; }
    }

    [NoCache]

    public class CorumBaseController : Controller
    {
        protected ICorumDataProvider context;
        protected string userId;
        protected string displayUserName;
        protected bool isAdmin;
        protected JavaScriptSerializer JsonSerializer;
        public Toastr Toastr { get; set; }

        public CorumBaseController()
        {
            context       = DependencyResolver.Current.GetService<ICorumDataProvider>();

            var emulateMode = false;

            if (System.Web.HttpContext.Current.Session != null)
            {
                if (System.Web.HttpContext.Current.Session["emulationMode"] != null) { emulateMode = (bool)System.Web.HttpContext.Current.Session["emulationMode"]; }

                if (emulateMode)
                {
                    if (System.Web.HttpContext.Current.Session["emulateUser"] != null)
                    {
                        this.userId = (string)System.Web.HttpContext.Current.Session["emulateUser"];
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["emulationMode"] = false;

                        if (System.Web.HttpContext.Current.Session["parentUser"] != null) System.Web.HttpContext.Current.Session.Remove("parentUser");
                        if (System.Web.HttpContext.Current.Session["emulateUser"] != null) System.Web.HttpContext.Current.Session.Remove("emulateUser");

                        //if (User != null) this.userId = User.Identity.GetUserId();
                        this.userId =System.Web.HttpContext.Current.User.Identity.GetUserId();
                    }
                }
                else
                {
                    //if (User!=null) this.userId = User.Identity.GetUserId();
                    this.userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                }
            }
            else
            {
                //if (User != null) this.userId = User.Identity.GetUserId();
                this.userId = System.Web.HttpContext.Current.User.Identity.GetUserId(); 
            }

            this.isAdmin       = false;
            if (!string.IsNullOrEmpty(this.userId))
            {
                var userInfo     = context.getUser(this.userId);
                this.isAdmin     = this.IsAdmin(this.userId);
                displayUserName  = userInfo.displayName;
            }

            Toastr        = new Toastr();

            this.JsonSerializer = new JavaScriptSerializer();
        }

        protected bool IsAdmin(string currentUserId)
        {
            return context.getUser(currentUserId).isAdmin;
        }

        public ToastMessage AddToastMessage(string title, string message, ToastType toastType)
        {
            return Toastr.AddToastMessage(title, message, toastType);
        }




    }

    public class OnlyAdminsAttribute : AuthorizeAttribute
    {
        private string userId;
        protected ICorumDataProvider context;
        private bool IsAdmin; 
        
        public OnlyAdminsAttribute()
        {
            this.context = DependencyResolver.Current.GetService<ICorumDataProvider>();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var emulateMode = false;

            if (System.Web.HttpContext.Current.Session != null)
            {


                if (System.Web.HttpContext.Current.Session["emulationMode"] != null) { emulateMode = (bool)System.Web.HttpContext.Current.Session["emulationMode"]; }


                if (emulateMode)
                {
                    if (System.Web.HttpContext.Current.Session["emulateUser"] != null)
                    {
                        this.userId = (string)System.Web.HttpContext.Current.Session["emulateUser"];
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["emulationMode"] = false;

                        if (System.Web.HttpContext.Current.Session["parentUser"] != null) System.Web.HttpContext.Current.Session.Remove("parentUser");
                        if (System.Web.HttpContext.Current.Session["emulateUser"] != null) System.Web.HttpContext.Current.Session.Remove("emulateUser");

                        this.userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    }

                }
                else
                {
                    this.userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                }
            }
            else
            {
                this.userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            }

            this.IsAdmin = context.IsUserAdmin(this.userId);

            
            return this.IsAdmin;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (this.IsAdmin==false)
                filterContext.Result = new ViewResult { ViewName = "Unauthorized" };
        }
    }

    



}