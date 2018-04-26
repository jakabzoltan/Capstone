using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Services.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizModel>> SearchForQuizzes(params string[] query);
        Task<IEnumerable<QuizModel>> GetAllQuizzes(bool includePrivateQuizzes = false);
        Task<IEnumerable<QuizModel>> GetUserOwnedQuizzes(string userId);
        Task<QuizModel> GetQuiz(Guid id);
        Task<QuizModel> CreateQuiz(QuizModel model);
        Task<QuizModel> EditQuiz(Guid quizId, QuizModel model);
        Task<bool> ArchiveQuiz(Guid quizId);
        Task<bool> AddQuestion(Guid quizId, QuestionModel model);
        Task<bool> AddQuestionAnswer(Guid quizId, Guid questionId, AnswerModel model);
        Task<bool> AttachUserQuestionToQuiz(Guid quizId, Guid userOwnedQuestionId);
        Task<bool> DetachUserQuestionFromQuiz(Guid quizId, Guid userOwnedQuestionId);
        Task<bool> EditQuestion(Guid questionId, QuestionModel model);
        Task<bool> EditAnswer(Guid answerId, AnswerModel model);
        Task<bool> RemoveQuestion(Guid questionId);
        Task<bool> RemoveAnswer(Guid answerId);
    }
}