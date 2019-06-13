namespace TravelAgencyIvanSusaninImplementDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class some5migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "NumberReserve", c => c.Int(nullable: false));
            AlterColumn("dbo.Travels", "TotalCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Travels", "TotalCost", c => c.Int(nullable: false));
            DropColumn("dbo.Reservations", "NumberReserve");
        }
    }
}
