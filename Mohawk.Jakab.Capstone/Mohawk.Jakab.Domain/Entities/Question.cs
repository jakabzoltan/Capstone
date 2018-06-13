using System;
using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        
        public int QuestionTypeId { get; set; }
        public string QuestionText { get; set; }

        //navigation properties
        public virtual QuestionType QuestionType { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        
    }
}