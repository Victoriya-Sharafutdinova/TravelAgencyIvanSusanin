using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyIvanSusaninModel
{
    public class Travel
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        [Required]
        public DateTime TimeCreate { get; set; }

        [Required]
        public int TotalCost { get; set; }

        public virtual Client Client { get; set; }

        [ForeignKey("TravelId")]
        public virtual List<TourTravel> TourTravels { get; set; }
    }
}
