namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreignKeyRefactoring : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AttemptedQuestions", "Question_ID", "dbo.Questions");
            DropIndex("dbo.AttemptedQuestions", new[] { "Question_ID" });
            RenameColumn(table: "dbo.AttemptedQuestions", name: "Question_ID", newName: "QuestionID");
            RenameColumn(table: "dbo.Questions", name: "Image_ID", newName: "ImageID");
            RenameColumn(table: "dbo.Options", name: "Image_ID", newName: "ImageID");
            RenameIndex(table: "dbo.Questions", name: "IX_Image_ID", newName: "IX_ImageID");
            RenameIndex(table: "dbo.Options", name: "IX_Image_ID", newName: "IX_ImageID");
            AddColumn("dbo.AttemptedQuestions", "Score", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AttemptedOptions", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.AttemptedQuestions", "QuestionID", c => c.Int(nullable: false));
            CreateIndex("dbo.AttemptedQuestions", "QuestionID");
            AddForeignKey("dbo.AttemptedQuestions", "QuestionID", "dbo.Questions", "ID", cascadeDelete: true);
            DropColumn("dbo.AttemptedQuestions", "IsCorrect");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AttemptedQuestions", "IsCorrect", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AttemptedQuestions", "QuestionID", "dbo.Questions");
            DropIndex("dbo.AttemptedQuestions", new[] { "QuestionID" });
            AlterColumn("dbo.AttemptedQuestions", "QuestionID", c => c.Int());
            DropColumn("dbo.AttemptedOptions", "ModifiedOn");
            DropColumn("dbo.AttemptedQuestions", "Score");
            RenameIndex(table: "dbo.Options", name: "IX_ImageID", newName: "IX_Image_ID");
            RenameIndex(table: "dbo.Questions", name: "IX_ImageID", newName: "IX_Image_ID");
            RenameColumn(table: "dbo.Options", name: "ImageID", newName: "Image_ID");
            RenameColumn(table: "dbo.Questions", name: "ImageID", newName: "Image_ID");
            RenameColumn(table: "dbo.AttemptedQuestions", name: "QuestionID", newName: "Question_ID");
            CreateIndex("dbo.AttemptedQuestions", "Question_ID");
            AddForeignKey("dbo.AttemptedQuestions", "Question_ID", "dbo.Questions", "ID");
        }
    }
}
