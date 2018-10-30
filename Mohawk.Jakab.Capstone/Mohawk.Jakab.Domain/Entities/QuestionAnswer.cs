using System;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuestionAnswer
    {
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }

        //navigation properties
        public virtual Question Question { get; set; }
    }
}