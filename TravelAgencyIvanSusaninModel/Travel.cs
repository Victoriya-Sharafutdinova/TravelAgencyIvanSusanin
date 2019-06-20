using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninModel
{
    [DataContract]
    public class Travel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public int ClientId { get; set; }

        [DataMember]
        [Required]
        public decimal TotalCost { get; set; }

        [DataMember]
        public TravelStatus TravelStatus { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime DateCreate { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Column(TypeName = "datetime2")]
        public DateTime? DateImplement { get; set; }

        public virtual Client Client { get; set; }

        [ForeignKey("TravelId")]
        public virtual List<TourTravel> TourTravels { get; set; }
    }
}
