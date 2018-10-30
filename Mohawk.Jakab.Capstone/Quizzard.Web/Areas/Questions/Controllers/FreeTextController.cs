using System;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.Models;

namespace Quizzard.Web.Areas.Questions.Controllers
{
    [RouteArea("Questions")]
    public class FreeTextController : QuestionBaseController
    {
        public FreeTextController()
        {
            
        }
        public override PartialViewResult QuestionPlayView(QuizSubmissionAnswerModel questionModel)
        {
            return PartialView("_QuestionPlayView", questionModel);
        }

        public override PartialViewResult QuestionDetails(string id)
        {
            throw new NotImplementedException();
        }

        public override PartialViewResult CreateQuestion(string quizId = null)
        {
            throw new NotImplementedException();
        }

        public override PartialViewResult EditQuestion(string id)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveQuestion(QuestionModel model)
        {
            throw new NotImplementedException();
        }
    }
}