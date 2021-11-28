namespace CrudWithImage1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PicturePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PicturePath");
        }
    }
}
