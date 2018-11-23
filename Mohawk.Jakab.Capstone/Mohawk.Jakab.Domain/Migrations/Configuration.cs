using Microsoft.AspNet.Identity;
using Mohawk.Jakab.Quizzard.Domain.Entities;
using Constants = Mohawk.Jakab.Quizzard.Domain.Constants;

namespace Mohawk.Jakab.Quizzard.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Mohawk.Jakab.Quizzard.Domain.QuizzardContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Mohawk.Jakab.Quizzard.Domain.QuizzardContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            context.ReactionTypes.AddOrUpdate(new ReactionType()
            {
                Id = 1,
                ReactionTypeText = "Like"
            });
            context.ReactionTypes.AddOrUpdate(new ReactionType()
            {
                Id = 2,
                ReactionTypeText = "Dislike"
            });

            context.QuestionTypes.AddOrUpdate(new QuestionType()
            {
                Id = Constants.QuestionTypes.FreeText,
                QuestionTypeText = Constants.QuestionTypes.FreeText,
            });
            context.QuestionTypes.AddOrUpdate(new QuestionType()
            {
                Id = Constants.QuestionTypes.MultipleChoice,
                QuestionTypeText = Constants.QuestionTypes.MultipleChoice,
            });


        }



    }
}
