using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyIvanSusaninModel
{
    public class Tour
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int Cost { get; set; }

        [ForeignKey("TourId")]
        public virtual List<TourTravel> TourTravels { get; set; }

        [ForeignKey("TourId")]
        public virtual List<TourReservation> TourReservation { get; set; }
    }
}
