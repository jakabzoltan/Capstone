using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mohawk.Jakab.Quizzard.Startup))]
namespace Mohawk.Jakab.Quizzard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
