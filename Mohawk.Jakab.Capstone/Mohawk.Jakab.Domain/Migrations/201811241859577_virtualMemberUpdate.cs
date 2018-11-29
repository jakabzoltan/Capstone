namespace Mohawk.Jakab.Quizzard.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class virtualMemberUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserOwnedQuestions", "Quiz_Id", "dbo.Quizs");
            DropIndex("dbo.UserOwnedQuestions", new[] { "Quiz_Id" });
            CreateTable(
                "dbo.UserOwnedQuestionQuizs",
                c => new
                    {
                        UserOwnedQuestion_Id = c.String(nullable: false, maxLength: 128),
                        Quiz_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserOwnedQuestion_Id, t.Quiz_Id })
                .ForeignKey("dbo.UserOwnedQuestions", t => t.UserOwnedQuestion_Id, cascadeDelete: true)
                .ForeignKey("dbo.Quizs", t => t.Quiz_Id, cascadeDelete: true)
                .Index(t => t.UserOwnedQuestion_Id)
                .Index(t => t.Quiz_Id);
            
            DropColumn("dbo.UserOwnedQuestions", "Quiz_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserOwnedQuestions", "Quiz_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.UserOwnedQuestionQuizs", "Quiz_Id", "dbo.Quizs");
            DropForeignKey("dbo.UserOwnedQuestionQuizs", "UserOwnedQuestion_Id", "dbo.UserOwnedQuestions");
            DropIndex("dbo.UserOwnedQuestionQuizs", new[] { "Quiz_Id" });
            DropIndex("dbo.UserOwnedQuestionQuizs", new[] { "UserOwnedQuestion_Id" });
            DropTable("dbo.UserOwnedQuestionQuizs");
            CreateIndex("dbo.UserOwnedQuestions", "Quiz_Id");
            AddForeignKey("dbo.UserOwnedQuestions", "Quiz_Id", "dbo.Quizs", "Id");
        }
    }
}
