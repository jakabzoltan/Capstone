using System;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuizSubmissionAnswerModel
    {
        public Guid Id { get; set; }
        public Guid SubmissionId { get; set; }
        //one or the other
        public Guid? QuestionId { get; set; }
        public Guid? UserOwnedQuestionId { get; set; }

        public string UserAnswer { get; set; }

    }
}