namespace TravelAgencyIvanSusaninMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Travels", "TravelStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Travels", "DateImplement", c => c.DateTime());
            AddColumn("dbo.TourTravels", "Count", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TourTravels", "Count");
            DropColumn("dbo.Travels", "DateImplement");
            DropColumn("dbo.Travels", "TravelStatus");
        }
    }
}
