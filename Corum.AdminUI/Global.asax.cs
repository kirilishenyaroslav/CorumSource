using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Corum.Common;
using GemBox.Spreadsheet;
using Corum.Models.ViewModels.Orders;
using CorumAdminUI.CustomBinders;

namespace BarnivannAdminUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            DependencyResolver.SetResolver(new CorumNinjectResolver());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SpreadsheetInfo.SetLicense("E3JO-TSKL-8873-EJJM");


            ModelBinders.Binders.Add(typeof(OrderBaseViewModel), new OrdersBaseViewModelBinder());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex is HttpAntiForgeryException)
            {
                Response.Clear();
                Server.ClearError(); //make sure you log the exception first
                Response.Redirect("/Account/Login", true);
            }
            else
            {            
                Response.Clear();
                Server.ClearError(); //make sure you log the exception first
                Response.Redirect("/Error", true);
            }

            
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Response != null)
                {
                    var r = HttpContext.Current.Response;

                    r.Headers.Remove("X-Powered-By");
                    r.Headers.Remove("X-AspNet-Version");
                    r.Headers.Remove("X-AspNetMvc-Version");
                    r.Headers.Remove("Server");
                }
            }
            catch { }
        }

    }
}
