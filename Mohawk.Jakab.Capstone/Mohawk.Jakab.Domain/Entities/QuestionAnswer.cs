using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuestionAnswer
    {
        public string Id { get; set; }
        [ForeignKey("Question")]
        public string QuestionId { get; set; }
        public string AnswerText { get; set; }
        public int Correctness { get; set; }
        //navigation properties
        public virtual Question Question { get; set; }
    }
}