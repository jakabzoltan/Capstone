using System;
using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class UserOwnedQuestion
    {
        public string Id { get; set; }
        public string QuizzardUserId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionTypeId { get; set; }

        //navigation properties
        public virtual QuizzardUser QuizzardUser { get; set; }
        public virtual IEnumerable<Quiz> Quizzes { get; set; }
        public virtual IEnumerable<UserOwnedAnswer> UserOwnedAnswers { get; set; }
        public virtual QuestionType QuestionType { get; set; }
    }
}