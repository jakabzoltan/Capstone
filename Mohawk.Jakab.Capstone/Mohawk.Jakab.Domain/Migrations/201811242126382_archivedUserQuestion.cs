namespace Mohawk.Jakab.Quizzard.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class archivedUserQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserOwnedQuestions", "ArchivedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserOwnedQuestions", "ArchivedOn");
        }
    }
}
