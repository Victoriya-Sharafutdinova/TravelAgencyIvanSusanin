using System.ComponentModel.DataAnnotations;

namespace TravelAgencyIvanSusaninModel
{
    public class TourReservation
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        public int TourId { get; set; }

        [Required]
        public int NumberReservations { get; set; }

        public virtual Reservation Reservation { get; set; }

        public virtual Tour Tour { get; set; }
    }
}
