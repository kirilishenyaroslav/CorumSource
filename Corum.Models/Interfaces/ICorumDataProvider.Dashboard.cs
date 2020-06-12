using System;
using System.Collections.Generic;
using Corum.Models.ViewModels.Dashboard;


namespace Corum.Models
{
    public partial interface ICorumDataProvider
    {
        List<DashboardViewModelItem> getBPInfoByUser(DateTime dateStart, string userId, bool isAdmin = false, bool isFinishStatuses = false);
        bool getFinishStatusesByUserId(string userId);
    }
}
