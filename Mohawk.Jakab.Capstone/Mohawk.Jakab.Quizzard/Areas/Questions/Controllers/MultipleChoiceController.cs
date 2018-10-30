using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Areas.Questions.Controllers
{
    [RouteArea("Questions")]
    public class MultipleChoiceController : QuestionBaseController<QuestionModel>
    {
        public override PartialViewResult QuestionPlayView(QuizSubmissionAnswerModel questionModel)
        {
            throw new NotImplementedException();
        }

        public override ActionResult QuestionDetails(string id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult CreateQuestion()
        {
            throw new NotImplementedException();
        }

        public override ActionResult EditQuestion(string id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult SaveQuestion(QuestionModel model)
        {
            throw new NotImplementedException();
        }
    }
}