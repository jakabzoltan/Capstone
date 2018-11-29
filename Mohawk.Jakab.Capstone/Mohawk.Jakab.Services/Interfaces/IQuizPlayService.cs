using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Services.Interfaces
{
    public interface IQuizPlayService
    {
        Task PlayQuiz(string userId, QuizSubmissionModel model);
        Task PlayQuizAnonymously(QuizSubmissionModel model);
        QuizStatisticsModel GetQuizStatistics(string quizId);

        QuizResultsModel GetQuizResults(string submissionId);
    }
}