﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Interfaces;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuizModel
    {
        public QuizModel()
        {
        }

        public string Id { get; set; }
        public string QuizzardUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name="Skill Level")]
        public string SkillLevel { get; set; }
        public bool Private { get; set; }
        public bool DraftMode { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ArchivedOn { get; set; }
        public string CreatorName { get; set; }
        public IEnumerable<QuestionModel> Questions { get; set; }

        public static Expression<Func<Quiz, QuizModel>> BuildModel(bool archivedQuestions = true)
        {
            var questionExp = QuestionModel.BuildModel();
            var userQuestionExp = UserOwnedQuestionModel.BuildModel();
            return x => new QuizModel()
            {
                Id = x.Id,
                Title = x.Title,
                SkillLevel = x.SkillLevel,
                CreatedOn = x.CreatedOn,
                Private = x.Private,
                ArchivedOn = x.ArchivedOn,
                Description = x.Description,
                DraftMode = x.DraftMode,
                QuizzardUserId = x.QuizzardUserId,
                CreatorName = x.QuizzardUser.FirstName + " " + x.QuizzardUser.LastName,
                Questions = x.Questions.AsQueryable().Where(z => (archivedQuestions && z.ArchivedOn != null) || z.ArchivedOn == null).Select(z=> questionExp.Invoke(z)),

            };
        }
    }
}