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
    public class UserQuestionPoolService : IUserQuestionPoolService
    {
        private readonly QuizzardContext _context;

        public UserQuestionPoolService()
        {
            _context = QuizzardContext.Create();
        }

        public async Task<IEnumerable<UserOwnedQuestionModel>> GetUserOwnedQuestions(string userId)
        {
            var exp = UserOwnedQuestionModel.BuildModel();

            return await _context.UserOwnedQuestions.AsExpandable()
                .Where(x => x.QuizzardUserId == userId && x.ArchivedOn == null)
                .Select(x=> exp.Invoke(x))
                .ToListAsync();
        }

        public async Task<bool> AddUserOwnedQuestion(UserOwnedQuestionModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var id = model.Id ?? Guid.NewGuid().ToString();
                    _context.UserOwnedQuestions.Add(new UserOwnedQuestion()
                    {
                        Id = id,
                        QuizzardUserId = model.UserId,
                        QuestionText = model.QuestionText,
                        QuestionTypeId = model.QuestionTypeId,
                        UserOwnedAnswers = new List<UserOwnedAnswer>(model.Answers.Select(x => new UserOwnedAnswer()
                        {
                            Id = x.Id ?? Guid.NewGuid().ToString(),
                            AnswerText = x.AnswerText,
                            Correctness = x.Correctness,
                            UserOwnedQuestionId = id
                        }))
                    });
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> EditUserOwnedQuestion(UserOwnedQuestionModel model)
        {
            var exp = UserOwnedQuestionModel.BuildModel();
            try
            {
                var toChange = await _context.UserOwnedQuestions.Where(x => x.Id == model.Id)
                    .FirstOrDefaultAsync();

                if (toChange != null)
                {
                    toChange.QuestionText = model.QuestionText;
                    toChange.UserOwnedAnswers.Clear();
                    foreach (var answer in model.Answers)
                    {
                        toChange.UserOwnedAnswers.Add(new UserOwnedAnswer()
                        {
                            AnswerText = answer.AnswerText,
                            Correctness = answer.Correctness,
                            Id = answer.Id ?? Guid.NewGuid().ToString(),
                            UserOwnedQuestionId = model.Id
                        });
                    }
                }

                await _context.SaveChangesAsync();
 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            

            return true;
        }

        public async Task<UserOwnedQuestionModel> GetUserOwnedQuestion(string id)
        {
            var exp = UserOwnedQuestionModel.BuildModel();
            return await _context.UserOwnedQuestions.AsExpandable().Where(x => x.Id == id)
                .Select(z=> exp.Invoke(z))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveUserOwnedQuestion(string id)
        {
            var question = await _context.UserOwnedQuestions.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (question != null) question.ArchivedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}