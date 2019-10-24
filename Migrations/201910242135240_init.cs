namespace ResultManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "ImgPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "ImgPath");
        }
    }
}
