namespace QuizbeePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeparatingImage : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Images", "ModifiedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "ModifiedOn", c => c.DateTime());
        }
    }
}
