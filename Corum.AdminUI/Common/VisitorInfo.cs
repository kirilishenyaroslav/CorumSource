using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarnivannAdminUI.Common
{
    public static class VisitorInfo
    {
        public static string GetVisitorIP()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }

            return string.Empty;
        }        
    }
}