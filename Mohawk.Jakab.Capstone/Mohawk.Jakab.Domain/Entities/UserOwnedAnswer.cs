using System;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class UserOwnedAnswer
    {
        public Guid Id { get; set; }
        public Guid UserOwnedQuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }

        //navigation properties
        public virtual UserOwnedQuestion UserOwnedQuestion { get; set; }
    }
}