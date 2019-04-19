using System.ComponentModel.DataAnnotations;

namespace TravelAgencyIvanSusaninModel
{
    public class ReservationRequest
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        public int RequestId { get; set; }

        [Required]
        public int NumberReservation { get; set; }

        public virtual Reservation Reservation { get; set; }

        public virtual Request Request { get; set; }
    }
}
