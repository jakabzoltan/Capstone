using System.Collections.Generic;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Models.Home
{
    public class SearchResults
    {
        public List<QuizModel> PrivateQuizzes { get; set; }
        public List<QuizModel> PublicQuizzes { get; set; }
    }
}