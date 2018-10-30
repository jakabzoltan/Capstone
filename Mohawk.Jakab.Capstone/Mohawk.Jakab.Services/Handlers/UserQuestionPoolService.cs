using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Domain;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Interfaces;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Services.Handlers
{
    public class UserQuestionPoolService : IUserQuestionPoolService
    {
        private readonly QuizzardContext _context;

        public UserQuestionPoolService()
        {
            _context = QuizzardContext.Create();
        }

        public async Task<IEnumerable<UserOwnedQuestionModel>> GetUserOwnedQuestions(string userId)
        {
            return await _context.UserOwnedQuestions
                .Where(x => x.QuizzardUserId == userId)
                .Select(UserOwnedQuestionModel.BuildModel)
                .ToListAsync();
        }

        public async Task<UserOwnedQuestionModel> AddUserOwnedQuestion(UserOwnedQuestionModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var id = Guid.NewGuid().ToString();
                    _context.UserOwnedQuestions.Add(new UserOwnedQuestion()
                    {
                        Id = id,
                        QuizzardUserId = model.UserId,
                        QuestionText = model.QuestionText,
                        QuestionTypeId = model.QuestionTypeId,
                        UserOwnedAnswers = new List<UserOwnedAnswer>(model.Answers.Select(x => new UserOwnedAnswer()
                        {
                            Id = Guid.NewGuid().ToString(),
                            AnswerText = x.AnswerText,
                            IsCorrect = x.IsCorrect,
                            UserOwnedQuestionId = id
                        }))
                    });
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return await _context.UserOwnedQuestions.Where(x => x.Id == id)
                        .Select(UserOwnedQuestionModel.BuildModel)
                        .FirstOrDefaultAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<UserOwnedQuestionModel> EditUserOwnedQuestion(UserOwnedQuestionModel model)
        {
            var toChange = await _context.UserOwnedQuestions.Where(x => x.Id == model.Id)
                .Select(UserOwnedQuestionModel.BuildModel)
                .FirstOrDefaultAsync();

            if (toChange != null)
            {
                toChange.QuestionText = model.QuestionText;
            }
            await _context.SaveChangesAsync();

            var answers = _context.UserOwnedAnswers.Where(x => x.UserOwnedQuestionId == model.Id);
            await model.Answers.AsQueryable().ForEachAsync(answer =>
            {
                var toUpdate = answers.FirstOrDefault(x => x.Id == answer.Id);
                if (toUpdate == null) return;
                toUpdate.AnswerText = answer.AnswerText;
                toUpdate.IsCorrect = answer.IsCorrect;
            });

            await _context.SaveChangesAsync();

            return await _context.UserOwnedQuestions.Where(x => x.Id == model.Id)
                .Select(UserOwnedQuestionModel.BuildModel)
                .FirstOrDefaultAsync();
        }

        public async Task<UserOwnedQuestionModel> GetUserOwnedQuestion(string id)
        {
           return await _context.UserOwnedQuestions.Where(x => x.Id == id)
                .Select(UserOwnedQuestionModel.BuildModel)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveUserOwnedQuestion(string id)
        {
            var question = await _context.UserOwnedQuestions.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (question != null) _context.UserOwnedQuestions.Remove(question);
            return true;
        }
    }
}