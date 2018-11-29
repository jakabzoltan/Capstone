namespace Mohawk.Jakab.Quizzard.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class archivedOn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "ArchivedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "ArchivedOn");
        }
    }
}
