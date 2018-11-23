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
                Route = new DynamicDataRoute("{controller}/{action}/{id}")
            }));
        }

        public string Name => GetType().Name.Replace("Controller", "");
        
        [HttpGet]
        public abstract Task<PartialViewResult> QuestionPlayView(QuizSubmissionAnswerModel questionModel);

        [HttpGet]
        public abstract Task<PartialViewResult> QuestionDetails(string id);

        [HttpGet]
        public abstract Task<PartialViewResult> EditQuestion(string quizId, string questionId);

        public ActionResult ExecuteActionByRoute(RouteData route)
        {
            ControllerContext = new ControllerContext(new HttpContextWrapper(System.Web.HttpContext.Current), route, this);
            return RedirectToRoute(route.Values);
        }
  
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