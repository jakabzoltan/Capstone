using System;
using System.Collections.Generic;
using Mohawk.Jakab.Quizzard.Domain.Entities;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuizSubmissionModel
    {
        public QuizSubmissionModel()
        {
            Answers = new List<QuizSubmissionAnswerModel>();
        }

        public string Id { get; set; }
        public string QuizzardUserId { get; set; }
        public string QuizId { get; set; }
        public DateTime SubmittedOn { get; set; }
        public QuizModel Quiz { get; set; }
        public List<QuizSubmissionAnswerModel> Answers { get; }
    }
}