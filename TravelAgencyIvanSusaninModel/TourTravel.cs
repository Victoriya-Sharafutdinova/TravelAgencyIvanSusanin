using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninModel
{
    public class TourTravel
    {
        public int Id { get; set; }

        public int TravelId { get; set; }

        public int TourId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateReservation { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата начала")]
        [Column(TypeName = "datetime2")]
        public DateTime DateBegin { get; set; }

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
