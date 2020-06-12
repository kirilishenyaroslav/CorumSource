using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorumAdminUI.Controllers
{
    public class ErrorController : CorumBaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
    }
}