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
    public class QuizPlayService : IQuizPlayService
    {
        private IQuizService _quizService;
        private readonly QuizzardContext _context;

        public QuizPlayService(IQuizService quizService)
        {
            _quizService = quizService;
            _context = QuizzardContext.Create();
        }

        public async Task PlayQuiz(string userId, QuizSubmissionModel model)
        {
            var quiz = _context.Quizzes.FirstOrDefault(x => x.Id == model.QuizId);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var quizSubmissionId = model.Id ?? Guid.NewGuid().ToString();
                    var entity = new QuizSubmission()
                    {
                        Id = quizSubmissionId,
                        QuizzardUserId = userId,
                        QuizId = model.QuizId,
                        SubmittedOn = DateTime.Now,
                        Answers = new List<QuizSubmissionAnswer>()
                    };

                    //entity.Answers = model.Answers.Select(x => new QuizSubmissionAnswer()
                    //{
                    //    Id = Guid.NewGuid().ToString(),
                    //    UserOwnedQuestionId = quiz.UserAttachedQuestions.FirstOrDefault(z => z.Id == x.QuestionId).Id,
                    //    QuestionId = quiz.Questions.FirstOrDefault(z => z.Id == x.QuestionId).Id,
                    //    SubmissionId = quizSubmissionId,
                    //    UserAnswer = x.UserAnswer
                    //}).ToList();


                    foreach (var x in model.Answers)
                    {
                        var userQuestionId = quiz.UserAttachedQuestions.FirstOrDefault(z => z.Id == x.QuestionId)?.Id;
                        var questionId = quiz.Questions.FirstOrDefault(z => z.Id == x.QuestionId)?.Id;

                        var userAnswer = new QuizSubmissionAnswer()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserOwnedQuestionId = userQuestionId,
                            QuestionId = questionId,
                            SubmissionId = quizSubmissionId,
                            UserAnswer = x.UserAnswer
                        };

                        entity.Answers.Add(userAnswer);
                    }



                    _context.QuizSubmissions.Add(entity);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }


            }

        }

        public async Task PlayQuizAnonymously(QuizSubmissionModel model)
        {
            var quiz = _context.Quizzes.FirstOrDefault(x => x.Id == model.QuizId);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var quizSubmissionId = model.Id ?? Guid.NewGuid().ToString();
                    var entity = new QuizSubmission()
                    {
                        Id = quizSubmissionId,
                        QuizId = model.QuizId,
                        SubmittedOn = DateTime.Now,
                        Answers = new List<QuizSubmissionAnswer>()
                    };

                    //entity.Answers = model.Answers.Select(x => new QuizSubmissionAnswer()
                    //{
                    //    Id = Guid.NewGuid().ToString(),
                    //    UserOwnedQuestionId = quiz.UserAttachedQuestions.FirstOrDefault(z => z.Id == x.QuestionId).Id,
                    //    QuestionId = quiz.Questions.FirstOrDefault(z => z.Id == x.QuestionId).Id,
                    //    SubmissionId = quizSubmissionId,
                    //    UserAnswer = x.UserAnswer
                    //}).ToList();


                    foreach (var x in model.Answers)
                    {
                        var userQuestionId = quiz.UserAttachedQuestions.FirstOrDefault(z => z.Id == x.QuestionId)?.Id;
                        var questionId = quiz.Questions.FirstOrDefault(z => z.Id == x.QuestionId)?.Id;

                        var userAnswer = new QuizSubmissionAnswer()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserOwnedQuestionId = userQuestionId,
                            QuestionId = questionId,
                            SubmissionId = quizSubmissionId,
                            UserAnswer = x.UserAnswer
                        };

                        entity.Answers.Add(userAnswer);
                    }



                    _context.QuizSubmissions.Add(entity);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }

            }
        }

        public QuizStatisticsModel GetQuizStatistics(string quizId)
        {
            var model = new QuizStatisticsModel();
            var quizExp = QuizModel.BuildModel();
            var expression = QuizResultsModel.BuildModel();
            model.Quiz = _context.Quizzes.AsExpandable().Where(z => z.Id == quizId).Select(x=> quizExp.Invoke(x)).FirstOrDefault();

            model.QuizResults = _context.QuizSubmissions
                .AsExpandable()
                .Where(x => x.QuizId == quizId && x.QuizzardUserId != null)
                .Select(x=>expression.Invoke(x)).ToList();

            return model;
        }

        public QuizResultsModel GetQuizResults(string submissionId)
        {
            var expression = QuizResultsModel.BuildModel();
            var data = _context.QuizSubmissions.AsExpandable().Where(x => x.Id == submissionId).Select(x=> expression.Invoke(x)).FirstOrDefault();
            return data;
        }
    }
}