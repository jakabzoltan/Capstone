using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Routing;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.Models;

namespace Quizzard.Web.Areas.Questions
{
    [RouteArea("Questions")]
    public abstract class QuestionBaseController : Controller
    {
        public QuestionBaseController()
        {
            Initialize(new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData()
            {
                Route = new DynamicDataRoute(Name+"/{action}/{id}")
            }));

            ControllerContext = new ControllerContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData()
            {
                Values =
                {
                    {"controller", Name},
                }
            }, this);
        }

        public string Name => GetType().Name.Replace("Controller", "");
        
        [HttpGet]
        public abstract Task<PartialViewResult> QuestionPlayView(QuizSubmissionAnswerModel questionModel);

        [HttpGet]
        public abstract Task<PartialViewResult> EditQuestion(string quizId, string questionId);

        public abstract Task<PartialViewResult> EditUserOwnedQuestion(string questionId);


        public void ContextualizeController(string actionName)
        {
            ControllerContext = new ControllerContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData()
            {
                Values =
                {
                    {"action", actionName},
                    {"controller", Name},
                }
            }, this);
        }
    }
}