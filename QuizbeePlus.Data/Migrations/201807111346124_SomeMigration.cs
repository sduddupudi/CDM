namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AttemptedQuestions", "SelectedOption_ID", "dbo.Options");
            DropIndex("dbo.AttemptedQuestions", new[] { "SelectedOption_ID" });
            AddColumn("dbo.Options", "AttemptedQuestion_ID", c => c.Int());
            CreateIndex("dbo.Options", "AttemptedQuestion_ID");
            AddForeignKey("dbo.Options", "AttemptedQuestion_ID", "dbo.AttemptedQuestions", "ID");
            DropColumn("dbo.AttemptedQuestions", "SelectedOption_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AttemptedQuestions", "SelectedOption_ID", c => c.Int());
            DropForeignKey("dbo.Options", "AttemptedQuestion_ID", "dbo.AttemptedQuestions");
            DropIndex("dbo.Options", new[] { "AttemptedQuestion_ID" });
            DropColumn("dbo.Options", "AttemptedQuestion_ID");
            CreateIndex("dbo.AttemptedQuestions", "SelectedOption_ID");
            AddForeignKey("dbo.AttemptedQuestions", "SelectedOption_ID", "dbo.Options", "ID");
        }
    }
}
