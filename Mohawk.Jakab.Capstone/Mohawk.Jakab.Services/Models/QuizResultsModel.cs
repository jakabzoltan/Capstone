using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Mohawk.Jakab.Quizzard.Domain.Entities;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuizResultsModel
    {
        public QuizModel Quiz { get; set; }
        public IEnumerable<QuizSubmissionAnswerModel> SubmittedAnswers { get; set; }
        public DateTime SubmittedOn { get; set; }
        public QuizzardUser User { get; set; }

        public double QuizResult()
        {
            double total = 0d;
            foreach (var submittedAnswer in SubmittedAnswers)
            {
                var firstOrDefault = submittedAnswer
                    .GetQuestion()
                    .Answers
                    .FirstOrDefault(
                        x => submittedAnswer.UserAnswer.Equals(x.AnswerText, StringComparison.OrdinalIgnoreCase));
                if (firstOrDefault != null)
                    total += firstOrDefault.Correctness;
            }
            return total / SubmittedAnswers.ToList().Count;
        }


        public static Expression<Func<QuizSubmission, QuizResultsModel>> BuildModel()
        {
            var quizExpression = QuizModel.BuildModel();
            var answerExpression = QuizSubmissionAnswerModel.BuildModel();
            return x => new QuizResultsModel()
            {
                Quiz = quizExpression.Invoke(x.Quiz),
                SubmittedAnswers = x.Answers.AsQueryable().Select(z=>answerExpression.Invoke(z)),
                SubmittedOn = x.SubmittedOn,
                User = x.QuizzardUser
            };
        }

    }
}