using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Corum.Models;


namespace Corum.Helpers
{
    public static class ExtensionMethods
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return file != null && file.ContentLength > 0;
        }
    }

    

    

}