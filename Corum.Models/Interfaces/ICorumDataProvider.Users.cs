using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels;
using Corum.Models.ViewModels.Admin;
using System;

namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        string GetDisplayName(string _userId);
        IQueryable<LoginHistoryViewModel> getSessionLog();
        bool AddSessionInfo(LoginHistoryViewModel model);

        IQueryable<UserViewModel> getUsers(string Search);
        UserViewModel getUser(string userId);
        void UpdateUser(UserViewModel model);
        void AddRole(string userId, string roleId);
        void RemoveRole(string userId, string roleId);
        bool UserHasRole(string userId, string roleId);
        void RemoveUser(string userId);
        List<UserRoleViewModel> getUserRoles(string userid);
        void AssignRoles(string userId, string[] roles);
        void UpdateUserDisplayName(string userId, string displayName, string postName, string contactPhone);
        IEnumerable<MenuAccessViewModel> MenuUserRole(string userId);
        bool IsUserAdmin(string userId);

        List<ImportTemplateInfo> GetImportTemplateInfo(int FileType);
        bool DoImport(ConfiguredByUserColumsPairsModel preImportConfig, string[] HeadersCSVFile,
            string[] DataCSVFile, string[] FirstDataRowCSVFileref, ref string guidSessionString, ref int Id_Snapshot);
        bool DoImportOrders(ConfiguredByUserColumsPairsModel preImportConfig, string[] HeadersCSVFile,
            string[] DataCSVFile, string[] FirstDataRowCSVFileref, ref string guidSessionString, ref int Id_Snapshot);
        bool DoImportTruckOrders(ConfiguredByUserColumsPairsModel preImportConfig, string[] HeadersCSVFile,
            string[] DataCSVFile, string[] FirstDataRowCSVFileref, ref string guidSessionString, ref int Id_Snapshot);
        string GetColumnType(bool isRests, string ColumnName);
        List<ColumnSettingsModel> GetConfigColumn(bool isRests);
        List<ColumnSettingsModel> GetImportConfig();
        List<ColumnNameModel> GetImportColumnName();
        IQueryable<ImportError> getImportErrors(int idSnapshot, string logId);

        int GetDefaultSnapshotId();
        bool DeleteSnapshot(int Id);
        bool MakeSnapshotAsDefault(int Id);
        bool MakeSnapshotAsArchive(int Id);
        bool GetDateSnapshot(int IdSnapshot, ref string ActualDateBeg, ref string ActualDateEnd);
        bool GetCommentColumnName(bool IsRest, string ColumnName, ref string CommentColumnName);

        IEnumerable<MenuAccessViewModel> GetMenuTree();

        MenuAccessViewModel GetMenu(int menuId);
        List<MenuRoleViewModel> GetMenuRoles(int menuid);
        void AssignMenuRoles(int menuId, string[] roles);
        IEnumerable<MenuAccessViewModel> UserGetMenuTree(string userId);

        IQueryable<UserViewModel> getUsersForClone(string userId);

        List<UserViewModel> GetUsers(string searchTerm, int pageSize, int pageNum);

        IQueryable<UserViewModel> GetUsersBySearchString(string searchTerm);

        int GetUserCount(string searchTerm);

        void CloneRolesForUser(string ReceiverId, string UserId);
        void UpdateUserProfile(UserProfileViewModel model);

        UserProfileViewModel getUserProfile(string userId);

        string getUserName(string userId);

        IQueryable<UserMessagesViewModel> getUserMessagesIn(string userId);
        IQueryable<UserMessagesViewModel> getUserMessagesOut(string userId);
        bool NewMessage(UserMessagesViewModel model);
        UserMessagesViewModel getUserMessage(int Id);
        bool UpdateDateMessageOpen(UserMessagesViewModel model);
        int GetUserCountMessages(string userId);
        int checkEmailExist(string userEmail, string userId);
    }
}
