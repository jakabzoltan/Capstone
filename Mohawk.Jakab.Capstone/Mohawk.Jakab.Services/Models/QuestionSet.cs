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

        public Guid QuizId { get; set; }
        public List<QuestionModel> QuizQuestions { get; set; }

    }
}