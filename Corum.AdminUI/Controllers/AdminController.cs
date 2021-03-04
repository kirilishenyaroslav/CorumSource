using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Corum.Models;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Admin;
using CorumAdminUI.Helpers;
using Newtonsoft.Json;

namespace CorumAdminUI.Controllers
{
    [Authorize]
    public class AdminController : CorumBaseController
    {
        public ActionResult LoginHistory(NavigationInfo navInfo)
        {
            var model = new NavigationResult<LoginHistoryViewModel>(navInfo, userId)
            {
                DisplayValues = context.getSessionLog()
            };
            return View(model);
        }

        public ActionResult DeleteSnapshot(int snapshotId)
        {
            context.DeleteSnapshot(snapshotId);
            return RedirectToAction("Snapshots", "Admin");
        }
      
        public ActionResult ArchiveSnapshot(int snapshotId)
        {
            context.MakeSnapshotAsArchive(snapshotId);
            return RedirectToAction("Snapshots", "Admin");
        }

        public ActionResult ViewLogSnapshot(NavigationInfo navInfo, int snapshotId)
        {
            var model = new ImportErrorsNavigationResult<ImportError>(navInfo, userId)
            {
                DisplayValues = context.getImportErrors(snapshotId, null),
                errorCommonInfo = new ImportErrorsInfo()
                {
                    logId = null,
                    snapshotId = snapshotId
                }
            };
            return View(model);
        }
        
        public ActionResult DefaultSnapshot(int snapshotId)
        {
            context.MakeSnapshotAsDefault(snapshotId);
            return RedirectToAction("Snapshots", "Admin");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Snapshots(NavigationInfo navInfo)
        {
            var model = new NavigationResult<SnapshotInfoViewModel>(navInfo, userId)
            {
                DisplayValues = context.GetSnapshotLists()
            };
            return View(model);
        }

        public ActionResult UploadFile()
        {
            var model = new UploadFileViewModel
          {
                 IsDocsFile = false,
                 IsRestFile = true,
                 IsOrdersFile = false,
                 CurrentSettings = context.GetImportConfig().ToList(),
                 ColumnNameList = context.GetImportColumnName().ToList(),
                 CurrentSettingsRests = context.GetConfigColumn(true).ToList(),
                 CurrentSettingsDocs = context.GetConfigColumn(false).ToList()
            };
            
            return View(model);
        }

        [HttpPost]
        public ActionResult SetColumnSettings(ColumnSettingsModel newSettings)
        {

            //сюда пришли новые настроки от пользователя. Сохраняем их.

            return RedirectToAction("UploadFile", "Admin");
        }

        [HttpPost]
        public ActionResult UploadFile(UploadFileViewModel model, HttpPostedFileBase ExternalFile)
        {
            //Сохранение файла во временную папку на сервере с временным именем
            var innerFileName = DateTime.Now.Ticks.ToString();
            var fileName = Server.MapPath("~/ImportFiles/" + innerFileName);
            
            ExternalFile.SaveAs(fileName);
            
            //Перенаправляем на форму конфигурирования параметров и заголовков
            return RedirectToAction("ColumnSettings", "Admin", new { RealName=ExternalFile.FileName, ServerFilePath = fileName, fileType = model.FileType });
        }


        public ActionResult ColumnSettings(string RealName, string ServerFilePath, /*bool IsRestFile*/int FileType)
        {   
            var columnsInfo = new ColumnsFromExternalFile
            {
                ServerFileName = ServerFilePath,
                RealName = RealName,
                FileType = FileType
            };
            
            //получение заголовков из csv-файла
            var HeadersCSVFile = CSVFileObject.GetHeaderCSVFile(ServerFilePath);            
            columnsInfo.Headers.Clear();
            columnsInfo.Headers.Add("по умолчанию");
            for (int i = 0; i < HeadersCSVFile.Length; i++)
                if (!String.IsNullOrEmpty(HeadersCSVFile[i]))
                {
                    columnsInfo.Headers.Add(HeadersCSVFile[i].Trim());
                }            

            //получение из базы данных параметры {OurSPparams} ХП в зависимости от типа файла (IsRestFile)
            var templateInfo = context.GetImportTemplateInfo(FileType);
            columnsInfo.InnerSPparams.Clear();
            columnsInfo.InnerSPparamsDict.Clear();            
            for (int i = 0; i < templateInfo.Count; i++)
            {
                columnsInfo.InnerSPparams.Add(templateInfo[i].ColumnNameInDB);
                columnsInfo.InnerSPparamsDict.Add(templateInfo[i].ColumnNameInDB, templateInfo[i].ColumnDescription);
            }                            

            return View(columnsInfo);
        }

        [HttpPost]
        public ActionResult ColumnSettingsCommit(ConfiguredByUserColumsPairsModel preImportConfig)
        {
            //процедура реализаии импорта с логированием в БД тело которой тебе необходимо реализовать

            //получение заголовков из csv-файла
            string[] HeadersCSVFile = CSVFileObject.GetHeaderCSVFile(preImportConfig.ServerFileName);
            //общий массив данных csv-файла
            string[] DataCSVFile = CSVFileObject.GetDataCSVFile(preImportConfig.ServerFileName);
            //получение первой строки CSV-файла с данными
            string[] FirstDataRowCSVFile = CSVFileObject.GetFirstDataRowCSVFile(preImportConfig.ServerFileName);
            
            int newSnapshotId = 0; //необходимо его получать после импорта
            string logGuid = string.Empty; //необходимо его получать после импорта

            var importResult = false;
            if (preImportConfig.FileType == 0 || preImportConfig.FileType == 1)
            {
                importResult = context.DoImport(preImportConfig, HeadersCSVFile, DataCSVFile,
                    FirstDataRowCSVFile, ref logGuid, ref newSnapshotId); //и получить результат
            }
            else if (preImportConfig.FileType == 2)
            {
                importResult = context.DoImportOrders(preImportConfig, HeadersCSVFile, DataCSVFile,
                   FirstDataRowCSVFile, ref logGuid, ref newSnapshotId);
            }
            else
            {
                importResult = context.DoImportTruckOrders(preImportConfig, HeadersCSVFile, DataCSVFile,
                   FirstDataRowCSVFile, ref logGuid, ref newSnapshotId);
            }

            System.IO.File.Delete(preImportConfig.ServerFileName);

            //Редирект на view в зависимости от успешности
            if (importResult){ return RedirectToAction("ImportSucceed", "Admin", new { snapshotId = newSnapshotId }); }
                        else { return RedirectToAction("ImportError", "Admin", new { snapshotId= newSnapshotId, logId = logGuid }); };
            
        }

        public ActionResult ImportSucceed(int snapshotId)
        {
            //Показываем форму успешного завершения процесса
            return View(snapshotId);
        }
        public ActionResult ImportError(NavigationInfo navInfo, string logId, int snapshotId)
        { 
            var model = new ImportErrorsNavigationResult<ImportError>(navInfo, userId)
            {
                DisplayValues = context.getImportErrors(snapshotId, logId),
                errorCommonInfo = new ImportErrorsInfo()
                {
                    logId=logId,
                    snapshotId=snapshotId
                }
            };
            return View(model);
            //Достаем из БД ошибки чтобы показать пользователю
            
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Roles(NavigationInfo navInfo)
        {
            var model = new NavigationResult<RoleViewModel>(navInfo, userId)
              {
               DisplayValues = context.getRoles(navInfo.SearchResult)
              };

            model.RequestParams.SearchResult = navInfo.SearchResult;

            return View(model);
          }

        [HttpGet]
        public ActionResult NewRole(string roleId, int roleGroupsId)
        {
            var model = new RoleViewModel
            {
                roleGroupsId = roleGroupsId
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewRole(RoleViewModel model)
        {                     
            if (context.CheckGroupRolesName(model.roleName))
                ModelState.AddModelError("roleName", "Роль может быть только в одной папке");

            if (!ModelState.IsValid) return View();//PartialView(model);

           context.AddNewRole(model);
           return RedirectToAction("RoleGroups", "Admin");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateRole(string roleId, int roleGroupsId)
        {                        
            var roleInfo = context.getRole(roleId);
            if (roleInfo != null)
                roleInfo.roleGroupsId = roleGroupsId;
            return View(roleInfo);
        }

        [HttpPost]
        public ActionResult UpdateRole(RoleViewModel model)
        {
            if (context.CheckGroupRolesName2(model.roleName, model.roleId))
                ModelState.AddModelError("roleName", "Роль может быть только в одной папке");

            if (!ModelState.IsValid) return PartialView(model);
            context.UpdateRole(model);

            return RedirectToAction("RoleGroups", "Admin");
        }

        [HttpGet]
        public ActionResult RemoveRole(string roleId)
        {
            context.RemoveRole(roleId);
            return RedirectToAction("RoleGroups", "Admin");         
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult RoleGroups()
        {
           var menuItems = context.GetGroupRolesTree();
            
           var model = new ManageGroupRolesAccessViewModel()
            {
                JSONData = JsonConvert.SerializeObject(menuItems)
            };
                                   
            return View(model);    
        }  
      
        public ActionResult RoleGroupRoles(int? roleGroupsId)
        {
            GroupRolesViewModel model = null;
            if (roleGroupsId != null)
            {
             model = new GroupRolesViewModel()
                {
                    GroupRolesInfo = context.GetGroupRoles(roleGroupsId),
                    Roles = context.GetRoleGroupRoles(roleGroupsId)
                };
            }

            return PartialView(model);
        }
        

        [HttpGet]
        public ActionResult NewGroupRole(int? parentId)
        {
            var groupRolesInfo = new RoleGroupsViewModel()
            {   parentId = parentId,
                //AvailableRoleGroups = context.GetGroupRole().ToList(),  
                AvailableRoleGroups = context.GetGroupRolesTree().ToList(),                  
            };

            return PartialView(groupRolesInfo);
        }

        [HttpPost]
        public ActionResult NewGroupRole(RoleGroupsViewModel model)
        {
            context.AddNewGroupRole(model);
            return RedirectToAction("RoleGroups", "Admin");
        }

        [HttpGet]
        public ActionResult RemoveGroupRole(int roleGroupsId)
        {
            context.RemoveGroupRole(roleGroupsId);
            return RedirectToAction("RoleGroups", "Admin");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateGroupRole(int roleGroupsId)
        {
            var roleGroupInfo = context.GetGroupRoles(roleGroupsId);

            var groupRolesInfo = new GroupRolesAccessViewModel()
            {
                Id = roleGroupInfo.Id,
                GroupRolesName = roleGroupInfo.GroupRolesName,
                parentId = roleGroupInfo.parentId,
                IdForScript = roleGroupInfo.IdForScript,
                is_Leaf = roleGroupInfo.is_Leaf,
                isRoot = roleGroupInfo.isRoot,
                //AvailableRoleGroups = context.GetGroupRole().ToList(),
                AvailableRoleGroups = context.GetGroupRolesTree().ToList(),                
            };

            return View(groupRolesInfo);
        }
       

        [HttpPost]
        public ActionResult UpdateGroupRole(GroupRolesAccessViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            context.UpdateGroupRole(model);
            return RedirectToAction("RoleGroups", "Admin");
        }
        

        
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateGroupForRole(string roleId, int roleGroupsId)
        {                                 
            var roleInfo = context.getRole(roleId);
            if (roleInfo != null)
                roleInfo.roleGroupsId = roleGroupsId;

            var roleGroupInfo = context.GetGroupRoles(roleGroupsId);

            var groupRolesInfo = new GroupRolesAccessViewModel()
            {
                Id = roleGroupInfo.Id,
                GroupRolesName = roleGroupInfo.GroupRolesName,
                parentId = roleGroupInfo.parentId,
                IdForScript = roleGroupInfo.IdForScript,
                is_Leaf = roleGroupInfo.is_Leaf,
                isRoot = roleGroupInfo.isRoot,
                AvailableRoleGroups = context.GetLeafGroupRolesTree().ToList(),
            };

            var groupRole = new RoleGroupViewModel()
            {
                GroupRolesInfo = groupRolesInfo,
                Roles = roleInfo
            };

            return View(groupRole);       
        }                       
        
        [HttpPost]
        public ActionResult UpdateGroupForRole(RoleGroupViewModel model)
        {
            context.UpdateGroupForRole(model);
            return RedirectToAction("RoleGroups", "Admin");
        }

        [HttpGet]
        public JsonResult CheckGroupRolesName(string roleName)
        {
            var result = !context.CheckGroupRolesName(roleName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckGroupRolesName2(string roleName, string roleId)
        {
            var result = !context.CheckGroupRolesName2(roleName, roleId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult FAQ(FAQNavigationInfo navInfo)
        {
            var faqGroupes = context.getAvailableFAQGroupes().ToList();
            var groupeId = (navInfo.GroupeId != null) ? navInfo.GroupeId.Value : faqGroupes.FirstOrDefault().Id;
            var displayValues = context.getFAQAnswers(groupeId).ToList();

            var model = new FAQNavigationResult<FAQAnswersViewModel>(navInfo, userId)
            {
              DisplayValues = displayValues.AsQueryable(),
              AvailiableGroupes = faqGroupes,
              GroupeId = groupeId
            };
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewFAQAnswer(int GroupeId)
        {
            var groupes = context.getAvailableFAQGroupes().ToList();
            var model = new FAQAnswersViewModel()
            {
                GroupeId = GroupeId,
                NameFAQGroup = groupes.FirstOrDefault(t => t.Id == GroupeId).NameFAQGroup
            };

            return View(model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult NewFAQAnswer(FAQAnswersViewModel model)
        {
            if (context.NewFAQAnswer(model))
            {
                return RedirectToAction("FAQ", "Admin", new { GroupId = model.GroupeId });
            }
       
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateFAQAnswer(int Id)
        {
            var model = context.getFAQAnswer(Id);

            return View(model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult UpdateFAQAnswer(FAQAnswersViewModel model)
        {
            if (context.UpdateFAQAnswer(model))
            {
                return RedirectToAction("FAQ", "Admin", new { GroupeId = model.GroupeId });
            }

            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteFAQAnswer(int Id)
        {
            context.DeleteFAQAnswer(Id);
            return RedirectToAction("FAQ", "Admin");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult FAQHelpUsers(int groupeId, FAQNavigationInfo navInfo)
        {
            var faqGroupes = context.getAvailableFAQGroupes().ToList();
            var displayValues = context.getFAQAnswers(groupeId).ToList();
            var nameFAQGroup = faqGroupes.FirstOrDefault(t => t.Id == groupeId).NameFAQGroup;

            var model = new FAQNavigationResult<FAQAnswersViewModel>(navInfo, userId)
            {
                DisplayValues = displayValues.AsQueryable(),
                AvailiableGroupes = faqGroupes,
                GroupeId = groupeId,
                NameFAQGroup = nameFAQGroup
            };
            return View(model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult ShowFAQAnswer(int Id)
        {
            var model = context.getFAQAnswer(Id);

            return View(model);
        }
    }
}