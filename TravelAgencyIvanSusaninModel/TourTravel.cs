using System;
using System.ComponentModel.DataAnnotations;

namespace TravelAgencyIvanSusaninModel
{
    public class TourTravel
    {
        public int Id { get; set; }

        public int TravelId { get; set; }

        public int TourId { get; set; }

        public DateTime DateReservation { get; set; }

        [Required]
        public DateTime DateBegin { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }

        public virtual Travel Travel { get; set; }

        public virtual Tour Tour { get; set; }
    }
}
