using CorumAdminUI.Common;
using Corum.Models.ViewModels.Cars;
using System;
using Corum.Models.ViewModels;
using Corum.Common;
using Corum.Models.Tender;
using Corum.Models.ViewModels.Tender;
using System.Collections;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using CorumAdminUI;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CorumAdminUI.Controllers
{
    public class FormContragentsController : CorumBaseController
    {
        // GET: FormContragents
        public ActionResult SendFormToCorumSource()
        {
            return View();
        }
    }
}