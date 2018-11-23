using System;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class UserOwnedAnswer
    {
        public string Id { get; set; }
        public string UserOwnedQuestionId { get; set; }
        public string AnswerText { get; set; }
        public int Correctness { get; set; }

        //navigation properties
        public virtual UserOwnedQuestion UserOwnedQuestion { get; set; }
    }
}