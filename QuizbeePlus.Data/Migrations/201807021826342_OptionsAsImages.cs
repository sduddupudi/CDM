namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionsAsImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Options", "Image_ID", c => c.Int());
            CreateIndex("dbo.Options", "Image_ID");
            AddForeignKey("dbo.Options", "Image_ID", "dbo.Images", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Options", "Image_ID", "dbo.Images");
            DropIndex("dbo.Options", new[] { "Image_ID" });
            DropColumn("dbo.Options", "Image_ID");
        }
    }
}
