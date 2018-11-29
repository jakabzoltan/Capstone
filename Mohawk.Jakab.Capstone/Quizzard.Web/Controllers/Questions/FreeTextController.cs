using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Mohawk.Jakab.Quizzard.Services.Interfaces;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.Areas.Questions;
using Constants = Mohawk.Jakab.Quizzard.Domain.Constants;

namespace Quizzard.Web.Controllers.Questions
{
    [RouteArea("Questions")]
    [RoutePrefix("Questions")]
    public class FreeTextController : QuestionBaseController
    {
        private IQuizService QuizService { get; set; }
        private IUserQuestionPoolService QuestionPoolService { get; set; }
        public FreeTextController()
        {

        }
        public FreeTextController(IQuizService quizService, IUserQuestionPoolService questionPoolService)
        {
            QuizService = quizService;
            QuestionPoolService = questionPoolService;
        }

        public override async Task<PartialViewResult> QuestionPlayView(QuizSubmissionAnswerModel model)
        {
            return PartialView("QuestionPlayView", model);
        }


        public override async Task<PartialViewResult> EditQuestion(string quizId, string id)
        {
            QuestionModel model = null;
            if (!QuizService.QuestionExists(id))
            {
                model = new QuestionModel() { Id = id, QuizId = quizId };
            }
            else
            {
                model = await QuizService.GetQuestion(id);
            }
            var partial = PartialView("EditQuestion", model);

            return partial;
        }

        public override async Task<PartialViewResult> EditUserOwnedQuestion(string questionId)
        {
            QuestionModel model = null;
            if ((await QuestionPoolService.GetUserOwnedQuestion(questionId)) == null)
            {
                model = new QuestionModel() { Id = questionId, UserOwned = true };
            }
            else
            {
                model = await QuestionPoolService.GetUserOwnedQuestion(questionId);
            }
            return PartialView("EditUserQuestion", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveUserQuestion(QuestionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AlertSaveFailed");
            }

            model.QuestionTypeId = Constants.QuestionTypes.FreeText;
            try
            {
                //user pool
                bool success;
                var question = await QuestionPoolService.GetUserOwnedQuestion(model.Id);
                if (question == null)
                    success = await QuestionPoolService.AddUserOwnedQuestion(new UserOwnedQuestionModel(User.Identity.GetUserId(), model));
                else
                    success = await QuestionPoolService.EditUserOwnedQuestion(new UserOwnedQuestionModel(User.Identity.GetUserId(), model));

                if (success)
                    return PartialView("_AlertSaveSuccess");
                return PartialView("_AlertSaveFailed");
            }
            catch (Exception e)
            {
                return PartialView("_AlertSaveFailed");
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveQuestion(QuestionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AlertSaveFailed");
            }
            model.QuestionTypeId = Constants.QuestionTypes.FreeText;
            if (model.QuizId == null) await SaveUserQuestion(model);
            try
            {

                bool success;
                if (!QuizService.QuestionExists(model.Id))
                    success = await QuizService.AddQuestion(model.QuizId, model);
                else
                    success = await QuizService.EditQuestion(model.Id, model);

                if (!success)
                {
                    return PartialView("_AlertSaveFailed");
                }
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