using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.Models;

namespace Quizzard.Web.Areas.Questions
{
    public abstract class QuestionBaseController : Controller 
    {
        [HttpGet]
        public abstract PartialViewResult QuestionPlayView(QuizSubmissionAnswerModel questionModel);

        [HttpGet]
        public abstract PartialViewResult QuestionDetails(string id);

        [HttpGet]
        public abstract PartialViewResult CreateQuestion(string quizId = null);

        [HttpGet]
        public abstract PartialViewResult EditQuestion(string id);

    }
}