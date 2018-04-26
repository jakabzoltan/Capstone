using Mohawk.Jakab.Quizzard.Domain.Entities;

namespace Mohawk.Jakab.Quizzard.Web.Models
{
    public class SecurityQuestionViewModel
    {
        public QuizzardUser User { get; set; }
        public string SecurityAnswer { get; set; }
    }
}