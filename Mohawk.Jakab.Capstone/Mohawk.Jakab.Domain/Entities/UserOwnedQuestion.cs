using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class UserOwnedQuestion
    {
        public string Id { get; set; }
        public string QuizzardUserId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionTypeId { get; set; }
        public DateTime? ArchivedOn { get; set; }

        //navigation properties
        public virtual QuizzardUser QuizzardUser { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<UserOwnedAnswer> UserOwnedAnswers { get; set; }
        public virtual QuestionType QuestionType { get; set; }
    }
}