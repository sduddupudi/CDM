namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttemptedOptions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Options", "AttemptedQuestion_ID", "dbo.AttemptedQuestions");
            DropIndex("dbo.Options", new[] { "AttemptedQuestion_ID" });
            CreateTable(
                "dbo.AttemptedOptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AttemptedQuestionID = c.Int(nullable: false),
                        OptionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Options", t => t.OptionID, cascadeDelete: true)
                .ForeignKey("dbo.AttemptedQuestions", t => t.AttemptedQuestionID, cascadeDelete: true)
                .Index(t => t.AttemptedQuestionID)
                .Index(t => t.OptionID);
            
            DropColumn("dbo.Options", "AttemptedQuestion_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Options", "AttemptedQuestion_ID", c => c.Int());
            DropForeignKey("dbo.AttemptedOptions", "AttemptedQuestionID", "dbo.AttemptedQuestions");
            DropForeignKey("dbo.AttemptedOptions", "OptionID", "dbo.Options");
            DropIndex("dbo.AttemptedOptions", new[] { "OptionID" });
            DropIndex("dbo.AttemptedOptions", new[] { "AttemptedQuestionID" });
            DropTable("dbo.AttemptedOptions");
            CreateIndex("dbo.Options", "AttemptedQuestion_ID");
            AddForeignKey("dbo.Options", "AttemptedQuestion_ID", "dbo.AttemptedQuestions", "ID");
        }
    }
}
