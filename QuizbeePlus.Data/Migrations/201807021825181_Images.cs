namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Images : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Questions", "Image_ID", c => c.Int());
            CreateIndex("dbo.Questions", "Image_ID");
            AddForeignKey("dbo.Questions", "Image_ID", "dbo.Images", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Image_ID", "dbo.Images");
            DropIndex("dbo.Questions", new[] { "Image_ID" });
            DropColumn("dbo.Questions", "Image_ID");
            DropTable("dbo.Images");
        }
    }
}
