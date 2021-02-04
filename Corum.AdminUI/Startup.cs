using Microsoft.Owin;
using Owin;

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
