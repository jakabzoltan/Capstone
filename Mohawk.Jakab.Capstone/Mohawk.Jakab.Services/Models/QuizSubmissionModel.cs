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
        public Guid Id { get; set; }
        public string QuizzardUserId { get; set; }
        public Guid QuizId { get; set; }
        public DateTime SubmittedOn { get; set; }
        public ICollection<QuizSubmissionAnswerModel> Answers { get; set; }
    }
}