using System;
using System.Linq;
using System.Linq.Expressions;
using Mohawk.Jakab.Quizzard.Domain.Entities;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class UserOwnedQuestionModel : QuestionModel
    {
        public string UserId { get; set; }

        public new static Expression<Func<UserOwnedQuestion, UserOwnedQuestionModel>> BuildModel => x => new UserOwnedQuestionModel()
        {
            UserId = x.QuizzardUserId,
            Id = x.Id,
            QuestionText = x.QuestionText,
            Answers = x.UserOwnedAnswers.AsQueryable().Select(AnswerModel.BuildUserOwnedModel),
            QuestionTypeId = x.QuestionTypeId,
            UserOwned = true
        };
    }
}