using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninModel
{
    [DataContract]
    public class ReservationRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ReservationId { get; set; }

        [DataMember]
        public int RequestId { get; set; }

        [DataMember]
        [Required]
        public int NumberReservation { get; set; }

        public virtual Reservation Reservation { get; set; }

        public virtual Request Request { get; set; }
    }
}
