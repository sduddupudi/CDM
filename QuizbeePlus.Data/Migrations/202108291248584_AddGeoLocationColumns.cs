namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGeoLocationColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentQuizs", "IPAddress", c => c.String(nullable: true, maxLength: 128));
            AddColumn("dbo.StudentQuizs", "State", c => c.String(nullable: true, maxLength: 128));
            AddColumn("dbo.StudentQuizs", "City", c => c.String(nullable: true, maxLength: 128));       
        }

        public override void Down()
        {
            DropColumn("dbo.StudentQuizs", "IPAddress");
            DropColumn("dbo.StudentQuizs", "State");
            DropColumn("dbo.StudentQuizs", "City");
        }
    }
}
