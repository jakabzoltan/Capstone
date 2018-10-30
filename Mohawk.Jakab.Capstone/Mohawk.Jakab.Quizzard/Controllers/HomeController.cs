using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Mohawk.Jakab.Quizzard.Models.Home;
using Mohawk.Jakab.Quizzard.Services.Interfaces;

namespace Mohawk.Jakab.Quizzard.Controllers
{
    public class HomeController : Controller
    {
        private IQuizService _quizService { get; set; }

        public HomeController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public async Task<ActionResult> Index()
        {
            var quizzes = (await _quizService.GetAllQuizzes(User.Identity.IsAuthenticated)).ToList();
            var model = new SearchResults()
            {
                PrivateQuizzes = quizzes.Where(x => x.Private).ToList(),
                PublicQuizzes = quizzes.Where(x => !x.Private).ToList()
            };
            return View(model);
        }

        public async Task<ActionResult> Search(SearchTerm term)
        {
            var quizzes = (await _quizService.SearchForQuizzes(term.Term)).ToList();
            var model = new SearchResults()
            {
                PrivateQuizzes = quizzes.Where(x => x.Private).ToList(),
                PublicQuizzes = quizzes.Where(x => !x.Private).ToList()
            };
            return View("Index", model);
        }
    }
}