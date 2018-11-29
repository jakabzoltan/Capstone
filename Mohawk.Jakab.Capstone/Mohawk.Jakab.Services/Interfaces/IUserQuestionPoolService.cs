using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mohawk.Jakab.Quizzard.Services.Models;

namespace Mohawk.Jakab.Quizzard.Services.Interfaces
{
    public interface IUserQuestionPoolService
    {
        Task<IEnumerable<UserOwnedQuestionModel>> GetUserOwnedQuestions(string userId);
        Task<bool> AddUserOwnedQuestion(UserOwnedQuestionModel model);
        Task<bool> EditUserOwnedQuestion(UserOwnedQuestionModel model);
        Task<UserOwnedQuestionModel> GetUserOwnedQuestion(string id);
        Task<bool> RemoveUserOwnedQuestion(string questionId);
    }
}