using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninModel
{
    [DataContract]
    public class Reservation
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public int Number { get; set; }

        [DataMember]
        [Required]
        public int NumberReserve { get; set; }

        [ForeignKey("ReservationId")]
        public virtual List<TourReservation> TourReservations { get; set; }

        [ForeignKey("ReservationId")]
        public virtual List<ReservationRequest> ReservationRequest { get; set; }
    }
}
