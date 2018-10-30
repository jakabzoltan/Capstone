using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Areas.Questions
{
    public abstract class QuestionBaseController<T> : Controller where T : QuestionModel
    {
        public abstract PartialViewResult QuestionPlayView(QuizSubmissionAnswerModel questionModel);

        [HttpGet]
        public abstract ActionResult QuestionDetails(string id);
        [HttpGet]
        public abstract ActionResult CreateQuestion();

        [HttpGet]
        public abstract ActionResult EditQuestion(string id);

        [HttpPost]
        public abstract ActionResult SaveQuestion(T model);

    }
}