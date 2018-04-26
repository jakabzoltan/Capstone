using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using Mohawk.Jakab.Quizzard.Domain;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Interfaces;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Services.Handlers
{
    public class QuizService : IQuizService
    {
        private QuizzardContext _context;

        public QuizService()
        {
            _context = QuizzardContext.Create();
        }

        public async Task<IEnumerable<QuizModel>> SearchForQuizzes(params string[] query)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x => query.Any(q => x.Title.Contains(q)) || query.Any(q => x.Description.Contains(q)))
                .Select(q=>QuizModel.BuildModel(false).Invoke(q))
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizModel>> GetAllQuizzes(bool includePrivateQuizzes = false)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x => x.Private == false && x.Private == includePrivateQuizzes) 
                .Select(q => QuizModel.BuildModel(false).Invoke(q))
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizModel>> GetUserOwnedQuizzes(string userId)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x=>x.QuizzardUserId == userId)
                .Select(q => QuizModel.BuildModel(false).Invoke(q))
                .ToListAsync();
        }

        public async Task<QuizModel> GetQuiz(Guid id)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x=>x.Id == id).Select(q => QuizModel.BuildModel(false).Invoke(q))
                .FirstOrDefaultAsync();
        }

        public async Task<QuizModel> CreateQuiz(QuizModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Quizzes.Add(new Quiz
                    {
                        Id = model.Id
                    });
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return model;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Task<QuizModel> EditQuiz(Guid quizId, QuizModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ArchiveQuiz(Guid quizId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddQuestion(Guid quizId, QuestionModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddQuestionAnswer(Guid quizId, Guid questionId, AnswerModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AttachUserQuestionToQuiz(Guid quizId, Guid userOwnedQuestionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DetachUserQuestionFromQuiz(Guid quizId, Guid userOwnedQuestionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditQuestion(Guid questionId, QuestionModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditAnswer(Guid answerId, AnswerModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveQuestion(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAnswer(Guid answerId)
        {
            throw new NotImplementedException();
        }
    }
}