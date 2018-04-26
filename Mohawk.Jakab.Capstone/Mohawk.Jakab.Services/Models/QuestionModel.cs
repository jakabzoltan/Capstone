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
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }

        public int QuestionTypeId { get; set; }
        public string QuestionText { get; set; }


        public IEnumerable<AnswerModel> Answers { get; set; }
        public static Expression<Func<Question,QuestionModel>> BuildModel()
        {
            return x => new QuestionModel()
            {
                Id = x.Id,
                QuizId = x.QuizId,
                QuestionText = x.QuestionText,
                QuestionTypeId = x.QuestionTypeId,
                Answers = x.QuestionAnswers.Select(AnswerModel.BuildModel().Invoke)
            };
        }
    }
}