namespace TravelAgencyIvanSusaninMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FIO = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Travels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        TotalCost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.TourTravels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TravelId = c.Int(nullable: false),
                        TourId = c.Int(nullable: false),
                        DateReservation = c.DateTime(nullable: false),
                        DateBegin = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tours", t => t.TourId, cascadeDelete: true)
                .ForeignKey("dbo.Travels", t => t.TravelId, cascadeDelete: true)
                .Index(t => t.TravelId)
                .Index(t => t.TourId);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Cost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TourReservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReservationId = c.Int(nullable: false),
                        TourId = c.Int(nullable: false),
                        NumberReservations = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reservations", t => t.ReservationId, cascadeDelete: true)
                .ForeignKey("dbo.Tours", t => t.TourId, cascadeDelete: true)
                .Index(t => t.ReservationId)
                .Index(t => t.TourId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Number = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReservationRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReservationId = c.Int(nullable: false),
                        RequestId = c.Int(nullable: false),
                        NumberReservation = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requests", t => t.RequestId, cascadeDelete: true)
                .ForeignKey("dbo.Reservations", t => t.ReservationId, cascadeDelete: true)
                .Index(t => t.ReservationId)
                .Index(t => t.RequestId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TourTravels", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.TourTravels", "TourId", "dbo.Tours");
            DropForeignKey("dbo.TourReservations", "TourId", "dbo.Tours");
            DropForeignKey("dbo.TourReservations", "ReservationId", "dbo.Reservations");
            DropForeignKey("dbo.ReservationRequests", "ReservationId", "dbo.Reservations");
            DropForeignKey("dbo.ReservationRequests", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.Travels", "ClientId", "dbo.Clients");
            DropIndex("dbo.ReservationRequests", new[] { "RequestId" });
            DropIndex("dbo.ReservationRequests", new[] { "ReservationId" });
            DropIndex("dbo.TourReservations", new[] { "TourId" });
            DropIndex("dbo.TourReservations", new[] { "ReservationId" });
            DropIndex("dbo.TourTravels", new[] { "TourId" });
            DropIndex("dbo.TourTravels", new[] { "TravelId" });
            DropIndex("dbo.Travels", new[] { "ClientId" });
            DropTable("dbo.Requests");
            DropTable("dbo.ReservationRequests");
            DropTable("dbo.Reservations");
            DropTable("dbo.TourReservations");
            DropTable("dbo.Tours");
            DropTable("dbo.TourTravels");
            DropTable("dbo.Travels");
            DropTable("dbo.Clients");
        }
    }
}
