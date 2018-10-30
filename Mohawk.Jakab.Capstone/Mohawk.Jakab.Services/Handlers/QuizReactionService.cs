using System;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Domain;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Interfaces;

namespace Mohawk.Jakab.Quizzard.Services.Handlers
{
    public class QuizReactionService : IQuizReactionService
    {
        private readonly QuizzardContext _context;

        public QuizReactionService()
        {
            _context = QuizzardContext.Create();
        }

        public async Task<bool> LikeOrUnlikeQuiz(string quizId, string userId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.QuizReactions.Add(new QuizReaction(){QuizId = quizId, QuizzardUserId = userId});
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    return false;
                }
            }
           
        }
    }
}