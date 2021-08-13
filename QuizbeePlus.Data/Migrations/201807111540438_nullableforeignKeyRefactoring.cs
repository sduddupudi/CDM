namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableforeignKeyRefactoring : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AttemptedQuestions", name: "StudentQuiz_ID", newName: "StudentQuizID");
            RenameIndex(table: "dbo.AttemptedQuestions", name: "IX_StudentQuiz_ID", newName: "IX_StudentQuizID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AttemptedQuestions", name: "IX_StudentQuizID", newName: "IX_StudentQuiz_ID");
            RenameColumn(table: "dbo.AttemptedQuestions", name: "StudentQuizID", newName: "StudentQuiz_ID");
        }
    }
}
