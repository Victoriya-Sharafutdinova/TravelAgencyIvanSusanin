namespace TravelAgencyIvanSusaninImplementDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class some3migration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Travels", "DateImplement", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.TourTravels", "DateReservation", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TourTravels", "DateReservation", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Travels", "DateImplement", c => c.DateTime());
        }
    }
}
