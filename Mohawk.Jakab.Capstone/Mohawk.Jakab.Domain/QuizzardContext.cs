using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mohawk.Jakab.Quizzard.Domain.Entities;

namespace Mohawk.Jakab.Quizzard.Domain
{
    public class QuizzardContext : IdentityDbContext<QuizzardUser>
    {
        public QuizzardContext()
            : base("QuizzardLocal", false)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizReaction> QuizReactions { get; set; }
        public DbSet<QuizSubmission> QuizSubmissions { get; set; }
        public DbSet<QuizSubmissionAnswer> QuizSubmissionAnswers { get; set; }
        public DbSet<ReactionType> ReactionTypes { get; set; }
        public DbSet<UserOwnedQuestion> UserOwnedQuestions { get; set; }
        public DbSet<UserOwnedAnswer> UserOwnedAnswers { get; set; }

        public static QuizzardContext Create()
        {
            return new QuizzardContext();
        }

    }
}
