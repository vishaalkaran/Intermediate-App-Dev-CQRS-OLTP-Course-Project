using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eTools_2018_E01_Team_F_WebSite.Startup))]
namespace eTools_2018_E01_Team_F_WebSite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
