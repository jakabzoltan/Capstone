using System;
using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuestionSet
    {
        public QuestionSet()
        {
            QuizQuestions = new List<QuestionModel>();
        }

        public string QuizId { get; set; }
        public IEnumerable<QuestionModel> QuizQuestions { get; set; }
    }
}