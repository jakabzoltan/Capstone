using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Domain;
using Mohawk.Jakab.Quizzard.Services.Handlers;
using Mohawk.Jakab.Quizzard.Services.Interfaces;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.Models;

namespace Quizzard.Web.Areas.Questions.Controllers
{
    [RouteArea("Questions")]
    [RoutePrefix("Questions")]
    public class FreeTextController : QuestionBaseController
    {
        private IQuizService QuizService { get; set; }
        public FreeTextController()
        {

        }
        public FreeTextController(IQuizService quizService)
        {
            QuizService = quizService;
        }

        public override async Task<PartialViewResult> QuestionPlayView(QuizSubmissionAnswerModel model)
        {
            return PartialView("QuestionPlayView", model);
        }

        public override async Task<PartialViewResult> QuestionDetails(string id)
        {
            throw new NotImplementedException();
        }
        
        public override async Task<PartialViewResult> EditQuestion(string quizId, string id)
        {
            QuestionModel model = null;
            if (!QuizService.QuestionExists(id))
            {
                model = new QuestionModel() { Id = id, QuizId = quizId};
            }
            else
            {
                model = QuizService.GetQuestion(id).Result;
            }
            var partial = PartialView("EditQuestion",model);

            return partial;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveQuestion(QuestionModel model)
        {
            try
            {
                model.QuestionTypeId = Constants.QuestionTypes.FreeText;
                if(!QuizService.QuestionExists(model.Id))
                    await QuizService.AddQuestion(model.QuizId, model);
                else 
                    await QuizService.EditQuestion(model.Id, model);
            }
            catch (Exception e)
            {
                return PartialView("_AlertSaveFailed");
            }
            return PartialView("_AlertSaveSuccess");

        }

        public ActionResult AddQuestionAnswer()
        {
            return PartialView("Questions/FreeText/_AnswerPartial", new AnswerModel());
        }


    }
}