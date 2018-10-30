using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Services.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizModel>> SearchForQuizzes(params string[] query);
        Task<IEnumerable<QuizModel>> GetAllQuizzes(bool includePrivateQuizzes = false);
        Task<IEnumerable<QuizModel>> GetUserOwnedQuizzes(string userId);
        Task<QuizModel> GetQuiz(string id);
        Task<QuestionSet> GetQuizQuestions(string id);
        Task<QuizModel> CreateQuiz(QuizModel model);
        Task<QuizModel> EditQuiz(string quizId, QuizModel model);
        Task<bool> ArchiveQuiz(string quizId);
        Task<QuestionModel> AddQuestion(string quizId, QuestionModel model);
        Task<AnswerModel> AddQuestionAnswer(string questionId, AnswerModel model);
        Task<bool> AttachUserQuestionToQuiz(string quizId, string userOwnedQuestionId);
        Task<bool> DetachUserQuestionFromQuiz(string quizId, string userOwnedQuestionId);
        Task<QuestionModel> EditQuestion(string questionId, QuestionModel model);
        Task<AnswerModel> EditAnswer(string answerId, AnswerModel model);
        Task<bool> RemoveQuestion(string questionId);
        Task<bool> RemoveAnswer(string answerId);
        Task<List<QuestionTypeModel>> GetQuestionTypes();
        bool ToggleDraftMode(string quizId);
    }
}