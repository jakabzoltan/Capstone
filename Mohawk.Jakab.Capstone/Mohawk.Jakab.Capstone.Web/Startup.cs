using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mohawk.Jakab.Capstone.Web.Startup))]
namespace Mohawk.Jakab.Capstone.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
