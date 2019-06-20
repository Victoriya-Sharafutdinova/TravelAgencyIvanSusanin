using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninModel
{
    [DataContract]
    public class TourReservation
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ReservationId { get; set; }

        [DataMember]
        public int TourId { get; set; }

        [DataMember]
        [Required]
        public int NumberReservations { get; set; }

        public virtual Reservation Reservation { get; set; }

        public virtual Tour Tour { get; set; }
    }
}
