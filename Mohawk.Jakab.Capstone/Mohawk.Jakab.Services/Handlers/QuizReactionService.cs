using System;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Services.Interfaces;

namespace Mohawk.Jakab.Quizzard.Services.Handlers
{
    public class QuizReactionService : IQuizReactionService
    {
        public Task<bool> LikeOrUnlikeQuiz(Guid quizId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}