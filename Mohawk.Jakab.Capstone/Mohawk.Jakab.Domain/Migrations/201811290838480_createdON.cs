namespace Mohawk.Jakab.Quizzard.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdON : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "CreatedOn");
        }
    }
}
