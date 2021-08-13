namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuizIsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quizs", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "RegisteredOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RegisteredOn");
            DropColumn("dbo.Quizs", "IsActive");
        }
    }
}
