using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Corum.Common;
using Corum.Models;
using Corum.Models.Toastr;
using Corum.Models.ViewModels.Orders;



namespace CorumAdminUI.Controllers
{
    [Authorize]
    public class ProjectsController : CorumBaseController
    {
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ProjectsList (ProjectNavigationInfo navInfo)
        {
            var model = new ProjectNavigationResult<OrderProjectViewModel>(navInfo, userId)
            {
                DisplayValues = context.GetProjects(),
                Code = navInfo.Code,
                Description = navInfo.Description,
                ProjectTypeName = navInfo.ProjectTypeName,
                ProjectCFOName = navInfo.ProjectCFOName,
                ProjectOrderer = navInfo.ProjectOrderer,
                ConstructionDesc = navInfo.ConstructionDesc,
                PlanCount = navInfo.PlanCount,
                ManufacturingEnterprise = navInfo.ManufacturingEnterprise,
                isActive = navInfo.isActive,
                Comments = navInfo.Comments,
                NumOrder = navInfo.NumOrder,
                DateOpenOrder = navInfo.DateOpenOrder,
                PlanPeriodForMP = navInfo.PlanPeriodForMP,
                PlanPeriodForComponents = navInfo.PlanPeriodForComponents,
                PlanPeriodForSGI = navInfo.PlanPeriodForSGI,
                PlanPeriodForTransportation = navInfo.PlanPeriodForTransportation,
                PlanDeliveryToConsignee = navInfo.PlanDeliveryToConsignee,
                DeliveryBasic = navInfo.DeliveryBasic,
                ShipperName = navInfo.ShipperName,
                ConsigneeName = navInfo.ConsigneeName
        };
            return View(model);
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [HttpGet]
        public ActionResult NewProject ()
        {
            return View(new OrderProjectViewModel()
            {
               AvailableCFOs         = context.getCenters(userId).ToList(),
               AvailableProjectTypes = context.getProjectTYpes().ToList()
            });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [HttpPost]
        public ActionResult NewProject(OrderProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            context.NewProject(model);
            return RedirectToAction("ProjectsList","Projects",
                 new RouteValueDictionary { { "grid-init", "1" },
                                { "Code", "true" },
                                { "Description", "true" },
                                { "ProjectTypeName", "true" },
                                { "ProjectCFOName", "true" },
                                { "ProjectOrderer", "true" },
                                { "ConstructionDesc", "true" },
                                { "PlanCount", "true" },
                                { "ManufacturingEnterprise", "true" },
                                { "isActive", "true" } });
            //AddToastMessage("Инфо", "Информация о проекте успешно сохранена!", toastType: ToastType.Success);

           // return RedirectToAction("NewProject", "Projects", new { Id = model.Id });
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [HttpGet]
        public ActionResult UpdateProject(int Id)
        {
            var projectInfo = context.GetProjectById(Id);
            projectInfo.AvailableCFOs = context.getCenters(userId).ToList();
            projectInfo.AvailableProjectTypes = context.getProjectTYpes().ToList();

            return View(projectInfo);
        }

        
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrdersProject(int Id)
        {                    
            return RedirectToAction("Orders", "Orders", new {UseOrderProjectFilter = true, FilterOrderProjectId = Id});
        }
        
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult OrdersBaseProject(int Id)
        {                    
            return RedirectToAction("OrdersBase", "Orders", new {UseOrderProjectFilter = true, FilterOrderProjectId = Id});
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult RestReportProject(string Id, string Code)
        {                    
            return RedirectToAction("RestReport", "Reports", new {UseOrderProjectFilter = true, FilterOrderProjectId = Code});
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult GroupRestProject(string Id, string Code)
        {                    
            return RedirectToAction("GroupRestReport", "Reports", new {UseOrderProjectFilter = true, FilterOrderProjectId = Code});
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [HttpPost]
        public ActionResult UpdateProject(OrderProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            context.UpdateProject(model);
            AddToastMessage("Инфо", "Информация о проекте успешно сохранена!", toastType: ToastType.Success);
            return RedirectToAction("UpdateProject", "Projects", new { Id = model.Id });
            //return RedirectToAction("ProjectsList", "Projects");
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [HttpGet]
        public ActionResult RemoveProject(int Id)
        {
            context.RemoveProject(Id);
            return RedirectToAction("ProjectsList", "Projects",
                 new RouteValueDictionary { { "grid-init", "1" },
                                { "Code", "true" },
                                { "Description", "true" },
                                { "ProjectTypeName", "true" },
                                { "ProjectCFOName", "true" },
                                { "ProjectOrderer", "true" },
                                { "ConstructionDesc", "true" },
                                { "PlanCount", "true" },
                                { "ManufacturingEnterprise", "true" },
                                { "isActive", "true" } });
        }

    }
}