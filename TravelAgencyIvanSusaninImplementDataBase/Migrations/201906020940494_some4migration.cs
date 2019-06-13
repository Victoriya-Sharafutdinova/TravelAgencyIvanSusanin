namespace TravelAgencyIvanSusaninImplementDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class some4migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TourTravels", "DateBegin", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.TourTravels", "DateEnd", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TourTravels", "DateEnd");
            DropColumn("dbo.TourTravels", "DateBegin");
        }
    }
}
