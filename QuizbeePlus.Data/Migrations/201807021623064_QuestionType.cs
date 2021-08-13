namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Questions", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
