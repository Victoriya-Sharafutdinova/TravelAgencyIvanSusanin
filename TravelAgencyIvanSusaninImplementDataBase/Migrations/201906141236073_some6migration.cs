namespace TravelAgencyIvanSusaninImplementDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class some6migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "DateCreate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "DateCreate");
        }
    }
}
