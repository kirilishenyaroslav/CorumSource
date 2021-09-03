using Microsoft.Owin;
using Owin;
using CorumAdminUI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using CorumAdminUI.Models;
using System.Collections.Generic;
using System.Diagnostics;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Hangfire.AspNet;
using CorumAdminUI.HangFireTasks;

[assembly: OwinStartupAttribute(typeof(BarnivannAdminUI.Startup))]
namespace BarnivannAdminUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangFireAuthorizationFilter() }
            });

            RecurringJob.AddOrUpdate<HangFireTasks>(x => x.ListTasks(true), "0 23 * * *");
        }
    }
}
