namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstInitMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AttemptedQuestions", "IsActive");
            DropColumn("dbo.Options", "IsActive");
            DropColumn("dbo.Quizs", "IsActive");
            DropColumn("dbo.StudentQuizs", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentQuizs", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Quizs", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Options", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AttemptedQuestions", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
