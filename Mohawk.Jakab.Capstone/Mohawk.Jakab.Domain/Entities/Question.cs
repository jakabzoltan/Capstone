using System;
using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class Question
    {
        public string Id { get; set; }
        public string QuizId { get; set; }
        
        public string QuestionTypeId { get; set; }
        public string QuestionText { get; set; }
        //navigation properties
        public virtual QuestionType QuestionType { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();
        
    }
}