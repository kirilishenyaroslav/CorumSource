using System.Collections.Generic;
using System.Linq;
using Corum.DAL.Mappings;
using Corum.Models;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Admin;
using System;
using Corum.DAL.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using Corum.DAL.Entity;
using System.Globalization;


namespace Corum.DAL
{
    public partial class EFCorumDataProvider : EFBaseCorumDataProvider, ICorumDataProvider
    {

        public string GetDisplayName(string _userId)
        {
            var user = this.getUser(_userId);
            return (user == null) ? string.Empty : user.displayName ?? user.userEmail;
        }


        public bool AddSessionInfo(LoginHistoryViewModel model)
        {
            var userInfo = db.AspNetUsers.FirstOrDefault(u => u.Email == model.UserEmail);

            if (userInfo != null)
            {
                db.LoginHistory.Add(new LoginHistory()
                {
                    Datetime = DateTime.Now,
                    IP = model.IP,
                    userId = userInfo.Id,
                    userAgent = model.UserAgent
                });
            }

            db.SaveChanges();

            return true;
        }


        public IQueryable<LoginHistoryViewModel> getSessionLog()
        {
            return db.LoginHistory
                          .AsNoTracking()
                            .Select(Mapper.Map)
                             .OrderByDescending(o => o.Datetime)
                              .AsQueryable();

        }


        public void UpdateUserDisplayName(string userId, string displayName, string postName, string contactPhone)
        {
            var user = db.AspNetUsers.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.DisplayName = displayName;
                user.TwoFactorEnabled = true;
                user.PostName = postName;
                user.ContactPhone = contactPhone;

                db.SaveChanges();
            }
        }

        public IQueryable<ImportError> getImportErrors(int idSnapshot, string logId)
        {
            if (logId == null)
                return db.LogImportErrors
                    .AsNoTracking()
                     .Where(e => e.idSnapshot == idSnapshot)
                     .OrderBy(e => e.guidSession)
                      .Select(Mapper.Map)
                       .AsQueryable();
            else
                return db.LogImportErrors
                .AsNoTracking()
                 .Where(e => e.idSnapshot == idSnapshot)
                  .Where(e => e.guidSession == logId)
                  .Select(Mapper.Map)
                   .AsQueryable();

        }



        public bool DeleteSnapshot(int Id)
        {
            var delete_item = db.LogisticSnapshots.FirstOrDefault(s => s.id_spanshot == Id);
            if (delete_item != null)
            {
                db.LogisticSnapshots.Remove(delete_item);
                db.SaveChanges();
            }

            return true;
        }

        public bool MakeSnapshotAsArchive(int Id)
        {
            var mark_item = db.LogisticSnapshots.FirstOrDefault(s => s.id_spanshot == Id);
            if (mark_item != null)
            {
                mark_item.isDefaultForReports = 0;
                db.SaveChanges();
            }
            return true;
        }

        public int GetDefaultSnapshotId()
        {
            var snapshot = db.LogisticSnapshots.FirstOrDefault(s=>s.isDefaultForReports == 1);
            if (snapshot != null) return snapshot.id_spanshot;
            return 0;
        }

        public bool MakeSnapshotAsDefault(int Id)
        {
            db.SetSnapshotAsDefaultForReports(Id);
            db.SaveChanges();
            return true;
        }

        public bool GetDateSnapshot(int IdSnapshot, ref string ActualDateBeg, ref string ActualDateEnd)
        {
            DateTime ActualDateBegAsDate;
            DateTime ActualDateEndAsDate;
            var GetDate = db.GetDateSnapshot(IdSnapshot).FirstOrDefault();
            ActualDateBegAsDate = GetDate.ActualDateBeg.Value;
            ActualDateEndAsDate = GetDate.ActualDateEnd.Value;
            ActualDateBeg = ActualDateBegAsDate.ToString("dd.MM.yyyy");
            ActualDateEnd = ActualDateEndAsDate.ToString("dd.MM.yyyy");
            return true;
        }

        public bool GetCommentColumnName(bool IsRest, string ColumnName, ref string CommentColumnName)
        {
            CommentColumnName = "";
            CultureInfo provider = CultureInfo.InvariantCulture;
            var GetComment = db.SelectCommentFieldForReport(IsRest, ColumnName).FirstOrDefault();
            CommentColumnName = GetComment.ColumnDescription;
            return true;
        }

        public IQueryable<UserViewModel> getUsers(string Search)
        {
            var search = (Search != null) ? Search : string.Empty;

            var users = db.AspNetUsers.Where(x => x.Dismissed == false);

            return users
              .AsNoTracking()
               .Where(u => (((search == "") || ((search != "") && ((u.UserName.ToUpper().StartsWith(search.ToUpper()))
                                                                                                || (u.UserName.ToUpper().Contains(search.ToUpper()))
                                                                                                || (u.UserName.ToUpper().EndsWith(search.ToUpper())))))
                             || ((search == "") || ((search != "") && ((u.PostName.ToUpper().StartsWith(search.ToUpper()))
                                                                                                || (u.PostName.ToUpper().Contains(search.ToUpper()))
                                                                                                || (u.PostName.ToUpper().EndsWith(search.ToUpper())))))
                             || ((search == "") || ((search != "") && ((u.ContactPhone.ToUpper().StartsWith(search.ToUpper()))
                                                                                                || (u.ContactPhone.ToUpper().Contains(search.ToUpper()))
                                                                                                || (u.ContactPhone.ToUpper().EndsWith(search.ToUpper())))))
                             || ((search == "") || ((search != "") && ((u.Email.ToUpper().StartsWith(search.ToUpper()))
                                                                                                || (u.Email.ToUpper().Contains(search.ToUpper()))
                                                                                                || (u.Email.ToUpper().EndsWith(search.ToUpper())))))
                             || ((search == "") || ((search != "") && ((u.DisplayName.ToUpper().StartsWith(search.ToUpper()))
                                                                                                || (u.DisplayName.ToUpper().Contains(search.ToUpper()))
                                                                                                || (u.DisplayName.ToUpper().EndsWith(search.ToUpper())))))))
                .Select(Mapper.Map)
                 .AsQueryable();
        }

        public IQueryable<UserViewModel> getUsersForClone(string userId)
        {
            return db.AspNetUsers
                .AsNoTracking()
                 .Where(u => u.Id != userId)
                  .Select(Mapper.Map)
                   .AsQueryable();
        }

        public UserViewModel getUser(string userId)
        {
            return Mapper.Map(db.AspNetUsers.AsNoTracking().FirstOrDefault(u => u.Id == userId));
        }

        public void AddRole(string userId, string roleId)
        {
            var adminRole = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId);
            var userInfo = db.AspNetUsers.FirstOrDefault(u => u.Id == userId);
            if ((adminRole == null) || (userInfo == null)) return;
            {
                if (UserHasRole(userId, roleId)) return;
                userInfo.AspNetRoles.Add(adminRole);
                db.SaveChanges();
            }
        }

        public void RemoveRole(string userId, string roleId)
        {
            var adminRole = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId);
            var userInfo = db.AspNetUsers.FirstOrDefault(u => u.Id == userId);
            if ((adminRole == null) || (userInfo == null)) return;
            {
                if ((!UserHasRole(userId, roleId))) return;
                userInfo.AspNetRoles.Remove(adminRole);
                db.SaveChanges();
            }
        }

        public bool UserHasRole(string userId, string roleId)
        {
            var userInfo = db.AspNetUsers.FirstOrDefault(u => u.Id == userId);
            return ((userInfo != null) && (userInfo.AspNetRoles.Any(r => r.Id == roleId)));
        }

        public bool IsUserAdmin(string userId)
        {
            return UserHasRole(userId, "1000");
        }

        public IEnumerable<MenuAccessViewModel> MenuUserRole(string userId)
        {
            //1) Админу видно все пункты меню
            if (!IsUserAdmin(userId))
                //Если на корневом элементе сконфигурировано - то доступ давать для всех подменю,
                //если нет, проверять права на самом элементе
                return UserGetMenuTree(userId);
            else return GetMenuTree();
        }

        public void UpdateUser(UserViewModel model)
        {
            var dbInfo = db.AspNetUsers.FirstOrDefault(u => u.Id == model.userId);
            if (dbInfo == null) return;

            dbInfo.DisplayName = model.displayName;
            dbInfo.Email = model.userEmail;
            dbInfo.UserName = model.userEmail;
            dbInfo.UserName = model.userEmail;
            dbInfo.TwoFactorEnabled = model.twoFactorEnabled;
            dbInfo.PostName = model.postName;
            dbInfo.ContactPhone = model.contactPhone;
            dbInfo.Dismissed = model.Dismissed;

            db.SaveChanges();

            var adminRoleId = GlobalConsts.GetAdminRoleId();

            if ((model.isAdmin) && (!(UserHasRole(model.userId, adminRoleId))))
            {
                AddRole(model.userId, GlobalConsts.GetAdminRoleId());
            }

            if ((!model.isAdmin) && (UserHasRole(model.userId, adminRoleId)))
            {
                RemoveRole(model.userId, GlobalConsts.GetAdminRoleId());
            }
        }

        public void RemoveUser(string userId)
        {
            var dbInfo = db.AspNetUsers.FirstOrDefault(u => u.Id == userId);
            if (dbInfo == null) return;

            db.AspNetUsers.Remove(dbInfo);
            db.SaveChanges();
        }

        public void AssignRoles(string userId, string[] roles)
        {
            var adminRoleId = GlobalConsts.GetAdminRoleId();
            var all_roles = db.AspNetRoles.Where(r => r.Id != adminRoleId).ToList();

            foreach (var role in all_roles)
            {
                RemoveRole(userId, role.Id);
            }

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    AddRole(userId, role);
                }
            }
        }

        public void CloneUserRoles(string userId)
        {
            var userRoles = getUserRoles(userId);

            List<string> roles = new List<string>();

            foreach (var role in userRoles)
            {
                roles.Add(role.roleId);
            }

            string[] allRoles = roles.ToArray();

            AssignRoles(userId, allRoles);
        }

        public List<UserRoleViewModel> getUserRoles(string userid)
        {
            var adminRoleId = GlobalConsts.GetAdminRoleId();

            return (from R in db.AspNetRoles
                    join RGR in db.RoleGroupsRole on R.Id equals RGR.RoleId
                    where (R.AspNetUsers.Any(m => m.Id == userid)
                        && R.Id != adminRoleId)
                    select new UserRoleViewModel()
                    {
                        roleId = R.Id,
                        roleName = R.Name,
                        assigned = true,
                        roleDescription = R.RoleDescription,
                        roleGroupName = RGR.RoleGroups.Name
                        //R.RoleGroups.FirstOrDefault(x=>x.AspNetRoles.Any(m=>m.Id == R.Id)).Name                        
                    })
                    .Union
                    (from R in db.AspNetRoles
                     join RGR in db.RoleGroupsRole on R.Id equals RGR.RoleId
                     where (!(R.AspNetUsers.Any(m => m.Id == userid)))
                        && (R.Id != adminRoleId)
                     select new UserRoleViewModel()
                     {
                         roleId = R.Id,
                         roleName = R.Name,
                         assigned = false,
                         roleDescription = R.RoleDescription,
                         roleGroupName = RGR.RoleGroups.Name
                     }).ToList();
        }

        public List<ImportTemplateInfo> GetImportTemplateInfo(/*bool isRests*/int FileType)
        {

            if (FileType == 0 || FileType == 1)
            {
                bool isRests = true;

                //0 - остатки, 1 - документы                
                if (FileType == 1) isRests = false;

                return (from R in db.SelectFieldsOfStuctureForImport(isRests)
                        select new ImportTemplateInfo()
                        {
                            ColumnNameInDB = R.ColumnName,
                            ColumnType = R.ColumnType,
                            ColumnDescription = R.ColumnDescription
                        }).ToList();
            }
            else if (FileType == 2)
            {
                List<ImportTemplateInfo> R = new List<ImportTemplateInfo>();

                ImportTemplateInfo elem = new ImportTemplateInfo();
                elem.ColumnNameInDB = "ProjectNum";
                elem.ColumnType = "varchar";
                elem.ColumnDescription = "Номер Заявки";

                R.Add(elem);

                ImportTemplateInfo elem1 = new ImportTemplateInfo();
                elem1.ColumnNameInDB = "ShortName";
                elem1.ColumnType = "nvarchar";
                elem1.ColumnDescription = "Тип Заявки";

                R.Add(elem1);
                ImportTemplateInfo elem2 = new ImportTemplateInfo();
                elem2.ColumnNameInDB = "PriotityType";
                elem2.ColumnType = "int";
                elem2.ColumnDescription = "Тип приоритета заявки";

                R.Add(elem2);
                ImportTemplateInfo elem3 = new ImportTemplateInfo();
                elem3.ColumnNameInDB = "OrderDate";
                elem3.ColumnType = "datetime2";
                elem3.ColumnDescription = "Дата_черновик";

                R.Add(elem3);

                ImportTemplateInfo elem3dop1 = new ImportTemplateInfo();
                elem3dop1.ColumnNameInDB = "СonfirmedDate";
                elem3dop1.ColumnType = "datetime2";
                elem3dop1.ColumnDescription = "Дата_утвердили";

                R.Add(elem3dop1);

                ImportTemplateInfo elem3dop2 = new ImportTemplateInfo();
                elem3dop2.ColumnNameInDB = "AcceptedDate";
                elem3dop2.ColumnType = "datetime2";
                elem3dop2.ColumnDescription = "Дата_приняли";

                R.Add(elem3dop2);

                ImportTemplateInfo elem4 = new ImportTemplateInfo();
                elem4.ColumnNameInDB = "TripType";
                elem4.ColumnType = "int";
                elem4.ColumnDescription = "Тип поездки";

                R.Add(elem4);
                ImportTemplateInfo elem5 = new ImportTemplateInfo();
                elem5.ColumnNameInDB = "FromCountry";
                elem5.ColumnType = "int";
                elem5.ColumnDescription = "Страна отправления";

                R.Add(elem5);
                ImportTemplateInfo elem6 = new ImportTemplateInfo();
                elem6.ColumnNameInDB = "FromCity";
                elem6.ColumnType = "varchar";
                elem6.ColumnDescription = "Город отправления";

                R.Add(elem6);
                ImportTemplateInfo elem7 = new ImportTemplateInfo();
                elem7.ColumnNameInDB = "AdressFrom";
                elem7.ColumnType = "varchar";
                elem7.ColumnDescription = "Адрес отправления";

                R.Add(elem7);
                ImportTemplateInfo elem8 = new ImportTemplateInfo();
                elem8.ColumnNameInDB = "OrgFrom";
                elem8.ColumnType = "varchar";
                elem8.ColumnDescription = "Организация отправитель";

                R.Add(elem8);
                ImportTemplateInfo elem9 = new ImportTemplateInfo();
                elem9.ColumnNameInDB = "StartDateTimeOfTrip";
                elem9.ColumnType = "datetime2";
                elem9.ColumnDescription = "Дата отправления";

                R.Add(elem9);
                ImportTemplateInfo elem10 = new ImportTemplateInfo();
                elem10.ColumnNameInDB = "StartDateTimeOfTrip2";
                elem10.ColumnType = "datetime2";
                elem10.ColumnDescription = "Время отправления";

                R.Add(elem10);
                ImportTemplateInfo elem11 = new ImportTemplateInfo();
                elem11.ColumnNameInDB = "ToCountry";
                elem11.ColumnType = "int";
                elem11.ColumnDescription = "Страна прибытия";

                R.Add(elem11);

                ImportTemplateInfo elem12 = new ImportTemplateInfo();
                elem12.ColumnNameInDB = "ToCity";
                elem12.ColumnType = "varchar";
                elem12.ColumnDescription = "Город прибытия";

                R.Add(elem12);
                ImportTemplateInfo elem13 = new ImportTemplateInfo();
                elem13.ColumnNameInDB = "AdressTo";
                elem13.ColumnType = "varchar";
                elem13.ColumnDescription = "Адрес прибытия";

                R.Add(elem13);

                ImportTemplateInfo elem14 = new ImportTemplateInfo();
                elem14.ColumnNameInDB = "OrgTo";
                elem14.ColumnType = "varchar";
                elem14.ColumnDescription = "Организация прибытия";

                R.Add(elem14);
                ImportTemplateInfo elem15 = new ImportTemplateInfo();
                elem15.ColumnNameInDB = "FinishDateTimeOfTrip";
                elem15.ColumnType = "datetime2";
                elem15.ColumnDescription = "Дата прибытия";

                R.Add(elem15);
                ImportTemplateInfo elem16 = new ImportTemplateInfo();
                elem16.ColumnNameInDB = "FinishDateTimeOfTrip2";
                elem16.ColumnType = "datetime2";
                elem16.ColumnDescription = "Время прибытия";

                R.Add(elem16);
                ImportTemplateInfo elem17 = new ImportTemplateInfo();
                elem17.ColumnNameInDB = "NeedReturn";
                elem17.ColumnType = "varchar";
                elem17.ColumnDescription = "Бронирование автомобиля для обратного пути";

                R.Add(elem17);
                ImportTemplateInfo elem18 = new ImportTemplateInfo();
                elem18.ColumnNameInDB = "ReturnStartDateTimeOfTrip";
                elem18.ColumnType = "datetime2";
                elem18.ColumnDescription = "Дата обратного отправления";

                R.Add(elem18);
                ImportTemplateInfo elem19 = new ImportTemplateInfo();
                elem19.ColumnNameInDB = "ReturnStartDateTimeOfTrip2";
                elem19.ColumnType = "datetime2";
                elem19.ColumnDescription = "Время обратного отправления";

                R.Add(elem19);
                ImportTemplateInfo elem20 = new ImportTemplateInfo();
                elem20.ColumnNameInDB = "ReturnFinishDateTimeOfTrip";
                elem20.ColumnType = "datetime2";
                elem20.ColumnDescription = "Дата окончания поездки";

                R.Add(elem20);
                ImportTemplateInfo elem21 = new ImportTemplateInfo();
                elem21.ColumnNameInDB = "ReturnFinishDateTimeOfTrip2";
                elem21.ColumnType = "datetime2";
                elem21.ColumnDescription = "Время окончания поездки";

                R.Add(elem21);
                ImportTemplateInfo elem22 = new ImportTemplateInfo();
                elem22.ColumnNameInDB = "PassInfo";
                elem22.ColumnType = "varchar";
                elem22.ColumnDescription = "Список пассажиров";

                R.Add(elem22);
                ImportTemplateInfo elem23 = new ImportTemplateInfo();
                elem23.ColumnNameInDB = "TripDescription";
                elem23.ColumnType = "varchar";
                elem23.ColumnDescription = "Цель поездки";

                R.Add(elem23);
                ImportTemplateInfo elem24 = new ImportTemplateInfo();
                elem24.ColumnNameInDB = "ClientCFO";
                elem24.ColumnType = "int";
                elem24.ColumnDescription = "ЦФО";

                R.Add(elem24);
                ImportTemplateInfo elem25 = new ImportTemplateInfo();
                elem25.ColumnNameInDB = "ClientName";
                elem25.ColumnType = "varchar";
                elem25.ColumnDescription = "Подразделение";

                R.Add(elem25);

                ImportTemplateInfo elem25dop1 = new ImportTemplateInfo();
                elem25dop1.ColumnNameInDB = "ProjectNumber";
                elem25dop1.ColumnType = "varchar";
                elem25dop1.ColumnDescription = "Номер проекта";

                R.Add(elem25dop1);


                ImportTemplateInfo elem26 = new ImportTemplateInfo();
                elem26.ColumnNameInDB = "PayerName";
                elem26.ColumnType = "varchar";
                elem26.ColumnDescription = "Плательщик";

                R.Add(elem26);
                ImportTemplateInfo elem27 = new ImportTemplateInfo();
                elem27.ColumnNameInDB = "CreatedByUser";
                elem27.ColumnType = "nvarchar";
                elem27.ColumnDescription = "Инициатор";

                R.Add(elem27);
                ImportTemplateInfo elem28 = new ImportTemplateInfo();
                elem28.ColumnNameInDB = "OrderExecuter";
                elem28.ColumnType = "nvarchar";
                elem28.ColumnDescription = "Исполнитель";

                R.Add(elem28);

                ImportTemplateInfo elem29 = new ImportTemplateInfo();
                elem29.ColumnNameInDB = "CreatorPosition";
                elem29.ColumnType = "nvarchar";
                elem29.ColumnDescription = "Контактное лицо (Ф.И.О /должность)";

                R.Add(elem29);

                ImportTemplateInfo elem30 = new ImportTemplateInfo();
                elem30.ColumnNameInDB = "CreatorContact";
                elem30.ColumnType = "nvarchar";
                elem30.ColumnDescription = "Контактный телефон по заявке";

                R.Add(elem30);

                ImportTemplateInfo elem30dop1 = new ImportTemplateInfo();
                elem30dop1.ColumnNameInDB = "Attachment";
                elem30dop1.ColumnType = "nvarchar";
                elem30dop1.ColumnDescription = "Вложение";

                R.Add(elem30dop1);

                ImportTemplateInfo elem31 = new ImportTemplateInfo();
                elem31.ColumnNameInDB = "ExpeditorName";
                elem31.ColumnType = "nvarchar";
                elem31.ColumnDescription = "Экспедитор";

                R.Add(elem31);

                ImportTemplateInfo elem32 = new ImportTemplateInfo();
                elem32.ColumnNameInDB = "ContractInfo";
                elem32.ColumnType = "nvarchar";
                elem32.ColumnDescription = "Договор";

                R.Add(elem32);
                ImportTemplateInfo elem33 = new ImportTemplateInfo();
                elem33.ColumnNameInDB = "CarrierInfo";
                elem33.ColumnType = "nvarchar";
                elem33.ColumnDescription = "Перевозчик";

                R.Add(elem33);
                ImportTemplateInfo elem34 = new ImportTemplateInfo();
                elem34.ColumnNameInDB = "CarModelInfo";
                elem34.ColumnType = "nvarchar";
                elem34.ColumnDescription = "марка авто";

                R.Add(elem34);

                ImportTemplateInfo elem35 = new ImportTemplateInfo();
                elem35.ColumnNameInDB = "CarRegNum";
                elem35.ColumnType = "nvarchar";
                elem35.ColumnDescription = "номер авто";

                R.Add(elem35);

                ImportTemplateInfo elem36 = new ImportTemplateInfo();
                elem36.ColumnNameInDB = "CarDriverInfo";
                elem36.ColumnType = "nvarchar";
                elem36.ColumnDescription = "ФИО водителя";

                R.Add(elem36);
                ImportTemplateInfo elem37 = new ImportTemplateInfo();
                elem37.ColumnNameInDB = "DriverCardInfo";
                elem37.ColumnType = "nvarchar";
                elem37.ColumnDescription = "Права";

                R.Add(elem37);
                ImportTemplateInfo elem38 = new ImportTemplateInfo();
                elem38.ColumnNameInDB = "DriverContactInfo";
                elem38.ColumnType = "nvarchar";
                elem38.ColumnDescription = "контакт водителя";

                R.Add(elem38);
                ImportTemplateInfo elem39 = new ImportTemplateInfo();
                elem39.ColumnNameInDB = "Comments";
                elem39.ColumnType = "nvarchar";
                elem39.ColumnDescription = "Комментарий";

                R.Add(elem39);

                return R;
            }
            else //if (FileType == 3)
            {
                List<ImportTemplateInfo> R = new List<ImportTemplateInfo>();

                ImportTemplateInfo elem = new ImportTemplateInfo();
                elem.ColumnNameInDB = "ProjectNum";
                elem.ColumnType = "varchar";
                elem.ColumnDescription = "Номер Заявки";

                R.Add(elem);

                ImportTemplateInfo elem1 = new ImportTemplateInfo();
                elem1.ColumnNameInDB = "OrderDate";
                elem1.ColumnType = "datetime2";
                elem1.ColumnDescription = "Дата указанная в заявке";

                R.Add(elem1);

                ImportTemplateInfo elem2 = new ImportTemplateInfo();
                elem2.ColumnNameInDB = "PayerName";
                elem2.ColumnType = "varchar";
                elem2.ColumnDescription = "Плательщик";

                R.Add(elem2);

                ImportTemplateInfo elem2dop1 = new ImportTemplateInfo();
                elem2dop1.ColumnNameInDB = "ClientCFO";
                elem2dop1.ColumnType = "varchar";
                elem2dop1.ColumnDescription = "ЦФО";

                R.Add(elem2dop1);

                ImportTemplateInfo elem2dop2 = new ImportTemplateInfo();
                elem2dop2.ColumnNameInDB = "ClientName";
                elem2dop2.ColumnType = "varchar";
                elem2dop2.ColumnDescription = "Заказчик";

                R.Add(elem2dop2);


                ImportTemplateInfo elem3 = new ImportTemplateInfo();
                elem3.ColumnNameInDB = "ApplicationOwner";
                elem3.ColumnType = "varchar";
                elem3.ColumnDescription = "Автор заявки";

                R.Add(elem3);


                ImportTemplateInfo elem3dop1 = new ImportTemplateInfo();
                elem3dop1.ColumnNameInDB = "OwnerPost";
                elem3dop1.ColumnType = "varchar";
                elem3dop1.ColumnDescription = "Должность автора";

                R.Add(elem3dop1);

                ImportTemplateInfo elem4 = new ImportTemplateInfo();
                elem4.ColumnNameInDB = "CreatorContact";
                elem4.ColumnType = "varchar";
                elem4.ColumnDescription = "Телефон автора";

                R.Add(elem4);


                ImportTemplateInfo elem5 = new ImportTemplateInfo();
                elem5.ColumnNameInDB = "Shipper";
                elem5.ColumnType = "varchar";
                elem5.ColumnDescription = "Грузоотправитель";

                R.Add(elem5);

                ImportTemplateInfo elem6 = new ImportTemplateInfo();
                elem6.ColumnNameInDB = "Consignee";
                elem6.ColumnType = "varchar";
                elem6.ColumnDescription = "Грузополучатель";

                R.Add(elem6);

                ImportTemplateInfo elem7 = new ImportTemplateInfo();
                elem7.ColumnNameInDB = "FromCountry";
                elem7.ColumnType = "varchar";
                elem7.ColumnDescription = "Страна отправления";

                R.Add(elem7);

                ImportTemplateInfo elem7dop1 = new ImportTemplateInfo();
                elem7dop1.ColumnNameInDB = "FromCity";
                elem7dop1.ColumnType = "varchar";
                elem7dop1.ColumnDescription = "Город отправления";

                R.Add(elem7dop1);

                ImportTemplateInfo elem8 = new ImportTemplateInfo();
                elem8.ColumnNameInDB = "AdressFrom";
                elem8.ColumnType = "varchar";
                elem8.ColumnDescription = "Пункт отправления";

                R.Add(elem8);

                ImportTemplateInfo elem8dop1 = new ImportTemplateInfo();
                elem8dop1.ColumnNameInDB = "ToCountry";
                elem8dop1.ColumnType = "varchar";
                elem8dop1.ColumnDescription = "Страна прибытия";

                R.Add(elem8dop1);

                ImportTemplateInfo elem8dop2 = new ImportTemplateInfo();
                elem8dop2.ColumnNameInDB = "ToCity";
                elem8dop2.ColumnType = "varchar";
                elem8dop2.ColumnDescription = "Город прибытия";

                R.Add(elem8dop2);

                ImportTemplateInfo elem9 = new ImportTemplateInfo();
                elem9.ColumnNameInDB = "AdressTo";
                elem9.ColumnType = "varchar";
                elem9.ColumnDescription = "Пункт прибытия";

                R.Add(elem9);

                ImportTemplateInfo elem10 = new ImportTemplateInfo();
                elem10.ColumnNameInDB = "TruckDescription";
                elem10.ColumnType = "varchar";
                elem10.ColumnDescription = "Наименование груза";

                R.Add(elem10);

                ImportTemplateInfo elem11 = new ImportTemplateInfo();
                elem11.ColumnNameInDB = "Weight";
                elem11.ColumnType = "decimal";
                elem11.ColumnDescription = "Вес груза";

                R.Add(elem11);

                ImportTemplateInfo elem11dop1 = new ImportTemplateInfo();
                elem11dop1.ColumnNameInDB = "Volume";
                elem11dop1.ColumnType = "decimal";
                elem11dop1.ColumnDescription = "Объем, м3";

                R.Add(elem11dop1);

                ImportTemplateInfo elem11dop2 = new ImportTemplateInfo();
                elem11dop2.ColumnNameInDB = "BoxingDescription";
                elem11dop2.ColumnType = "varchar";
                elem11dop2.ColumnDescription = "Упаковка";

                R.Add(elem11dop2);

                ImportTemplateInfo elem11dop3 = new ImportTemplateInfo();
                elem11dop3.ColumnNameInDB = "DimenssionL";
                elem11dop3.ColumnType = "decimal";
                elem11dop3.ColumnDescription = "Габарит L";

                R.Add(elem11dop3);

                ImportTemplateInfo elem11dop4 = new ImportTemplateInfo();
                elem11dop4.ColumnNameInDB = "DimenssionW";
                elem11dop4.ColumnType = "decimal";
                elem11dop4.ColumnDescription = "Габарит W";

                R.Add(elem11dop4);

                ImportTemplateInfo elem11dop5 = new ImportTemplateInfo();
                elem11dop5.ColumnNameInDB = "DimenssionH";
                elem11dop5.ColumnType = "decimal";
                elem11dop5.ColumnDescription = "Габарит H";

                R.Add(elem11dop5);

                ImportTemplateInfo elem12 = new ImportTemplateInfo();
                elem12.ColumnNameInDB = "VehicleTypeName";
                elem12.ColumnType = "varchar";
                elem12.ColumnDescription = "Тип авто/кузова";

                R.Add(elem12);


                ImportTemplateInfo elem13 = new ImportTemplateInfo();
                elem13.ColumnNameInDB = "LoadingTypeName";
                elem13.ColumnType = "varchar";
                elem13.ColumnDescription = "Вид загрузки";

                R.Add(elem13);

                ImportTemplateInfo elem14 = new ImportTemplateInfo();
                elem14.ColumnNameInDB = "UnloadingTypeName";
                elem14.ColumnType = "varchar";
                elem14.ColumnDescription = "Ограничения по выгрузке";

                R.Add(elem14);

                ImportTemplateInfo elem16 = new ImportTemplateInfo();
                elem16.ColumnNameInDB = "CarNumber";
                elem16.ColumnType = "int";
                elem16.ColumnDescription = "К-во авто к подаче";

                R.Add(elem16);

                ImportTemplateInfo elem17 = new ImportTemplateInfo();
                elem17.ColumnNameInDB = "FromShipperDate";
                elem17.ColumnType = "datetime2";
                elem17.ColumnDescription = "Дата подачи авто по заявке";

                R.Add(elem17);

                ImportTemplateInfo elem18 = new ImportTemplateInfo();
                elem18.ColumnNameInDB = "FromShipperTime";
                elem18.ColumnType = "datetime2";
                elem18.ColumnDescription = "Время подачи авто по заявке";

                R.Add(elem18);

                ImportTemplateInfo elem19 = new ImportTemplateInfo();
                elem19.ColumnNameInDB = "ToConsigneeDate";
                elem19.ColumnType = "datetime2";
                elem19.ColumnDescription = "Дата доставки груза по заявке";

                R.Add(elem19);

                ImportTemplateInfo elem20 = new ImportTemplateInfo();
                elem20.ColumnNameInDB = "ToConsigneeTime";
                elem20.ColumnType = "datetime2";
                elem20.ColumnDescription = "Время доставки груза по заявке";

                R.Add(elem20);

                ImportTemplateInfo elem20dop1 = new ImportTemplateInfo();
                elem20dop1.ColumnNameInDB = "CreateDate";
                elem20dop1.ColumnType = "datetime2";
                elem20dop1.ColumnDescription = "Дата подачи заявки";

                R.Add(elem20dop1);

                ImportTemplateInfo elem20dop2 = new ImportTemplateInfo();
                elem20dop2.ColumnNameInDB = "CreateTime";
                elem20dop2.ColumnType = "datetime2";
                elem20dop2.ColumnDescription = "Время подачи заявки";

                R.Add(elem20dop2);

                ImportTemplateInfo elem21 = new ImportTemplateInfo();
                elem21.ColumnNameInDB = "TruckTypeName";
                elem21.ColumnType = "varchar";
                elem21.ColumnDescription = "Тип груза";

                R.Add(elem21);

                ImportTemplateInfo elem22 = new ImportTemplateInfo();
                elem22.ColumnNameInDB = "PointSumma";
                elem22.ColumnType = "int";
                elem22.ColumnDescription = "Сумма точек загрузки и выгрузки";

                R.Add(elem22);

                ImportTemplateInfo elem23 = new ImportTemplateInfo();
                elem23.ColumnNameInDB = "RouteLength";
                elem23.ColumnType = "int";
                elem23.ColumnDescription = "Длина марш., км";

                R.Add(elem23);

                ImportTemplateInfo elem24 = new ImportTemplateInfo();
                elem24.ColumnNameInDB = "RouteSelection";
                elem24.ColumnType = "varchar";
                elem24.ColumnDescription = "Тип маршрута";

                R.Add(elem24);

                ImportTemplateInfo elem25 = new ImportTemplateInfo();
                elem25.ColumnNameInDB = "PriotityType";
                elem25.ColumnType = "varchar";
                elem25.ColumnDescription = "Плановая заявка";

                R.Add(elem25);

                ImportTemplateInfo elem26 = new ImportTemplateInfo();
                elem26.ColumnNameInDB = "ShortName";
                elem26.ColumnType = "varchar";
                elem26.ColumnDescription = "Тип Заявки";

                R.Add(elem26);

                ImportTemplateInfo elem27 = new ImportTemplateInfo();
                elem27.ColumnNameInDB = "TripType";
                elem27.ColumnType = "nvarchar";
                elem27.ColumnDescription = "Тип поездки";

                R.Add(elem27);

                ImportTemplateInfo elem28 = new ImportTemplateInfo();
                elem28.ColumnNameInDB = "ShipperContactPerson";
                elem28.ColumnType = "varchar";
                elem28.ColumnDescription = "Контактное лицо (грузоотправитель)";

                R.Add(elem28);

                ImportTemplateInfo elem29 = new ImportTemplateInfo();
                elem29.ColumnNameInDB = "ShipperContactPersonPhone";
                elem29.ColumnType = "varchar";
                elem29.ColumnDescription = "Телефон контактного лица (грузоотправитель)";

                R.Add(elem29);

                ImportTemplateInfo elem30 = new ImportTemplateInfo();
                elem30.ColumnNameInDB = "ConsigneeContactPerson";
                elem30.ColumnType = "varchar";
                elem30.ColumnDescription = "Контактное лицо (грузополучатель)";

                R.Add(elem30);

                ImportTemplateInfo elem31 = new ImportTemplateInfo();
                elem31.ColumnNameInDB = "ConsigneeContactPersonPhone";
                elem31.ColumnType = "varchar";
                elem31.ColumnDescription = "Телефон контактного лица (грузополучатель)";

                R.Add(elem31);

                return R;
            }

        }

        //получение типа столбца из бд
        public string GetColumnType(bool isRests, string ColumnName)
        {
            string ColumnTypeVal = "";
            var ColumnType = db.SelectColumnType(isRests, ColumnName).FirstOrDefault();
            ColumnTypeVal = ColumnType.ColumnType;
            return ColumnTypeVal;
        }

        public List<ColumnSettingsModel> GetConfigColumn(bool isRests)
        {
            return (from r in db.SelectColumnConfig(isRests)
                    select new ColumnSettingsModel()
                    {
                        ColumnName = r.ColumnName,
                        isNotNull = (r.isNotNull == true) ? true : false,
                        isNotNullForRest = (r.isNotNullForRest == true) ? true : false,
                        isZeroDateReplace = (r.isZeroDateReplace == true) ? true : false,
                        isNumeric = (r.isNumeric == true) ? true : false,
                        isZeroNumericReplace = (r.isZeroNumericReplace == true) ? true : false,
                        isNullForRest = (r.isNullForRest == true) ? true : false,

                        isUnique = (r.isUnique == true) ? true : false,
                        isLessActualDateBeg = (r.isLessActualDateBeg == true) ? true : false,
                        isBiggerActualDateEnd = (r.isBiggerActualDateEnd == true) ? true : false,
                        isSimilar = (r.isSimilar == true) ? true : false,
                        isNotZeroForQPrihodNotZeroForDocs = (r.isNotZeroForQPrihodNotZeroForDocs == true) ? true : false,
                        isNullForQPrihodZeroForDocs = (r.isNullForQPrihodZeroForDocs == true) ? true : false,
                        isNullForQPrihodNotZeroForDocs = (r.isNullForQPrihodNotZeroForDocs == true) ? true : false,
                        id = r.id
                    }).ToList();

        }


        //получение настроек импорта
        public List<ColumnSettingsModel> GetImportConfig()
        {
            return (from r in db.ImportConfig
                    select new ColumnSettingsModel()
                    {
                        ColumnName = r.Column_Name,
                        isNotNull = (r.isNotNull == true) ? true : false,
                        isNotNullForRest = (r.isNotNullForRest == true) ? true : false,
                        isZeroDateReplace = (r.isZeroDateReplace == true) ? true : false,
                        isNumeric = (r.isNumeric == true) ? true : false,
                        isZeroNumericReplace = (r.isZeroNumericReplace == true) ? true : false,
                        isNullForRest = (r.isNullForRest == true) ? true : false,

                        isUnique = (r.isUnique == true) ? true : false,
                        isLessActualDateBeg = (r.isLessActualDateBeg == true) ? true : false,
                        isBiggerActualDateEnd = (r.isBiggerActualDateEnd == true) ? true : false,
                        isSimilar = (r.isSimilar == true) ? true : false,
                        isNotZeroForQPrihodNotZeroForDocs = (r.isNotZeroForQPrihodNotZeroForDocs == true) ? true : false,
                        isNullForQPrihodZeroForDocs = (r.isNullForQPrihodZeroForDocs == true) ? true : false,
                        isNullForQPrihodNotZeroForDocs = (r.isNullForQPrihodNotZeroForDocs == true) ? true : false,
                        id = r.id
                    }).ToList();

        }

        //получение имен столбцов для импорта
        public List<ColumnNameModel> GetImportColumnName()
        {
            return (from r in db.ImportConfig
                    select new ColumnNameModel()
                    {
                        ColumnName = r.Column_Name
                    }).ToList();

        }
        //получение значения столбца по умолчанию 
        public string GetColumnValByDefault(bool IsRestFile, string ColumnName)
        {
            string ColumnValByDef = "";
            var ColumnValByDefault = db.SelectColumnValByDef(IsRestFile, ColumnName).FirstOrDefault();
            if (ColumnValByDefault != null) ColumnValByDef = ColumnValByDefault.ColumnValByDefault;
            return ColumnValByDef;
        }

        public void WriteLogImportError(bool IsRestFile, int NumRow, string ColumnName, string CommentError, string guidSession, int isCommentType, int idSnapshot)
        {
            db.LogImportErrorInsert(IsRestFile, NumRow, ColumnName, CommentError, guidSession, isCommentType, idSnapshot);
            db.SaveChanges();
        }

        //получение индекса столбца ColumnName в заголовках HeadersCSVFile
        public int GetIndexColumnInCSVFile(string[] HeadersCSVFile, string ColumnName)
        {
            int HeadersCSVFileLength, j;
            HeadersCSVFileLength = HeadersCSVFile.Length;

            var IndexColumnInFileVal = -1;
            for (j = 0; j < HeadersCSVFileLength; j++) //цикл по заголовкам файла
            {
                if (HeadersCSVFile[j] == ColumnName)
                {
                    IndexColumnInFileVal = j; //сохраняем индекс столбца в файле для i-го столбца таблицы в бд
                    break;
                }
            }
            return IndexColumnInFileVal;
        }

        // заполнение настроечного класса для импорта
        public List<ImportTemplateInfo> GetImportTemplateInfoWithCSVColumn(ConfiguredByUserColumsPairsModel preImportConfig,
            string[] HeadersCSVFile, string[] DataCSVFile)
        {
            // заполнение настроечного класса для импорта
            var templateInfo = new List<ImportTemplateInfo>();
            int i;
            int configuredPairsCount = preImportConfig.configuredPairs.Count;

            for (i = 0; i < configuredPairsCount; i++)// цикл по парам ("столбец в бд"; "столбец в файле")
            {
                bool isRests = true;

                //0 - остатки, 1 - документы
                if (preImportConfig.FileType == 1) isRests = false;

                var ColumnNameInDBVal = preImportConfig.configuredPairs.ElementAt(i).Key;//название столбца в бд
                var ColumnNameInFileVal = preImportConfig.configuredPairs.ElementAt(i).Value;//название столбца в файле
                var ColumnTypeVal = "";
                if (preImportConfig.FileType == 0 || preImportConfig.FileType == 1)
                    ColumnTypeVal = GetColumnType(/*preImportConfig.*/isRests, ColumnNameInDBVal);//тип столбца в бд
                var IndexColumnInFileVal = GetIndexColumnInCSVFile(HeadersCSVFile, ColumnNameInFileVal);//индекс выбранного ColumnNameInFileVal в заголовке HeadersCSVFile

                templateInfo.Add(new ImportTemplateInfo()
                {
                    ColumnNameInDB = ColumnNameInDBVal,
                    ColumnNameInFile = ColumnNameInFileVal,
                    IndexColumnInFile = IndexColumnInFileVal,
                    ColumnType = ColumnTypeVal
                });
            }
            return templateInfo;
        }

        public bool GetIdSnapshotForImport(string ServerFileName, bool IsRestFile, string[] HeadersCSVFile,
            string[] FirstDataRowCSVFile, ref int id_snapshot)
        {
            bool Res = true;
            int i;
            string ActualDateBeg = "";
            string ActualDateEnd = "";

            int HeadersCSVFileLength = HeadersCSVFile.Length;
            for (i = 0; i < HeadersCSVFileLength; i++)
            {
                if (HeadersCSVFile[i].Trim() == "ActualDateBeg")
                    ActualDateBeg = FirstDataRowCSVFile[i];
                if (HeadersCSVFile[i].Trim() == "ActualDateEnd")
                    ActualDateEnd = FirstDataRowCSVFile[i];
            }

            if (ActualDateBeg == "" || ActualDateEnd == "")
                Res = false;
            else
            {
                SqlParameter[] SQL_param = new SqlParameter[4];

                SQL_param[0] = new SqlParameter();
                SQL_param[0].ParameterName = "@ActualDateBeg";
                DateTime dt = DateTime.ParseExact(ActualDateBeg, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                SQL_param[0].Value = dt;

                SQL_param[1] = new SqlParameter();
                SQL_param[1].ParameterName = "@ActualDateEnd";
                DateTime dt_end = DateTime.ParseExact(ActualDateEnd, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                SQL_param[1].Value = dt_end;

                SQL_param[2] = new SqlParameter();
                SQL_param[2].ParameterName = "@IsRest";
                if (IsRestFile == true)
                    SQL_param[2].Value = 1;
                else
                    SQL_param[2].Value = 0;

                SQL_param[3] = new SqlParameter();
                SQL_param[3].ParameterName = "@id_snapshot";
                SQL_param[3].SqlDbType = SqlDbType.Int;
                SQL_param[3].Direction = ParameterDirection.Output;

                db.Database.ExecuteSqlCommand("exec dbo.GetIdSnapshot @ActualDateBeg, @ActualDateEnd, @IsRest, @id_snapshot output", SQL_param);
                id_snapshot = (int)SQL_param[3].Value;
                Res = true;

            }
            return Res;
        }


        public bool DoImport(ConfiguredByUserColumsPairsModel preImportConfig, string[] HeadersCSVFile,
            string[] DataCSVFile, string[] FirstDataRowCSVFile, ref string guidSessionString, ref int Id_Snapshot)
        {
            int i, j;
            int DataCSVFileLength = DataCSVFile.Length;//количество строк в файле, которые будем импортировать            
            bool FlagSuccessImport = true; //флаг полного успешного импорта 
            bool SuccessFindIdSnapshot = false;//флаг удачного забора/генерации нового Id_Snapshot для импорта

            Guid guidSession;
            guidSession = Guid.NewGuid(); //генерация гуида (используется при записи лога ошибок)
            guidSessionString = guidSession.ToString();

            // забор/генерация нового Id_Snapshot для импорта
            Id_Snapshot = 0;
            bool isRests = true;
            if (preImportConfig.FileType == 0 || preImportConfig.FileType == 1)
            {
                //0 - остатки, 1 - документы
                if (preImportConfig.FileType == 1) isRests = false;
            }
            SuccessFindIdSnapshot = GetIdSnapshotForImport(preImportConfig.ServerFileName, isRests,
                HeadersCSVFile, FirstDataRowCSVFile, ref Id_Snapshot);

            if (SuccessFindIdSnapshot == true)// если Id_Snapshot забрался, то осуществляем импорт
            {
                //заполнение настроечного класса
                var templateInfo = GetImportTemplateInfoWithCSVColumn(preImportConfig, HeadersCSVFile, DataCSVFile);

                for (i = 1; i < DataCSVFileLength; i++)
                {
                    //сохраняем запись во временную переменную
                    bool FlagSuccessImportRow = true; //флаг успешного импорта i-той строки
                    string[] RowCSVFile = null;
                    RowCSVFile = DataCSVFile[i].Split('\t');
                    string SQLComm;

                    if (isRests == true)
                        SQLComm = "exec dbo.RestsSnapshotInsert ";
                    else
                        SQLComm = "exec dbo.DocsSnapshotInsert ";

                    SqlParameter[] SQL_param = new SqlParameter[templateInfo.Count + 1];

                    //забираем настройки импорта
                    var importConfig = GetConfigColumn(/*preImportConfig.IsRestFile*/isRests).ToList();

                    for (j = 0; j < templateInfo.Count; j++)
                    {
                        //отправка на проверку конвертации типа столбца для ХП
                        string ColumnNameInDB = templateInfo[j].ColumnNameInDB;
                        string ColumnNameInFile = templateInfo[j].ColumnNameInFile;
                        string ColumnType = templateInfo[j].ColumnType;
                        int IndexColumnInFile = templateInfo[j].IndexColumnInFile;

                        string RowCSVFileValInCell;
                        if (ColumnNameInFile == "по умолчанию")
                            RowCSVFileValInCell = GetColumnValByDefault(/*preImportConfig.IsRestFile*/isRests, ColumnNameInDB); //отбор значений по умолчанию
                        else
                            RowCSVFileValInCell = RowCSVFile[IndexColumnInFile]; //сохраняем значение 

                        object ObjectCellVal = RowCSVFileValInCell; //переприсваивание для проверки конвертации
                        string CommentError = "";
                        string CommentChange = "";

                        //забираем настройки конкретного поля
                        var columnSettings = importConfig.FirstOrDefault(ic => ic.ColumnName == ColumnNameInDB);

                        if (columnSettings is null) columnSettings = new ColumnSettingsModel();

                        //проверка - не пустое поле
                        bool isNullCheck = false;
                        if (columnSettings.isNotNull == true)
                            isNullCheck = true;

                        //проверка - не пустое для остатков
                        if ((!isNullCheck) && (isRests) && (columnSettings.isNotNullForRest == true))
                            isNullCheck = true;

                        //проверка - пустое для остатков
                        bool isNullForRestCheck = false;
                        if ((isRests) && (columnSettings.isNullForRest == true))
                            isNullForRestCheck = true;

                        //замена не чиловых на 0
                        bool isZeroNumericReplace = false;
                        if (columnSettings.isZeroNumericReplace == true)
                            isZeroNumericReplace = true;

                        //замена не дат на пусто
                        bool isZeroDateReplace = false;
                        if (columnSettings.isZeroDateReplace == true)
                            isZeroDateReplace = true;

                        bool SuccessConvert = ConvertTypeHelpers.ConvertColumnVal(isRests, i, ColumnNameInDB, ColumnType, ref ObjectCellVal, ref CommentError, ref CommentChange, isNullCheck, isZeroNumericReplace, isZeroDateReplace, isNullForRestCheck);

                        if (SuccessConvert == false)
                        {
                            //запись в лог
                            if (CommentError != "")
                                WriteLogImportError(isRests, i, ColumnNameInDB, CommentError, guidSession.ToString(), 1, Id_Snapshot);

                            if (CommentChange != "")
                            {
                                WriteLogImportError(isRests, i, ColumnNameInDB, CommentChange,
                                    guidSession.ToString(), 2, Id_Snapshot);
                                FlagSuccessImport = false;
                            }

                            //отметка о том, что строка неудачно проимпортируется в связи с ошибкой конвертации типов
                            if ((FlagSuccessImportRow == true))
                                FlagSuccessImportRow = false;
                        }

                        if (SuccessConvert == true)
                        {
                            if (CommentChange != "")
                            {
                                WriteLogImportError(isRests, i, ColumnNameInDB, CommentChange,
                                    guidSession.ToString(), 2, Id_Snapshot);
                                FlagSuccessImport = false;
                            }

                            SQLComm = SQLComm + "@" + ColumnNameInDB + ", ";
                            SQL_param[j] = new SqlParameter
                            {
                                ParameterName = "@" + ColumnNameInDB,
                                Value = ObjectCellVal
                            };
                        }
                    }
                    if (FlagSuccessImportRow == true)
                    {
                        SQL_param[templateInfo.Count] = new SqlParameter
                        {
                            ParameterName = "@id_snapshot",
                            Value = Id_Snapshot
                        };

                        //формирование запроса вызова хранимой процедуры
                        SQLComm = SQLComm = SQLComm + "@id_snapshot";

                        //процедура вставки данных
                        db.Database.ExecuteSqlCommand(SQLComm, SQL_param);

                    }
                    else
                        if (FlagSuccessImport == true)
                        FlagSuccessImport = false; //отметка о том, что полностью импорт не прошел успешно
                }
            }
            else
            {
                FlagSuccessImport = false;
                db.LogImportErrorInsert(/*preImportConfig.IsRestFile*/isRests, 0, "все",
                    "не найдены поля ActualBeg ActualEnd", guidSessionString, 1, Id_Snapshot);
                db.SaveChanges();
            }

            if (FlagSuccessImport == true) return true; //переход на форму полного успешного импорта
            else return false;//переход на форму с логом ошибок
        }


        public bool DoImportOrders(ConfiguredByUserColumsPairsModel preImportConfig, string[] HeadersCSVFile,
           string[] DataCSVFile, string[] FirstDataRowCSVFile, ref string guidSessionString, ref int Id_Snapshot)
        {
            int i, j;
            int DataCSVFileLength = DataCSVFile.Length;//количество строк в файле, которые будем импортировать

            Guid guidSession;
            guidSession = Guid.NewGuid(); //генерация гуида (используется при записи лога ошибок)
            guidSessionString = guidSession.ToString();

            //заполнение настроечного класса
            var templateInfo = GetImportTemplateInfoWithCSVColumn(preImportConfig, HeadersCSVFile, DataCSVFile);

            for (i = 1; i < DataCSVFileLength; i++)
            {
                //сохраняем запись во временную переменную
                bool FlagSuccessImportRow = true; //флаг успешного импорта i-той строки
                string[] RowCSVFile = null;
                RowCSVFile = DataCSVFile[i].Split('\t');
                string SQLComm;

                SQLComm = "exec dbo.OrdersInsert ";

                SqlParameter[] SQL_param = new SqlParameter[templateInfo.Count + 1];

                for (j = 0; j < templateInfo.Count; j++)
                {
                    //отправка на проверку конвертации типа столбца для ХП
                    string ColumnNameInDB = templateInfo[j].ColumnNameInDB;
                    string ColumnNameInFile = templateInfo[j].ColumnNameInFile;
                    string ColumnType = templateInfo[j].ColumnType;
                    int IndexColumnInFile = templateInfo[j].IndexColumnInFile;

                    string RowCSVFileValInCell;

                    RowCSVFileValInCell = RowCSVFile[IndexColumnInFile]; //сохраняем значение 

                    object ObjectCellVal = RowCSVFileValInCell; //переприсваивание для проверки конвертации

                    SQLComm = SQLComm + "@" + ColumnNameInDB + ", ";
                    SQL_param[j] = new SqlParameter();
                    SQL_param[j].ParameterName = "@" + ColumnNameInDB;

                    if (ColumnNameInDB == "OrderDate" || ColumnNameInDB == "СonfirmedDate" || ColumnNameInDB == "AcceptedDate" || ColumnNameInDB == "StartDateTimeOfTrip" ||
                        ColumnNameInDB == "FinishDateTimeOfTrip" || ColumnNameInDB == "ReturnStartDateTimeOfTrip" || ColumnNameInDB == "ReturnFinishDateTimeOfTrip")
                    {

                        DateTime dt_end = DateTime.ParseExact(ObjectCellVal.ToString(), "dd.MM.yy", CultureInfo.InvariantCulture);
                        SQL_param[j].Value = dt_end;
                    }
                    else
                    {
                        SQL_param[j].Value = ObjectCellVal.ToString().Replace("\"\"", "\"");
                    }
                }

                SQLComm = SQLComm + "@CreateDatetime"; //+ ", ";
                SQL_param[j] = new SqlParameter();
                SQL_param[j].ParameterName = "@CreateDatetime";
                SQL_param[j].Value = DateTime.Now;
                db.Database.ExecuteSqlCommand(SQLComm, SQL_param);

            }

            return true; //переход на форму полного успешного импорта

        }


        public bool DoImportTruckOrders(ConfiguredByUserColumsPairsModel preImportConfig, string[] HeadersCSVFile,
          string[] DataCSVFile, string[] FirstDataRowCSVFile, ref string guidSessionString, ref int Id_Snapshot)
        {
            int i, j;
            int DataCSVFileLength = DataCSVFile.Length;//количество строк в файле, которые будем импортировать            
            bool FlagSuccessImport = true; //флаг полного успешного импорта 
            bool SuccessFindIdSnapshot = false;//флаг удачного забора/генерации нового Id_Snapshot для импорта

            Guid guidSession;
            guidSession = Guid.NewGuid(); //генерация гуида (используется при записи лога ошибок)
            guidSessionString = guidSession.ToString();

            //заполнение настроечного класса
            var templateInfo = GetImportTemplateInfoWithCSVColumn(preImportConfig, HeadersCSVFile, DataCSVFile);

            for (i = 1; i < DataCSVFileLength; i++)
            {
                //сохраняем запись во временную переменную
                bool FlagSuccessImportRow = true; //флаг успешного импорта i-той строки
                string[] RowCSVFile = null;
                RowCSVFile = DataCSVFile[i].Split('\t');
                string SQLComm;

                SQLComm = "exec dbo.TruckOrdersInsert ";

                SqlParameter[] SQL_param = new SqlParameter[templateInfo.Count + 1];

                for (j = 0; j < templateInfo.Count; j++)
                {
                    //отправка на проверку конвертации типа столбца для ХП
                    string ColumnNameInDB = templateInfo[j].ColumnNameInDB;
                    string ColumnNameInFile = templateInfo[j].ColumnNameInFile;
                    string ColumnType = templateInfo[j].ColumnType;
                    int IndexColumnInFile = templateInfo[j].IndexColumnInFile;

                    string RowCSVFileValInCell;

                    RowCSVFileValInCell = RowCSVFile[IndexColumnInFile]; //сохраняем значение 

                    object ObjectCellVal = RowCSVFileValInCell; //переприсваивание для проверки конвертации
                    string CommentError = "";
                    string CommentChange = "";

                    SQLComm = SQLComm + "@" + ColumnNameInDB + ", ";
                    SQL_param[j] = new SqlParameter();
                    SQL_param[j].ParameterName = "@" + ColumnNameInDB;

                    if (ColumnNameInDB == "OrderDate" || ColumnNameInDB == "FromShipperDate" || ColumnNameInDB == "ToConsigneeDate" || ColumnNameInDB == "CreateDate")
                    {
                        DateTime dt_end = DateTime.MinValue;
                        if (ObjectCellVal.ToString().Length == 8)
                            dt_end = DateTime.ParseExact(ObjectCellVal.ToString(), "dd.MM.yy", CultureInfo.InvariantCulture);
                        if (ObjectCellVal.ToString().Length == 10)
                            dt_end = DateTime.ParseExact(ObjectCellVal.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

                        SQL_param[j].Value = dt_end;
                    }
                    else if (ColumnNameInDB == "FromShipperTime" || ColumnNameInDB == "ToConsigneeTime" || ColumnNameInDB == "CreateTime")
                    {
                        DateTime dateTime = DateTime.ParseExact(ObjectCellVal.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                        SQL_param[j].Value = dateTime;
                    }
                    else if (ColumnNameInDB == "Weight")
                    {
                        SQL_param[j].Value = ObjectCellVal.ToString().Replace(",", ".");
                    }
                    else
                    {
                        SQL_param[j].Value = ObjectCellVal.ToString().Replace("\"\"", "\"");
                    }
                }

                SQLComm = SQLComm + "@CreateDatetime1"; //+ ", ";
                SQL_param[j] = new SqlParameter();
                SQL_param[j].ParameterName = "@CreateDatetime1";
                SQL_param[j].Value = DateTime.Now;
                db.Database.ExecuteSqlCommand(SQLComm, SQL_param);

            }

            return true; //переход на форму полного успешного импорта

        }


        public IEnumerable<MenuAccessViewModel> GetMenuTree()
        {
            return (from menuItem in db.MenuStructure
                    select new MenuAccessViewModel()
                    {
                        Id = menuItem.Id,
                        menuName = menuItem.menuName,
                        menuHtmlId = menuItem.menuId,
                        parentId = menuItem.parentId,
                        isLeaf = (menuItem.parentId != null) ? true : false,
                        menuIdForScript = menuItem.Id
                    }).ToList();

            var menuInfo = db.MenuStructure.ToList();

        }

        public MenuAccessViewModel GetMenu(int menuId)
        {
            return Mapper.Map(db.MenuStructure.AsNoTracking().FirstOrDefault(u => u.Id == menuId));
        }

        public List<MenuRoleViewModel> GetMenuRoles(int menuid)
        {
            var adminRoleId = GlobalConsts.GetAdminRoleId();

            return (from R in db.AspNetRoles
                    where (R.MenuStructure.Any(m => m.Id == menuid)
                        && R.Id != adminRoleId)
                    select new MenuRoleViewModel()
                    {
                        RoleId = R.Id,
                        RoleName = R.Name,
                        Assigned = true

                    })
                    .Union
                    (from R in db.AspNetRoles
                     where (!(R.MenuStructure.Any(m => m.Id == menuid)))
                        && (R.Id != adminRoleId)
                     select new MenuRoleViewModel()
                     {
                         RoleId = R.Id,
                         RoleName = R.Name,
                         Assigned = false

                     }).ToList();
        }

        public void AssignMenuRoles(int menuId, string[] roles)
        {
            var adminRoleId = GlobalConsts.GetAdminRoleId();
            var allRoles = db.AspNetRoles.Where(r => r.Id != adminRoleId).ToList();

            foreach (var role in allRoles)
            {
                RemoveMenuRole(menuId, role.Id);
            }

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    AddMenuRole(menuId, role);
                }
            }
        }

        public void AddMenuRole(int menuId, string roleId)
        {
            var adminRole = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId);
            var menuInfo = db.MenuStructure.FirstOrDefault(u => u.Id == menuId);
            if ((adminRole == null) || (menuInfo == null)) return;
            {
                if (MenuHasRole(menuId, roleId)) return;
                menuInfo.AspNetRoles.Add(adminRole);
                db.SaveChanges();
            }
        }

        public void RemoveMenuRole(int menuId, string roleId)
        {
            var adminRole = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId);
            var menuInfo = db.MenuStructure.FirstOrDefault(u => u.Id == menuId);
            if ((adminRole == null) || (menuInfo == null)) return;
            {
                if ((!MenuHasRole(menuId, roleId))) return;
                menuInfo.AspNetRoles.Remove(adminRole);
                db.SaveChanges();
            }
        }

        public bool MenuHasRole(int menuId, string roleId)
        {
            //var menuInfo = db.MenuStructure.FirstOrDefault(u => u.Id == menuId);
            //return ((menuInfo != null) && (menuInfo.AspNetRoles.Any(r => r.Id == roleId)));

            var menuInfo = db.MenuStructure
                   .Include("AspNetRoles")
                   .Where(x => x.Id == menuId);

            var roleInfo = menuInfo.Where(x => x.AspNetRoles.Any(r => r.Id == roleId)).ToList();

            var result = roleInfo.Count();
            return (result > 0);


        }

        public bool UserHasMenuRole(string userId, int menuId)
        {
            var isUserHasMenuRole = db.IsMenuHasRole(userId, menuId).ToList();

            if (isUserHasMenuRole.Count() > 0)
                return (isUserHasMenuRole.FirstOrDefault().Value) > 0;

            return false;
        }

        //корневой элемент или нет?
        public bool IsMenuRoot(int menuId)
        {
            var menuInfo = db.MenuStructure.FirstOrDefault(u => u.Id == menuId);

            if (menuInfo == null) return false;

            return (menuInfo.parentId == null);
        }


        public bool UserHasMenuRootRole(string userId, int menuId)
        {

            if (IsMenuRoot(menuId))
            {
                //если меню корень то  делаем проверку
                var childMenuItems = db.MenuStructure.Where(x => x.parentId == menuId);

                //на то есть ли хоть один разрешенный ребенок
                var childResult = false;
                foreach (var childMenuItem in childMenuItems)
                {
                    if (UserHasMenuRole(userId, childMenuItem.Id))
                    {
                        childResult = true;
                        break;
                    }
                }

                //+ есть ли разрешение на сам корень
                var root_result = UserHasMenuRole(userId, menuId);
                var result = root_result || childResult;

                return result;
            }
            else
            {   //если не корень, то ищем корень и проверяем права сразу на двух узлах
                var menuInfo = db.MenuStructure.FirstOrDefault(u => u.Id == menuId);
                if (menuInfo == null) return false;

                int root = (int)menuInfo.parentId;

                //показываем лист если есть на корне или на нем самом
                var result = (UserHasMenuRole(userId, root) || UserHasMenuRole(userId, menuId));

                return result;

            }
        }


        public IEnumerable<MenuAccessViewModel> UserGetMenuTree(string userId)
        {
            var menuInfo = db.MenuStructure.ToList();

            return (from menuItem in menuInfo
                    where UserHasMenuRootRole(userId, menuItem.Id)
                    select new MenuAccessViewModel()
                    {
                        Id = menuItem.Id,
                        menuName = menuItem.menuName,
                        menuHtmlId = menuItem.menuId,
                        parentId = menuItem.parentId
                    }).ToList();

        }

        public List<UserViewModel> GetUsers(string searchTerm, int pageSize, int pageNum)
        {

            return GetUsersBySearchString(searchTerm)
                        .Skip(pageSize * (pageNum - 1))
                         .Take(pageSize)
                           .ToList();

        }

        public IQueryable<UserViewModel> GetUsersBySearchString(string searchTerm)
        {
            return
            db.AspNetUsers
                  .AsNoTracking()
                  .Where(s => (s.Dismissed == false) && (((s.DisplayName.Contains(searchTerm) || s.DisplayName.StartsWith(searchTerm) || s.DisplayName.EndsWith(searchTerm)))))
                        .Select(Mapper.Map)
                         //.Where(o => o.isAdmin == false)
                         .OrderBy(o => o.displayName)
                         .AsQueryable();
        }

        public int GetUserCount(string searchTerm)
        {
            return GetUsersBySearchString(searchTerm).Count();
        }


        public void CloneRolesForUser(string ReceiverId, string UserId)
        {
            var userRoles = getUserRoles(UserId).Where(x => x.assigned == true);
            var receiverRoles = getUserRoles(ReceiverId).Where(x => x.assigned == true);

            List<string> roles = new List<string>();

            foreach (var role in receiverRoles)
            {
                //проверяем чтобы не было дублирования ролей (т.е. если уже есть такая роль у ReceiverId, то ее не добавлять)
                if (!(userRoles.Any(x => x.roleId == role.roleId)))
                    roles.Add(role.roleId);
            }

            string[] allRoles = roles.ToArray();

            //AssignRoles(UserId, allRoles);

            //добавляем роли
            if (allRoles != null)
            {
                foreach (var role in allRoles)
                {
                    AddRole(UserId, role);
                }
            }
        }

        public void UpdateUserProfile(UserProfileViewModel model)
        {
            var dbInfo = db.UserProfile.FirstOrDefault(u => u.UserId == model.UserId);
            if (dbInfo == null)
            { //добавление
                var UserProfileInfo = new UserProfile();

                if (UserProfileInfo != null)
                {
                    UserProfileInfo.UserId = model.UserId;
                    UserProfileInfo.City = model.City;
                    UserProfileInfo.CountryId = model.CountryId;
                    UserProfileInfo.AdressFrom = model.AdressFrom;
                    UserProfileInfo.isFinishStatuses = model.isFinishStatuses;

                    db.UserProfile.Add(UserProfileInfo);
                    db.SaveChanges();
                };
            }
            else
            { //редактирование               
                dbInfo.City = model.City;
                dbInfo.CountryId = model.CountryId;
                dbInfo.AdressFrom = model.AdressFrom;
                dbInfo.isFinishStatuses = model.isFinishStatuses;

                db.SaveChanges();
            }
        }

        public UserProfileViewModel getUserProfile(string userId)
        {
            if (db.UserProfile.FirstOrDefault(u => u.UserId == userId) != null)
                return Mapper.Map(db.UserProfile.AsNoTracking().FirstOrDefault(u => u.UserId == userId));
            else
                return null;
        }

        public string getUserName(string userId)
        {
            return db.AspNetUsers.FirstOrDefault(u => u.Id == userId).DisplayName;
        }

        public IQueryable<UserMessagesViewModel> getUserMessagesIn(string userId)
        {

            return db.UserMessages
                            .AsNoTracking()
                            .Where(x => x.CreatedToUser == userId)
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.DateTimeCreate)
                               .AsQueryable();
        }

        public IQueryable<UserMessagesViewModel> getUserMessagesOut(string userId)
        {
            return db.UserMessages
                            .AsNoTracking()
                            .Where(x => x.CreatedFromUser == userId)
                             .Select(Mapper.Map)
                              .OrderByDescending(o => o.DateTimeCreate)
                               .AsQueryable();
        }

        public bool NewMessage(UserMessagesViewModel model)
        {
            try
            {

                var msg = new UserMessages();

                if (msg != null)
                {
                    msg.MessageText = model.MessageText;
                    msg.DateTimeCreate = DateTime.Now;
                    msg.CreatedFromUser = model.CreatedFromUser;
                    msg.CreatedToUser = model.CreatedToUser;
                    msg.MessageSubject = model.MessageSubject;
                    msg.OrderId = model.OrderId;

                    db.UserMessages.Add(msg);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public UserMessagesViewModel getUserMessage(int Id)
        {
            return Mapper.Map(db.UserMessages.FirstOrDefault(or => or.Id == Id));
        }

        public bool UpdateDateMessageOpen(UserMessagesViewModel model)
        {
            try
            {
                var mes = db.UserMessages.FirstOrDefault(p => p.Id == model.Id);

                if (mes != null)
                {
                    mes.DateTimeOpen = DateTime.Now;

                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }

        }

        public int GetUserCountMessages(string userId)
        {
            var userMsg = getUserMessagesIn(userId).Where(x => x.DateTimeOpen == null).Count();
            return userMsg;
        }

        public int checkEmailExist(string userEmail, string userId)
        {
            var emailCnt = db.AspNetUsers.Where(x => x.Email == userEmail && x.Id != userId).Count();
            return emailCnt;
        }
    }
}
