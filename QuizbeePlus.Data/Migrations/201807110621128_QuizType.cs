namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuizType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quizs", "QuizType", c => c.Int(nullable: false));
            DropColumn("dbo.Questions", "QuestionType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "QuestionType", c => c.Int(nullable: false));
            DropColumn("dbo.Quizs", "QuizType");
        }
    }
}
