namespace AspNetMVC_API_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "RegisterDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Students", "RegisterDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "RegisterDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Students", "RegisterDate");
        }
    }
}
