using System;
using System.Collections.Generic;
using System.Linq;
using Corum.Models.ViewModels;

namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        Dictionary<string, string> GetRestColumnsForGroupReport(string currentFieldName);
        SnapshotInfoViewModel GetMaxScreenShot();
        SnapshotInfoViewModel GetScreenShotById(int Id);

        IQueryable<BriefViewModel> GetBriefDataByScreenShotId(int screenShotId, GroupItemFilters filters);
        IQueryable<SnapshotInfoViewModel> GetSnapshotLists();
        IQueryable<SnapshotInfoViewModel> GetScreenShots(DateTime actualDate);

        RestViewModel GetRestById(string InnerPartyKey, int snapshotId);
        List<DocViewModel> GetDocsById(string InnerPartyKey, int snapshotId);


        IQueryable<RestViewModel> GetRestsByScreenShotId(int screenShotId, GroupItemFilters filters);
        IQueryable<DocViewModel> GetDocsByScreenShotId(int screenShotId, GroupItemFilters filters);
        IQueryable<GroupItemRestViewModel> GetGroupRestsBySnapShotId(int screenShotId, string FieldName, GroupItemFilters filters);
        IQueryable<RestViewModel> GetGroupRecieverRestsByScreenShotId(int screenShotId, string storage);

        List<GroupItemViewModel> GetStorages(int snapShot, string searchTerm, int pageSize, int pageNum);
        int GetStoragesCount(int snapShot, string searchTerm);
        List<GroupItemViewModel> GetCenters(int snapShot, string searchTerm, int pageSize, int pageNum);
        int GetCentersCount(int snapShot, string searchTerm);
        List<GroupItemViewModel> GetRecievers(int snapShot, string searchTerm, int pageSize, int pageNum);
        int GetRecieversCount(int snapShot, string searchTerm);
        List<GroupItemViewModel> GetBalanceKeepers(int snapShot, string searchTerm, int pageSize, int pageNum);
        int GetBalanceKeepersCount(int snapShot, string searchTerm);
        List<GroupItemViewModel> GetProducers(int snapShot, string searchTerm, int pageSize, int pageNum);
        int GetProducersCount(int snapShot, string searchTerm);
        List<RestViewModel> GetProjects(int snapShot, string searchTerm, int pageSize, int pageNum);
        int GetProjectsCount(int snapShot, string searchTerm);
        List<RestViewModel> GetBarcodes(int snapShot, string searchTerm, int pageSize, int pageNum);
        int GetBarcodesCount(int snapShot, string searchTerm);
    }
}
