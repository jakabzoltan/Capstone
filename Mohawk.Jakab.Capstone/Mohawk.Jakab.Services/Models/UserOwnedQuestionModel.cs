using System;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Mohawk.Jakab.Quizzard.Domain.Entities;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class UserOwnedQuestionModel : QuestionModel
    {
        public UserOwnedQuestionModel()
        {
            
        }

        public UserOwnedQuestionModel(string userId, QuestionModel baseModel)
        {
            Id = baseModel.Id;
            QuizId = baseModel.QuizId;
            QuestionText = baseModel.QuestionText;
            QuestionTypeId = baseModel.QuestionTypeId;
            UserOwned = true;
            UserId = userId;
            ArchivedOn = baseModel.ArchivedOn;
            Answers = baseModel.Answers;
        }

        public string UserId { get; set; }

        public new static Expression<Func<UserOwnedQuestion, UserOwnedQuestionModel>> BuildModel()
        {
            var answerExp = AnswerModel.BuildUserOwnedModel();

            return x => new UserOwnedQuestionModel()
            {
                UserId = x.QuizzardUserId,
                Id = x.Id,
                QuestionText = x.QuestionText,
                Answers = x.UserOwnedAnswers.AsQueryable().Select(z => answerExp.Invoke(z)),
                QuestionTypeId = x.QuestionTypeId,
                UserOwned = true,
                ArchivedOn = x.ArchivedOn
            };
        }


    }
}

