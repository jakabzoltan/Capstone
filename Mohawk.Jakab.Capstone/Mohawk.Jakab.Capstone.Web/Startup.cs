using Microsoft.Owin;
using Mohawk.Jakab.Quizzard.Web;
using Owin;


[assembly: OwinStartup(typeof(Startup), "Capstone")]
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
