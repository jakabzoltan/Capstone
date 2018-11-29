using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Interfaces;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuestionModel 
    {
        public QuestionModel()
        {
            Answers = new List<AnswerModel>();
        }
        public string Id { get; set; }
        public string QuizId { get; set; }

        public string QuestionTypeId { get; set; }
        [MaxLength(1000)]
        [Display(Name="Question Text")]
        public string QuestionText { get; set; }
        public bool UserOwned { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ArchivedOn { get; set; }
        public IEnumerable<AnswerModel> Answers { get; set; }

        public static Expression<Func<Question, QuestionModel>> BuildModel()
        {
            var answerExpression = AnswerModel.BuildModel();
            return x => new QuestionModel()
            {
                Id = x.Id,
                QuizId = x.QuizId,
                QuestionText = x.QuestionText,
                CreatedOn = x.CreatedOn,
                QuestionTypeId = x.QuestionTypeId,
                UserOwned = false,
                ArchivedOn = x.ArchivedOn,
                Answers = x.QuestionAnswers.AsQueryable().Select(z => answerExpression.Invoke(z))
            };
        }
    }
}