using System;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Services.Interfaces;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Services.Handlers
{
    public class QuizPlayService : IQuizPlayService
    {
        private IQuizService _quizService;

        public QuizPlayService(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public Task<QuizResultsModel> PlayQuiz(string userId, QuizSubmissionModel model)
        {
            throw new NotImplementedException();
        }

        public Task<QuizResultsModel> PlayQuizAnonymously(QuizSubmissionModel model)
        {
            throw new NotImplementedException();
        }

        public Task<QuizResultsModel> GetQuizStatistics(Guid quizId)
        {
            throw new NotImplementedException();
        }
    }
}