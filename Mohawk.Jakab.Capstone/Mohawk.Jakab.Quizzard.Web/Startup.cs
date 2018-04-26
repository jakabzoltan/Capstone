using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mohawk.Jakab.Quizzard.Web.Startup))]
namespace Mohawk.Jakab.Quizzard.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
