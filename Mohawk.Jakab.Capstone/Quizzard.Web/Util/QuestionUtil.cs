using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mohawk.Jakab.Quizzard.Domain;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Mohawk.Jakab.Quizzard.Services.Models;
using Quizzard.Web.Areas.Questions;

namespace Quizzard.Web.Util
{
    public static class QuestionUtil
    {
        public static IEnumerable<QuestionTypeModel> ActiveQuestionTypes()
        {
            using (var ctx = QuizzardContext.Create())
            {
                return ctx.QuestionTypes.Select(QuestionTypeModel.Build).ToList();
            }
        }

        //public QuestionBaseController ResolveController(string key)
        //{
        //    using (var scope = AutofacSetup.Container.BeginLifetimeScope())
        //        DependencyResolver.Current.GetService();
        //}

    }
}