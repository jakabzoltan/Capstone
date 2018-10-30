using System;
using System.Collections;
using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuizSubmission
    {
        public QuizSubmission()
        {
            Answers = new List<QuizSubmissionAnswer>();
        }
        public string Id { get; set; }
        public string QuizzardUserId { get; set; }
        public string QuizId { get; set; }
        public DateTime SubmittedOn { get; set; }


        public virtual Quiz Quiz { get; set; }
        public virtual QuizzardUser QuizzardUser { get; set; }

        public virtual ICollection<QuizSubmissionAnswer> Answers { get; set; }
    }
}