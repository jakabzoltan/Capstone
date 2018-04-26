using System;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuizSubmissionAnswer
    {
        public Guid Id { get; set; }
        public Guid SubmissionId { get; set; }
        //one or the other
        public Guid? QuestionId { get; set; }
        public Guid? UserOwnedQuestionId { get; set; }

        public string UserAnswer { get; set; }

        public virtual Question Question { get; set; }
        public virtual UserOwnedQuestion UserOwnedQuestion { get; set; }
        public virtual QuizSubmission QuizSubmission { get; set; }

    }
}