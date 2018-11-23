using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Interfaces;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class AnswerModel
    {
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string AnswerText { get; set; }
        [Range(0,100)]
        public int Correctness { get; set; }
        public bool UserOwned { get; set; }

        public static Expression<Func<QuestionAnswer, AnswerModel>> BuildModel => x => new AnswerModel
        {
            Id = x.Id,
            AnswerText = x.AnswerText,
            Correctness = x.Correctness,
            QuestionId = x.QuestionId,
            UserOwned = false
        };
        public static Expression<Func<UserOwnedAnswer, AnswerModel>> BuildUserOwnedModel => x => new AnswerModel
        {
            Id = x.Id,
            AnswerText = x.AnswerText,
            Correctness = x.Correctness,
            QuestionId = x.UserOwnedQuestionId,
            UserOwned = true
        };
    }
}
