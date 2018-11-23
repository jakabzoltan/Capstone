using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

        public async Task<IEnumerable<QuizModel>> SearchForQuizzes(bool privateQuizzes = false, params string[] query)
        {
            return await _context.Quizzes
                .AsExpandable()
                .Where(x => (query.Any(q => x.Title.Contains(q)) || query.Any(q => x.Description.Contains(q))) &&
                            x.ArchivedOn == null && x.Private == false || x.Private == privateQuizzes)
                .Select(QuizModel.BuildModel)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizModel>> GetAllQuizzes(bool includePrivateQuizzes = false)
        {
            return await _context.Quizzes
                .Where(x => x.Private == false || x.Private == includePrivateQuizzes && x.ArchivedOn == null && !x.DraftMode)
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
            try
            {
                var questions = await _context.UserOwnedQuestions
                    .Where(x => x.Quizzes.Any(q => q.Id == id))
                    .Select(UserOwnedQuestionModel.BuildModel)
                    .ToListAsync();

                questionSet.QuizQuestions.ToList().AddRange(questions);

                var otherQuestions = await _context.Questions
                    .Where(x => x.QuizId == id)
                    .Select(QuestionModel.BuildModel)
                    .ToListAsync();

                questionSet.QuizQuestions.ToList().AddRange(otherQuestions);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Trace.TraceError(e.Message);
                Debug.Write(e.Message);
            }

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
                        DraftMode = true,
                        CreatedOn = DateTime.UtcNow
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

            return model;
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

                    var answers = model.Answers.Select(x => new QuestionAnswer()
                    {
                        Id = Guid.NewGuid().ToString(),
                        QuestionId = question.Id,
                        AnswerText = x.AnswerText,
                        Correctness = x.Correctness,
                    });

                    quiz.Questions.Add(question);
                    
                    foreach (var questionAnswer in answers)
                    {
                        question.QuestionAnswers.Add(questionAnswer);
                    }
                    
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return null;
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
                        Correctness = model.Correctness,
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

        public async Task<bool> EditQuestion(string questionId, QuestionModel model)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == questionId);

            if (question == null) return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    question.QuestionText = model.QuestionText;
                    question.QuestionTypeId = model.QuestionTypeId;

                    foreach (var answer in question.QuestionAnswers)
                    {
                        question.QuestionAnswers.Remove(answer);
                    }

                    var answers = model.Answers.Select(x => new QuestionAnswer()
                    {
                        QuestionId = question.Id,
                        AnswerText = x.AnswerText,
                        Correctness = x.Correctness,
                    });

                    foreach (var questionAnswer in answers)
                    {
                        question.QuestionAnswers.Add(questionAnswer);
                    }


                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
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
                    answer.Correctness = model.Correctness;
                    
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
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAnswer(string answerId)
        {
            var question = await _context.QuestionAnswers.FirstOrDefaultAsync(x => x.Id == answerId);
            if (question == null) return false;
            _context.QuestionAnswers.Remove(question);
            await _context.SaveChangesAsync();
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

        public async Task<List<QuizModel>> GetMyQuizzes(string userId)
        {
            return await _context.Quizzes
                .Where(x=>x.QuizzardUserId == userId)
                .Select(QuizModel.BuildModel)
                .ToListAsync();
        }

        public bool QuestionExists(string id)
        {
            return _context.Questions.FirstOrDefault(x => x.Id == id) != null;
        }
        public async Task<QuestionModel> GetQuestion(string id)
        {
            return await _context.Questions
                .Where(x => x.Id == id)
                .Select(QuestionModel.BuildModel)
                .FirstOrDefaultAsync();
        }
    }
}