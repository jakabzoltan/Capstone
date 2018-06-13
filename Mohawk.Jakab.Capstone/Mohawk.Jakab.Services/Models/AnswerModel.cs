using System;
using System.Linq.Expressions;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Interfaces;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class AnswerModel
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public bool UserOwned { get; set; }

        public static Expression<Func<QuestionAnswer, AnswerModel>> BuildModel => x => new AnswerModel
        {
            Id = x.Id,
            AnswerText = x.AnswerText,
            IsCorrect = x.IsCorrect,
            QuestionId = x.QuestionId,
            UserOwned = false
        };
        public static Expression<Func<UserOwnedAnswer, AnswerModel>> BuildUserOwnedModel => x => new AnswerModel
        {
            Id = x.Id,
            AnswerText = x.AnswerText,
            IsCorrect = x.IsCorrect,
            QuestionId = x.UserOwnedQuestionId,
            UserOwned = true
        };
    }
}
