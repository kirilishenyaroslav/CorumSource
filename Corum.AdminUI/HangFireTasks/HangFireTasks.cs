using Corum.Models;
using Corum.Models.Tender;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace CorumAdminUI.HangFireTasks
{
    public class HangFireTasks
    {
        protected Corum.Models.ICorumDataProvider context;
        List<RegisterTenders> listRegistryTenders;

        public HangFireTasks()
        {
            listRegistryTenders = new List<RegisterTenders>();
            context = DependencyResolver.Current.GetService<ICorumDataProvider>();
        }
        public async Task GetStatusTender()
        {
            try
            {
                listRegistryTenders = context.GetRegisterTenders();
            }
            catch (Exception e)
            { 
            
            }
            await Task.Run(() => Debug.WriteLine($"{listRegistryTenders[0].processValue}"));
        }
    }
}