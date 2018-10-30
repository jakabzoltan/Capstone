using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        // GET: Quiz
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveQuiz(QuizModel model)
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Play()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Play(dynamic model) //todo change
        {
            return View();
        }


        public ActionResult MyQuizzes()
        {
            return View();
        }

        public ActionResult QuizStats(string id)
        {
            return View();
        }
    }
}