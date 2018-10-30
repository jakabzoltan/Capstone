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
            var quizzes = (await _quizService.GetAllQuizzes(User.Identity.IsAuthenticated)).ToList();
            var model = new SearchResults()
            {
                PrivateQuizzes = quizzes.Where(x => x.Private).ToList(), //will be empty if the user is not logged in
                PublicQuizzes = quizzes.Where(x => !x.Private).ToList()
            };
            return View(model);
        }

        public async Task<ActionResult> Search(SearchTerm term)
        {
            ViewData["SearchTerm"] = term.Term;
            var quizzes = (await _quizService.SearchForQuizzes(term.Term)).ToList();
            var model = new SearchResults()
            {
                PrivateQuizzes = quizzes.Where(x => x.Private).ToList(), //will be empty if the user is not logged in
                PublicQuizzes = quizzes.Where(x => !x.Private).ToList()
            };
            return View("Index", model);
        }
    }
}