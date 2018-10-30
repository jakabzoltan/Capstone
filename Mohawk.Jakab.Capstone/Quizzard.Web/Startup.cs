using Microsoft.Owin;
using Owin;
using Quizzard.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace Quizzard.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
