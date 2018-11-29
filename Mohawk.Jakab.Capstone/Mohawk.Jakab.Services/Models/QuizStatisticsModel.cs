using System.Collections.Generic;

namespace Mohawk.Jakab.Quizzard.Services.Models
{
    public class QuizStatisticsModel
    {
        public QuizModel Quiz { get; set; }
        public List<QuizResultsModel> QuizResults { get; set; }

    }
}