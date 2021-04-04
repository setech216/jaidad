namespace PropertyProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roleId2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "roleId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "roleId", c => c.Int(nullable: false));
        }
    }
}
