using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var quizSubmissionId = Guid.NewGuid();
                    _context.QuizSubmissions.Add(new QuizSubmission()
                    {
                        Id = quizSubmissionId,
                        QuizzardUserId = userId,
                        SubmittedOn = DateTime.Now,
                        Answers = new List<QuizSubmissionAnswer>(model.Answers.Select(x => new QuizSubmissionAnswer()
                        {
                            Id = Guid.NewGuid(),
                            UserOwnedQuestionId = x.UserOwnedQuestionId,
                            QuestionId = x.QuestionId,
                            SubmissionId = quizSubmissionId,
                            UserAnswer = x.UserAnswer
                        }))


                    });
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }

                await _context.SaveChangesAsync();

            }

        }

        public Task PlayQuizAnonymously(QuizSubmissionModel model)
        {
            throw new NotImplementedException();
        }

        public Task<QuizResultsModel> GetQuizStatistics(Guid quizId)
        {
            throw new NotImplementedException();
        }

        public Task<QuizResultsModel> GetQuizResults(Guid submissionId)
        {
            throw new NotImplementedException();
        }
    }
}