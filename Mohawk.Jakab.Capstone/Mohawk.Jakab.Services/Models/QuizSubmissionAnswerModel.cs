using System;
using System.Linq.Expressions;
using LinqKit;
using Mohawk.Jakab.Quizzard.Domain.Entities;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuizSubmissionAnswerModel
    {
        public string Id { get; set; }
        public string SubmissionId { get; set; }
        public string QuestionId { get; set; }
        public bool UserOwned { get; set; }
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

        public QuestionModel GetQuestion()
        {
            return Question;
        }


        public static Expression<Func<QuizSubmissionAnswer, QuizSubmissionAnswerModel>> BuildModel()
        {
            var questionExpression = QuestionModel.BuildModel();
            return x =>
                new QuizSubmissionAnswerModel()
                {
                    Id = x.Id,
                    UserAnswer = x.UserAnswer,
                    Question = questionExpression.Invoke(x.Question),
                    SubmissionId = x.SubmissionId,
                    QuestionId = x.QuestionId ?? x.UserOwnedQuestionId,
                    UserOwned = x.Question != null
                };
        }
    }
}