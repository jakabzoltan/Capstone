using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.Models;

namespace Quizzard.Web.Areas.Questions.Controllers
{
    [RouteArea("Questions")]
    [RoutePrefix("Questions")]
    public class MultipleChoiceController : QuestionBaseController
    {
        public MultipleChoiceController()
        {
            
        }
        public override Task<PartialViewResult> QuestionPlayView(QuizSubmissionAnswerModel model)
        {
            throw new NotImplementedException();
        }

        public override Task<PartialViewResult> EditQuestion(string quizId, string id)
        {
            throw new NotImplementedException();
        }

        public override Task<PartialViewResult> EditUserOwnedQuestion(string questionId)
        {
            throw new NotImplementedException();
        }

        public ActionResult SaveQuestion(QuestionModel model)
        {
            throw new NotImplementedException();
        }
    }
}