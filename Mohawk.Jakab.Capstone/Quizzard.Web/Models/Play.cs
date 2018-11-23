using System.Collections.Generic;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Quizzard.Web.Models
{
    public class Play : QuizSubmissionModel
    {
        public List<ViewWrapper> QuestionPartials { get; set; } = new List<ViewWrapper>();
        public List<PartialViewResult> PartialViews { get; set; } = new List<PartialViewResult>();
    }
}