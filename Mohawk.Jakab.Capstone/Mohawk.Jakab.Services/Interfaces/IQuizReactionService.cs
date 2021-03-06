﻿using System;
using System.Threading.Tasks;

namespace Mohawk.Jakab.Quizzard.Services.Interfaces
{
    public interface IQuizReactionService
    {
        Task<bool> LikeOrUnlikeQuiz(string quizId, string userId);
    }
}