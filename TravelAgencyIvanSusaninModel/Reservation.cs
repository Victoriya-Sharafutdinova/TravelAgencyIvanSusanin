using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyIvanSusaninModel
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Number { get; set; }

        [ForeignKey("ReservationId")]
        public virtual List<TourReservation> TourReservations { get; set; }

        [ForeignKey("ReservationId")]
        public virtual List<ReservationRequest> ReservationRequest { get; set; }
    }
}
