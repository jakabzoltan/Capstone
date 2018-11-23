﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Mohawk.Jakab.Quizzard.Services.Interfaces;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.App_Data;
using Quizzard.Web.Areas.Questions;
using Quizzard.Web.Models;

namespace Quizzard.Web.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        public IQuizService QuizService { get; set; }
        public IQuizPlayService QuizPlayService { get; set; }
        public IUserQuestionPoolService UserQuestionPool { get; set; }
        public QuizController()
        {

        }

        public QuizController(IQuizService quizService, IQuizPlayService quizPlayService, IUserQuestionPoolService userQuestionPool)
        {
            QuizService = quizService;
            QuizPlayService = quizPlayService;
            UserQuestionPool = userQuestionPool;
        }

        // GET: Quiz
        public ActionResult Create()
        {
            return View(new QuizModel());
        }

        public async Task<ActionResult> ToggleDraftMode(string id)
        {
            var mode = QuizService.ToggleDraftMode(id);
            return mode ? RedirectToAction("Edit", new { id }) : RedirectToAction("Details", new { id });
        }

        public async Task<ActionResult> Edit(string id)
        {
            var quiz = await QuizService.GetQuiz(id);
            if (!quiz.DraftMode) return RedirectToAction("Details", new { id });
            return View(quiz);
        }

        public async Task<ActionResult> Details(string id)
        {
            var quiz = await QuizService.GetQuiz(id);
            if (quiz.DraftMode) return RedirectToAction("Edit", new { id });
            return View(quiz);
        }

        public async Task<ActionResult> RefreshQuizQuestions(string id)
        {
            var questions = await QuizService.GetQuizQuestions(id);
            return PartialView("_DraftQuestions", questions.QuizQuestions.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveQuiz(QuizModel model)
        {
            model.QuizzardUserId = User.Identity.GetUserId();
            if (!ModelState.IsValid) return View(model.Id.IsNullOrWhiteSpace() ? "Create" : "Edit", model);
            if (model.Id.IsNullOrWhiteSpace()) //create
            {
                var newModel = await QuizService.CreateQuiz(model);
                return RedirectToAction("Edit", new { id = newModel.Id });
            }
            await QuizService.EditQuiz(model.Id, model);
            return RedirectToAction("ToggleDraftMode", new { id = model.Id });
        }

        public ActionResult ChooseQuestionStore(string quizId)
        {
            return PartialView("_ModalQuestionStoreSelection", quizId);
        } 

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Play(string id)
        {
            var quizSubmissionId = Guid.NewGuid().ToString();
            var quiz = await QuizService.GetQuiz(id);

            var model = new Play
            {
                Id = quizSubmissionId,
                QuizzardUserId = User.Identity.GetUserId(),
                Quiz = quiz
            };


            var partialList = new List<ViewWrapper>();
            foreach (var question in quiz.Questions)
            {
                using (var container = AutofacSetup.Container.BeginLifetimeScope())
                {
                    var controller = container.ResolveKeyed<QuestionBaseController>(question.QuestionTypeId);
                    var emptySubmissionAnswer = new QuizSubmissionAnswerModel(quizSubmissionId, question);
                    model.Answers.Add(emptySubmissionAnswer);
                    controller.ContextualizeController(nameof(controller.QuestionPlayView));
                    var result = await controller.QuestionPlayView(emptySubmissionAnswer);
                    var viewLocations = result.ViewEngineCollection.FindPartialView(controller.ControllerContext, "QuestionPlayView");
                    result.View = viewLocations.View;
                    model.QuestionPartials.Add(new ViewWrapper((RazorView) viewLocations.View, result.Model));
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Play(Play model) //todo change
        {
            


            return View(model);
        }

        //id = quiz submission id
        public ActionResult QuizResult(string id)
        {
            return View();
        }

        public async Task<ActionResult> MyQuizzes()
        {
            var list = await QuizService.GetMyQuizzes(User.Identity.GetUserId());
            return View(list);
        }

        public ActionResult QuizStats(string id)
        {
            return View();
        }

        public async Task<ActionResult> AddNewQuestion(string quizId, string questionType)
        {

            using (var scope = AutofacSetup.Container.BeginLifetimeScope())
            {
                var controller = scope.ResolveKeyed<QuestionBaseController>(questionType);
                controller.ContextualizeController(nameof(controller.EditQuestion));
                var res = await controller.EditQuestion(quizId, Guid.NewGuid().ToString());

                var viewLocations = res.ViewEngineCollection.FindPartialView(controller.ControllerContext, "EditQuestion");
                res.View = viewLocations.View;

                var mod = new ViewWrapper((RazorView) res.View, res.Model);
                return View("_ViewWrapper", mod);
            }


        }

        public ActionResult QuestionTypeSelection(string quizId)
        {
            return PartialView("_QuestionTypeSelectionModal", quizId);
        }

        public async Task<ActionResult> AttachQuestionResults(string quizId)
        {
            var set = new QuestionSet()
            {
                QuizId = quizId,
                QuizQuestions = await UserQuestionPool.GetUserOwnedQuestions(User.Identity.GetUserId())
            };
            return View("_AttachQuestionsStore", set);
        }

        public async Task<ActionResult> AttachQuestion(string quizId, string questionId)
        {
            return Json(await QuizService.AttachUserQuestionToQuiz(quizId, questionId));
        }

        public ActionResult ArchiveQuiz(string id)
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<ActionResult> RemoveQuestion(QuestionModel model)
        {
            if (model.UserOwned)
            {
                await QuizService.DetachUserQuestionFromQuiz(model.QuizId, model.Id);
            }
            else
            {
                await QuizService.RemoveQuestion(model.Id);
            }
            return RedirectToAction("RefreshQuizQuestions", new { id = model.QuizId });
        }

    }
}