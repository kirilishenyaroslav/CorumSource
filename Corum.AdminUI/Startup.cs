using Microsoft.Owin;
using Owin;
using CorumAdminUI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using CorumAdminUI.Models;
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
        }
    }
}
