using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace Mohawk.Jakab.Quizzard.Web
{
    public class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);


            // Register components
            




            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}