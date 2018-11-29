using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Mohawk.Jakab.Quizzard.Domain;
using Mohawk.Jakab.Quizzard.Services.Handlers;
using Mohawk.Jakab.Quizzard.Services.Interfaces;
using Quizzard.Web.Areas.Questions;
using Quizzard.Web.Areas.Questions.Controllers;
using Quizzard.Web.Controllers.Questions;

namespace Quizzard.Web
{
    public static class AutofacSetup
    {
        public static IContainer Container { get; set; }

        public static void Build()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //Register Dependecies

            builder.RegisterType<MultipleChoiceController>().Keyed<QuestionBaseController>(Constants.QuestionTypes.MultipleChoice);
            builder.RegisterType<FreeTextController>().Keyed<QuestionBaseController>(Constants.QuestionTypes.FreeText);
            builder.RegisterType<QuizPlayService>().As<IQuizPlayService>();
            builder.RegisterType<QuizReactionService>().As<IQuizReactionService>();
            builder.RegisterType<QuizService>().As<IQuizService>();
            builder.RegisterType<UserQuestionPoolService>().As<IUserQuestionPoolService>();

            builder.RegisterType<UserService>().AsSelf();

            builder.RegisterType<SessionStateTempDataProvider>().As<ITempDataProvider> ();
            //builder.RegisterType<RouteCollection>().As(RouteTable.Routes);

            //builder.RegisterModule<AutofacWebTypesModule>();

            

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            Container = container;
        }
    }
}