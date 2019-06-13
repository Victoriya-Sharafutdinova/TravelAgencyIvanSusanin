namespace TravelAgencyIvanSusaninImplementDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somemigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TourTravels", "DateBegin");
            DropColumn("dbo.TourTravels", "DateEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TourTravels", "DateEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.TourTravels", "DateBegin", c => c.DateTime(nullable: false));
        }
    }
}
