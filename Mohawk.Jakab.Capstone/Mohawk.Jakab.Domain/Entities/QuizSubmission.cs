using System;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuizSubmission
    {
        public Guid Id { get; set; }
        public string QuizzardUserId { get; set; }
        public Guid QuizId { get; set; }
        public DateTime SubmittedOn { get; set; }


        public virtual Quiz Quiz { get; set; }
        public virtual QuizzardUser QuizzardUser { get; set; }
    }
}