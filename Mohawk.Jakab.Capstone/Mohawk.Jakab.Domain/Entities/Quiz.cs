using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class Quiz
    {
        public string Id { get; set; }
        public string QuizzardUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SkillLevel { get; set; }
        public bool Private { get; set; }
        public bool DraftMode { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ArchivedOn { get; set; }

        //navigation properties
        public virtual QuizzardUser QuizzardUser { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<UserOwnedQuestion> UserAttachedQuestions { get; set; } = new List<UserOwnedQuestion>();
        public virtual ICollection<QuizReaction> QuizReactions { get; set; } = new List<QuizReaction>();

    }
}