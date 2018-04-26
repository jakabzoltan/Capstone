using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Mohawk.Jakab.Quizzard.Domain.Entities
{
    public class QuizzardUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string SecurityQuestion { get; set; }
        public string SecurityAnswerHash { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<QuizzardUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual IEnumerable<Quiz> Quizzes { get; set; }
        public virtual IEnumerable<QuizSubmission> QuizSubmissions { get; set; }
        public virtual IEnumerable<QuizReaction> QuizReactions { get; set; }
        public virtual IEnumerable<UserOwnedQuestion> UserOwnedQuestions { get; set; }

    }
}