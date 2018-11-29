using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Microsoft.AspNet.Identity;
using Mohawk.Jakab.Quizzard.Services.Interfaces;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.Areas.Questions;
using Quizzard.Web.Models;

namespace Quizzard.Web.Controllers
{
    [Authorize]
    public class QuestionPoolController : Controller
    {
        private IUserQuestionPoolService UserQuestionPoolService { get; set; }
        public QuestionPoolController()
        {
            
        }
        public QuestionPoolController(IUserQuestionPoolService userQuestionPoolService)
        {
            UserQuestionPoolService = userQuestionPoolService;
        }
        // GET: QuestionPool
        public async Task<ActionResult> Index()
        {
            var id = User.Identity.GetUserId();
            var userQuestions = await UserQuestionPoolService.GetUserOwnedQuestions(id);

            return View(userQuestions.ToList());
        }

        public async Task<ActionResult> AddQuestion(string questionType)
        {
            using (var scope = AutofacSetup.Container.BeginLifetimeScope())
            {
                var controller = scope.ResolveKeyed<QuestionBaseController>(questionType);
                var res = await controller.EditUserOwnedQuestion(Guid.NewGuid().ToString());

                var viewLocations = res.ViewEngineCollection.FindPartialView(controller.ControllerContext, res.ViewName);
                res.View = viewLocations.View;

                return PartialView("_ViewWrapper", new ViewWrapper((RazorView)res.View, res.Model));
            }
        }


        public ActionResult QuestionTypeSelection()
        {
            return PartialView("_QuestionTypeSelectionModal");
        }

        public async Task<ActionResult> GetUserQuestions()
        {
            var id = User.Identity.GetUserId();
            var userQuestions = await UserQuestionPoolService.GetUserOwnedQuestions(id);

            return PartialView("_UserQuestions", userQuestions.ToList());
        }

        public async Task<ActionResult> RemoveQuestion(UserOwnedQuestionModel model)
        {
            await UserQuestionPoolService.RemoveUserOwnedQuestion(model.Id);

            
            var id = User.Identity.GetUserId();
            var userQuestions = await UserQuestionPoolService.GetUserOwnedQuestions(id);
            return PartialView("_UserQuestions", userQuestions.ToList());
        }

        public async Task<ActionResult> EditQuestion(string questionId)
        {
            var question = await UserQuestionPoolService.GetUserOwnedQuestion(questionId);

            using (var scope = AutofacSetup.Container.BeginLifetimeScope())
            {
                var controller = scope.ResolveKeyed<QuestionBaseController>(question.QuestionTypeId);
                var res = await controller.EditUserOwnedQuestion(questionId);

                var viewLocations = res.ViewEngineCollection.FindPartialView(controller.ControllerContext, res.ViewName);
                res.View = viewLocations.View;

                return PartialView("_ViewWrapper", new ViewWrapper((RazorView)res.View, res.Model));
            }



        }


    }
}