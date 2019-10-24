namespace ResultManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Results", "UnitCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Results", "UnitCode", c => c.Int(nullable: false));
        }
    }
}
