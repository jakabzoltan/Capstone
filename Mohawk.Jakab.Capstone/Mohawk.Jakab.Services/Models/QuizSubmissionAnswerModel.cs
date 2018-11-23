using System;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuizSubmissionAnswerModel
    {
        public string Id { get; set; }
        public string SubmissionId { get; set; }
        public string QuestionId { get; set; }
        public QuestionModel Question { get; set; }
        public string UserAnswer { get; set; }

        public QuizSubmissionAnswerModel()
        {
            
        }
        public QuizSubmissionAnswerModel(string submissionId) : this()
        {
            Id = Guid.NewGuid().ToString();
            SubmissionId = submissionId;
        }

        public QuizSubmissionAnswerModel(string submissionId, QuestionModel model) : this(submissionId)
        {   
            Question = model;
            QuestionId = model.Id;
        }

    }
}