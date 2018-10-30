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
                .Select(QuizModel.BuildModel)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizModel>> GetAllQuizzes(bool includePrivateQuizzes = false)
        {
            return await _context.Quizzes
                .Where(x => x.Private == false && x.Private == includePrivateQuizzes && x.ArchivedOn == null && !x.DraftMode)
                .Select(QuizModel.BuildModel)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizModel>> GetUserOwnedQuizzes(string userId)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x => x.QuizzardUserId == userId)
                .Select(QuizModel.BuildModel)
                .ToListAsync();
        }

        public async Task<QuizModel> GetQuiz(string id)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x => x.Id == id).Select(QuizModel.BuildModel)
                .FirstOrDefaultAsync();
        }

        public async Task<QuestionSet> GetQuizQuestions(string id)
        {
            var questionSet = new QuestionSet()
            {
                QuizId = id
            };
            questionSet.QuizQuestions.ToList().AddRange(await _context.UserOwnedQuestions
                .Where(x => x.Quizzes.Any(q => q.Id == id))
                .Select(UserOwnedQuestionModel.BuildModel)
                .ToListAsync());
            questionSet.QuizQuestions.ToList().AddRange(await _context.Questions
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
                    var newId = Guid.NewGuid().ToString();
                    _context.Quizzes.Add(new Quiz
                    {
                        Id = newId,
                        Title = model.Title,
                        Description =  model.Description,
                        SkillLevel =  model.SkillLevel,
                        Private = model.Private,
                        DraftMode = true
                    });
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    model.Id = newId;
                    return model;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<QuizModel> EditQuiz(string quizId, QuizModel model)
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

            return QuizModel.BuildModel.Invoke(quiz);
        }

        public async Task<bool> ArchiveQuiz(string quizId)
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

        public async Task<QuestionModel> AddQuestion(string quizId, QuestionModel model)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == quizId);

            if (quiz == null) return null;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var question = new Question()
                    {
                        Id = Guid.NewGuid().ToString(),
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

        public async Task<AnswerModel> AddQuestionAnswer(string questionId, AnswerModel model)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == questionId);

            if (question == null) return null;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var answer = new QuestionAnswer()
                    {
                        Id = Guid.NewGuid().ToString(),
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

        public async Task<bool> AttachUserQuestionToQuiz(string quizId, string userOwnedQuestionId)
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

        public async Task<bool> DetachUserQuestionFromQuiz(string quizId, string userOwnedQuestionId)
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

        public async Task<QuestionModel> EditQuestion(string questionId, QuestionModel model)
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

        public async Task<AnswerModel> EditAnswer(string answerId, AnswerModel model)
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

        public async Task<bool> RemoveQuestion(string questionId)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == questionId);
            if (question == null) return false;
            _context.Questions.Remove(question);
            return true;
        }

        public async Task<bool> RemoveAnswer(string answerId)
        {
            var question = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.Id == answerId);
            if (question == null) return false;
            _context.QuestionAnswers.Remove(question);
            return true;
        }

        public async Task<List<QuestionTypeModel>> GetQuestionTypes()
        {
            return await _context.QuestionTypes.Select(QuestionTypeModel.Build).ToListAsync();
        }

        public bool ToggleDraftMode(string quizId)
        {
            var quiz = _context.Quizzes.FirstOrDefault(x => x.Id == quizId);
            if (quiz == null) return false;
            quiz.DraftMode = !quiz.DraftMode;
            _context.SaveChanges();
            return !quiz.DraftMode;
        }
    }
}