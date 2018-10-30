using System.Collections.Generic;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Quizzard.Web.Models
{
    public class Play : QuizSubmissionModel
    {
        public List<PartialViewResult> QuestionPartials { get; set; }
    }
}