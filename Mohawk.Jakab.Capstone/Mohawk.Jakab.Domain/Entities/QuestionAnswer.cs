using System;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuestionAnswer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }

        //navigation properties
        public virtual Question Question { get; set; }
    }
}