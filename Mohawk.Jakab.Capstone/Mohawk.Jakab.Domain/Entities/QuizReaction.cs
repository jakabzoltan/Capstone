using System;
using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuizReaction
    {
        public int Id { get; set; }
        public int ReactionTypeId { get; set; }
        public Guid QuizId { get; set; }
        public string QuizzardUserId { get; set; }

        //navigation properties
        public virtual ReactionType ReactionType { get; set; }
        public virtual QuizzardUser QuizzardUser { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}