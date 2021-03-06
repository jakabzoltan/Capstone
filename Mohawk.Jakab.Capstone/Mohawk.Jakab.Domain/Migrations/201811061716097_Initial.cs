namespace Mohawk.Jakab.Quizzard.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionAnswers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuestionId = c.String(maxLength: 128),
                        AnswerText = c.String(),
                        Correctness = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuizId = c.String(maxLength: 128),
                        QuestionTypeId = c.String(maxLength: 128),
                        QuestionText = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionTypes", t => t.QuestionTypeId)
                .ForeignKey("dbo.Quizs", t => t.QuizId)
                .Index(t => t.QuizId)
                .Index(t => t.QuestionTypeId);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuestionTypeText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuizReactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReactionTypeId = c.Int(nullable: false),
                        QuizId = c.String(maxLength: 128),
                        QuizzardUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quizs", t => t.QuizId)
                .ForeignKey("dbo.AspNetUsers", t => t.QuizzardUserId)
                .ForeignKey("dbo.ReactionTypes", t => t.ReactionTypeId, cascadeDelete: true)
                .Index(t => t.ReactionTypeId)
                .Index(t => t.QuizId)
                .Index(t => t.QuizzardUserId);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuizzardUserId = c.String(maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                        SkillLevel = c.String(),
                        Private = c.Boolean(nullable: false),
                        DraftMode = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ArchivedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.QuizzardUserId)
                .Index(t => t.QuizzardUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        SecurityQuestion = c.String(),
                        SecurityAnswerHash = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserOwnedQuestions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuizzardUserId = c.String(maxLength: 128),
                        QuestionText = c.String(),
                        QuestionTypeId = c.String(maxLength: 128),
                        Quiz_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionTypes", t => t.QuestionTypeId)
                .ForeignKey("dbo.AspNetUsers", t => t.QuizzardUserId)
                .ForeignKey("dbo.Quizs", t => t.Quiz_Id)
                .Index(t => t.QuizzardUserId)
                .Index(t => t.QuestionTypeId)
                .Index(t => t.Quiz_Id);
            
            CreateTable(
                "dbo.ReactionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReactionTypeText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuizSubmissionAnswers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SubmissionId = c.String(),
                        QuestionId = c.String(maxLength: 128),
                        UserOwnedQuestionId = c.String(maxLength: 128),
                        UserAnswer = c.String(),
                        QuizSubmission_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .ForeignKey("dbo.QuizSubmissions", t => t.QuizSubmission_Id)
                .ForeignKey("dbo.UserOwnedQuestions", t => t.UserOwnedQuestionId)
                .Index(t => t.QuestionId)
                .Index(t => t.UserOwnedQuestionId)
                .Index(t => t.QuizSubmission_Id);
            
            CreateTable(
                "dbo.QuizSubmissions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuizzardUserId = c.String(maxLength: 128),
                        QuizId = c.String(maxLength: 128),
                        SubmittedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quizs", t => t.QuizId)
                .ForeignKey("dbo.AspNetUsers", t => t.QuizzardUserId)
                .Index(t => t.QuizzardUserId)
                .Index(t => t.QuizId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserOwnedAnswers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserOwnedQuestionId = c.String(maxLength: 128),
                        AnswerText = c.String(),
                        Correctness = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserOwnedQuestions", t => t.UserOwnedQuestionId)
                .Index(t => t.UserOwnedQuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserOwnedAnswers", "UserOwnedQuestionId", "dbo.UserOwnedQuestions");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.QuizSubmissionAnswers", "UserOwnedQuestionId", "dbo.UserOwnedQuestions");
            DropForeignKey("dbo.QuizSubmissions", "QuizzardUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuizSubmissions", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.QuizSubmissionAnswers", "QuizSubmission_Id", "dbo.QuizSubmissions");
            DropForeignKey("dbo.QuizSubmissionAnswers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuizReactions", "ReactionTypeId", "dbo.ReactionTypes");
            DropForeignKey("dbo.QuizReactions", "QuizzardUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserOwnedQuestions", "Quiz_Id", "dbo.Quizs");
            DropForeignKey("dbo.UserOwnedQuestions", "QuizzardUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserOwnedQuestions", "QuestionTypeId", "dbo.QuestionTypes");
            DropForeignKey("dbo.Quizs", "QuizzardUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuizReactions", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Questions", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Questions", "QuestionTypeId", "dbo.QuestionTypes");
            DropForeignKey("dbo.QuestionAnswers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.UserOwnedAnswers", new[] { "UserOwnedQuestionId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.QuizSubmissions", new[] { "QuizId" });
            DropIndex("dbo.QuizSubmissions", new[] { "QuizzardUserId" });
            DropIndex("dbo.QuizSubmissionAnswers", new[] { "QuizSubmission_Id" });
            DropIndex("dbo.QuizSubmissionAnswers", new[] { "UserOwnedQuestionId" });
            DropIndex("dbo.QuizSubmissionAnswers", new[] { "QuestionId" });
            DropIndex("dbo.UserOwnedQuestions", new[] { "Quiz_Id" });
            DropIndex("dbo.UserOwnedQuestions", new[] { "QuestionTypeId" });
            DropIndex("dbo.UserOwnedQuestions", new[] { "QuizzardUserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Quizs", new[] { "QuizzardUserId" });
            DropIndex("dbo.QuizReactions", new[] { "QuizzardUserId" });
            DropIndex("dbo.QuizReactions", new[] { "QuizId" });
            DropIndex("dbo.QuizReactions", new[] { "ReactionTypeId" });
            DropIndex("dbo.Questions", new[] { "QuestionTypeId" });
            DropIndex("dbo.Questions", new[] { "QuizId" });
            DropIndex("dbo.QuestionAnswers", new[] { "QuestionId" });
            DropTable("dbo.UserOwnedAnswers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.QuizSubmissions");
            DropTable("dbo.QuizSubmissionAnswers");
            DropTable("dbo.ReactionTypes");
            DropTable("dbo.UserOwnedQuestions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Quizs");
            DropTable("dbo.QuizReactions");
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.Questions");
            DropTable("dbo.QuestionAnswers");
        }
    }
}
