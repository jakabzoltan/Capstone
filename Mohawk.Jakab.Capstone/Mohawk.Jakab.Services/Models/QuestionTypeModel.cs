using System;
using System.Linq.Expressions;
using Mohawk.Jakab.Quizzard.Domain.Entities;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuestionTypeModel : QuestionType
    {
        public string Id { get; set; }
        public string QuestionTypeText { get; set; }

        public static Expression<Func<QuestionType, QuestionTypeModel>> Build => x => new QuestionTypeModel()
        {
            Id = x.Id,
            QuestionTypeText = x.QuestionTypeText
        };
    }
}