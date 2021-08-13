namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentQuizRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentQuizs", "Quiz_ID", "dbo.Quizs");
            DropIndex("dbo.StudentQuizs", new[] { "Quiz_ID" });
            RenameColumn(table: "dbo.StudentQuizs", name: "Quiz_ID", newName: "QuizID");
            AlterColumn("dbo.StudentQuizs", "QuizID", c => c.Int(nullable: false));
            CreateIndex("dbo.StudentQuizs", "QuizID");
            AddForeignKey("dbo.StudentQuizs", "QuizID", "dbo.Quizs", "ID", cascadeDelete: true);
            DropColumn("dbo.Questions", "IsMCQ");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "IsMCQ", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.StudentQuizs", "QuizID", "dbo.Quizs");
            DropIndex("dbo.StudentQuizs", new[] { "QuizID" });
            AlterColumn("dbo.StudentQuizs", "QuizID", c => c.Int());
            RenameColumn(table: "dbo.StudentQuizs", name: "QuizID", newName: "Quiz_ID");
            CreateIndex("dbo.StudentQuizs", "Quiz_ID");
            AddForeignKey("dbo.StudentQuizs", "Quiz_ID", "dbo.Quizs", "ID");
        }
    }
}
