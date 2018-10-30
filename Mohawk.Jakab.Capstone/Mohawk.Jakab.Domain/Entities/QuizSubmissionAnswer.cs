using System;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuizSubmissionAnswer
    {
        public string Id { get; set; }
        public string SubmissionId { get; set; }
        //one or the other
        public string QuestionId { get; set; }
        public string UserOwnedQuestionId { get; set; }

        public string UserAnswer { get; set; }

        public virtual Question Question { get; set; }
        public virtual UserOwnedQuestion UserOwnedQuestion { get; set; }
        public virtual QuizSubmission QuizSubmission { get; set; }

    }
}