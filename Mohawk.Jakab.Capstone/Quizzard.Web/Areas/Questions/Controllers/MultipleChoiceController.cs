using System;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.Models;

namespace Quizzard.Web.Areas.Questions.Controllers
{
    [RouteArea("Questions")]
    public class MultipleChoiceController : QuestionBaseController
    {
        public MultipleChoiceController()
        {
            
        }
        public override PartialViewResult QuestionPlayView(QuizSubmissionAnswerModel questionModel)
        {
            throw new NotImplementedException();
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

        public ActionResult SaveQuestion(QuestionModel model)
        {
            throw new NotImplementedException();
        }
    }
}