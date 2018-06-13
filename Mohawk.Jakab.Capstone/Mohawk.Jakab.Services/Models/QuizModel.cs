using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Interfaces;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuizModel
    {
        public Guid Id { get; set; }
        public string QuizzardUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SkillLevel { get; set; }
        public bool Private { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ArchivedOn { get; set; }

        public IEnumerable<QuestionModel> Questions { get; set; }

        public static Expression<Func<Quiz, QuizModel>> BuildModel(bool includeQuestions)
        {
            return x => new QuizModel()
            {
                Id = x.Id,
                Title = x.Title,
                SkillLevel = x.SkillLevel,
                CreatedOn = x.CreatedOn,
                Private = x.Private,
                ArchivedOn = x.ArchivedOn,
                Description = x.Description,
                QuizzardUserId = x.QuizzardUserId,
                Questions = x.Questions.AsQueryable().Select(QuestionModel.BuildModel)
            };
        }
    }
}