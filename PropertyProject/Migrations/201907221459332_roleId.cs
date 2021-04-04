namespace PropertyProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roleId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "roleId", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "roleId");
        }
    }
}
