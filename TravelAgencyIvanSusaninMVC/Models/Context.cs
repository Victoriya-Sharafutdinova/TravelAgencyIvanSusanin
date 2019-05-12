using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TravelAgencyIvanSusaninMVC.Models
{
    public class Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Context() : base("name=Context")
        {
        }

        public System.Data.Entity.DbSet<TravelAgencyIvanSusaninModel.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<TravelAgencyIvanSusaninModel.Travel> Travels { get; set; }

        public System.Data.Entity.DbSet<TravelAgencyIvanSusaninModel.Tour> Tours { get; set; }
    }
}
