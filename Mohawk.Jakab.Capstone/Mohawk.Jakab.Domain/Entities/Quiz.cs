using System;
using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public string QuizzardUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SkillLevel { get; set; }
        public bool Private { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ArchivedOn { get; set; }

        //navigation properties
        public virtual QuizzardUser QuizzardUser { get; set; }
        public virtual IEnumerable<Question> Questions { get; set; }
        public virtual IEnumerable<UserOwnedQuestion> UserAttachedQuestions { get; set; }
        public virtual IEnumerable<QuizReaction> QuizReactions { get; set; }
    }
}