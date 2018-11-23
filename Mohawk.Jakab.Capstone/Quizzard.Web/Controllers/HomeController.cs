using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Interfaces;
using Quizzard.Web.Models.Home;

namespace Quizzard.Web.Controllers
{
    public class HomeController : Controller
    {
        private IQuizService _quizService { get; set; }

        public HomeController()
        {

        }
        public HomeController(IQuizService quizService) : this()
        {
            _quizService = quizService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new SearchResults()
            {
                Quizzes = (await _quizService.GetAllQuizzes(User.Identity.IsAuthenticated)).ToList()
            };
            return View(model);
        }

        public async Task<ActionResult> Search(SearchTerm term)
        {
            ViewData["SearchTerm"] = term.Term;
            var model = new SearchResults()
            {
                Quizzes = (await _quizService.SearchForQuizzes(User.Identity.IsAuthenticated, term.Term)).ToList()
            };
            return View("Index", model);
        }
    }
}