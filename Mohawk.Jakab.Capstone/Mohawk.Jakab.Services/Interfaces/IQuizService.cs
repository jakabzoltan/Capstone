﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Services.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizModel>> SearchForQuizzes(bool privateQuizzes = false, params string[] query);
        Task<IEnumerable<QuizModel>> GetAllQuizzes(bool includePrivateQuizzes = false);
        Task<IEnumerable<QuizModel>> GetUserOwnedQuizzes(string userId);
        Task<QuizModel> GetQuiz(string id);
        Task<QuestionSet> GetQuizQuestions(string id);
        Task<QuizModel> CreateQuiz(QuizModel model);
        Task<QuizModel> EditQuiz(string quizId, QuizModel model);
        Task<bool> ArchiveQuiz(string quizId);
        Task<bool> AddQuestion(string quizId, QuestionModel model);
        Task<bool> AttachUserQuestionToQuiz(string quizId, string userOwnedQuestionId);
        Task<bool> DetachUserQuestionFromQuiz(string quizId, string userOwnedQuestionId);
        Task<bool> EditQuestion(string questionId, QuestionModel model);
        Task<AnswerModel> EditAnswer(string answerId, AnswerModel model);
        Task<bool> RemoveQuestion(string questionId);
        Task<bool> RemoveAnswer(string answerId);
        Task<List<QuestionTypeModel>> GetQuestionTypes();
        bool ToggleDraftMode(string quizId);
        Task<QuestionModel> GetQuestion(string id);
        Task<List<QuizModel>> GetMyQuizzes(string userId);

        bool QuestionExists(string id);
    }
}