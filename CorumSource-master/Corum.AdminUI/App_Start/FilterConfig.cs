using System.Web;
using System.Web.Mvc;
using BarnivannAdminUI.Common;

namespace BarnivannAdminUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MessagesActionFilter());
        }
    }
}
