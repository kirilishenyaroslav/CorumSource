using Microsoft.Owin;
using Owin;
using CorumAdminUI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using CorumAdminUI.Models;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Hangfire.AspNet;
using System.Collections.Generic;
using System.Diagnostics;

[assembly: OwinStartupAttribute(typeof(BarnivannAdminUI.Startup))]
namespace BarnivannAdminUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            // Let's also create a sample background job
            BackgroundJob.Enqueue(() => Debug.WriteLine("Hello world from Hangfire!"));
        }
    }
}
