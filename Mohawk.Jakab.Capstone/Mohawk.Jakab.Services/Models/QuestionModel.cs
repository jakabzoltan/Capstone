using System;
using System.Collections.Generic;
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
        public string QuestionText { get; set; }
        public string Description { get; set; }
        public bool UserOwned { get; set; }

        public IEnumerable<AnswerModel> Answers { get; set; }

        public static Expression<Func<Question, QuestionModel>> BuildModel => x => new QuestionModel()
        {
            Id = x.Id,
            QuizId = x.QuizId,
            QuestionText = x.QuestionText,
            QuestionTypeId = x.QuestionTypeId,
            UserOwned = false,
            Answers = x.QuestionAnswers.AsQueryable().Select(AnswerModel.BuildModel)
        };

    }
}