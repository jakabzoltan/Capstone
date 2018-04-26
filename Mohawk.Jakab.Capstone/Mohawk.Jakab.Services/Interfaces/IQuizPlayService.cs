using System;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Services.Interfaces
{
    public interface IQuizPlayService
    {
        Task<QuizResultsModel> PlayQuiz(string userId, QuizSubmissionModel model);
        Task<QuizResultsModel> PlayQuizAnonymously(QuizSubmissionModel model);
        Task<QuizResultsModel> GetQuizStatistics(Guid quizId);
    }
}