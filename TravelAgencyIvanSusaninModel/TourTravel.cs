using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninModel
{
    [DataContract]
    public class TourTravel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int TravelId { get; set; }

        [DataMember]
        public int TourId { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Column(TypeName = "datetime2")]
        public DateTime DateReservation { get; set; }

        [DataMember]
        [Required]
        public int Count { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата начала")]
        [Column(TypeName = "datetime2")]
        public DateTime DateBegin { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата конца")]
        [Column(TypeName = "datetime2")]
        public DateTime DateEnd { get; set; }

        public virtual Travel Travel { get; set; }

        public virtual Tour Tour { get; set; }
    }
}
