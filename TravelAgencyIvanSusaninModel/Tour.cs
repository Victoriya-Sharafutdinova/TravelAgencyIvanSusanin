using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninModel
{
    [DataContract]
    public class Tour
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public int Cost { get; set; }

        [DataMember]
        [Column(TypeName = "datetime2")]
        public DateTime DateCreate { get; set; }

        [ForeignKey("TourId")]
        public virtual List<TourTravel> TourTravels { get; set; }

        [ForeignKey("TourId")]
        public virtual List<TourReservation> TourReservations { get; set; }
    }
}
