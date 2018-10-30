using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuestionType
    {
        public string Id { get; set; }
        public string QuestionTypeText { get; set; }
        //navigation
        public virtual IEnumerable<Question> Questions { get; set; }
    }
}