namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionPropertyNameSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttemptedQuestions", "IsCorrect", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttemptedQuestions", "IsCorrect");
        }
    }
}
