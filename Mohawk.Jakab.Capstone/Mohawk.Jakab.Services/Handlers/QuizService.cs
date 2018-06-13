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
        private readonly QuizzardContext _context;

        public QuizService()
        {
            _context = QuizzardContext.Create();
        }

        public async Task<IEnumerable<QuizModel>> SearchForQuizzes(params string[] query)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x => (query.Any(q => x.Title.Contains(q)) || query.Any(q => x.Description.Contains(q))) &&
                            x.ArchivedOn == null)
                .Select(q => QuizModel.BuildModel(false).Invoke(q))
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizModel>> GetAllQuizzes(bool includePrivateQuizzes = false)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x => x.Private == false && x.Private == includePrivateQuizzes && x.ArchivedOn == null)
                .Select(q => QuizModel.BuildModel(false).Invoke(q))
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizModel>> GetUserOwnedQuizzes(string userId)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x => x.QuizzardUserId == userId)
                .Select(q => QuizModel.BuildModel(false).Invoke(q))
                .ToListAsync();
        }

        public async Task<QuizModel> GetQuiz(Guid id)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x => x.Id == id).Select(q => QuizModel.BuildModel(false).Invoke(q))
                .FirstOrDefaultAsync();
        }

        public async Task<QuestionSet> GetQuizQuestions(Guid id)
        {
            var questionSet = new QuestionSet()
            {
                QuizId = id
            };
            questionSet.QuizQuestions.AddRange(await _context.UserOwnedQuestions
                .Where(x => x.Quizzes.Any(q => q.Id == id))
                .Select(UserOwnedQuestionModel.BuildModel)
                .ToListAsync());
            questionSet.QuizQuestions.AddRange(await _context.Questions
                .Where(x => x.QuizId == id)
                .Select(QuestionModel.BuildModel)
                .ToListAsync());
            return questionSet;
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

        public async Task<QuizModel> EditQuiz(Guid quizId, QuizModel model)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);

            if (quiz == null) return null;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    quiz.Title = model.Title;
                    quiz.Description = model.Description;
                    quiz.Private = model.Private;
                    quiz.SkillLevel = model.SkillLevel;

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return QuizModel.BuildModel(true).Invoke(quiz);
        }

        public async Task<bool> ArchiveQuiz(Guid quizId)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);

            if (quiz == null) return false;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    quiz.ArchivedOn = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<QuestionModel> AddQuestion(Guid quizId, QuestionModel model)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);

            if (quiz == null) return null;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var question = new Question()
                    {
                        Id = Guid.NewGuid(),
                        QuestionTypeId = model.QuestionTypeId,
                        QuestionText = model.QuestionText
                    };


                    quiz.Questions.Add(question);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return QuestionModel.BuildModel.Invoke(question);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<AnswerModel> AddQuestionAnswer(Guid questionId, AnswerModel model)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == questionId);

            if (question == null) return null;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var answer = new QuestionAnswer()
                    {
                        Id = Guid.NewGuid(),
                        AnswerText = model.AnswerText,
                        IsCorrect = model.IsCorrect,
                        QuestionId = model.QuestionId
                    };
                    question.QuestionAnswers.Add(answer);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return AnswerModel.BuildModel.Invoke(answer);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        public async Task<bool> AttachUserQuestionToQuiz(Guid quizId, Guid userOwnedQuestionId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var quiz = await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);
                    if (quiz == null) return false;
                    quiz.UserAttachedQuestions.Add(
                        await _context.UserOwnedQuestions.FirstOrDefaultAsync(q => q.Id == userOwnedQuestionId));

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> DetachUserQuestionFromQuiz(Guid quizId, Guid userOwnedQuestionId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var quiz = await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);
                    if (quiz == null) return false;
                    quiz.UserAttachedQuestions.Remove(
                        await _context.UserOwnedQuestions.FirstOrDefaultAsync(q => q.Id == userOwnedQuestionId));

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<QuestionModel> EditQuestion(Guid questionId, QuestionModel model)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == questionId);

            if (question == null) return null;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    question.QuestionText = model.QuestionText;
                    question.QuestionTypeId = model.QuestionTypeId;

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return QuestionModel.BuildModel.Invoke(question);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        public async Task<AnswerModel> EditAnswer(Guid answerId, AnswerModel model)
        {
            var answer = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.Id == answerId);

            if (answer == null) return null;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    answer.AnswerText = model.AnswerText;
                    answer.IsCorrect = model.IsCorrect;
                    
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return AnswerModel.BuildModel.Invoke(answer);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        public async Task<bool> RemoveQuestion(Guid questionId)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == questionId);
            if (question == null) return false;
            _context.Questions.Remove(question);
            return true;
        }

        public async Task<bool> RemoveAnswer(Guid answerId)
        {
            var question = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.Id == answerId);
            if (question == null) return false;
            _context.QuestionAnswers.Remove(question);
            return true;
        }
    }
}