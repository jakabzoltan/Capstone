using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class ReactionType
    {
        public int Id { get; set; }
        public string ReactionTypeText { get; set; }

        public virtual IEnumerable<QuizReaction> QuizReactions { get; set; }
    }
}