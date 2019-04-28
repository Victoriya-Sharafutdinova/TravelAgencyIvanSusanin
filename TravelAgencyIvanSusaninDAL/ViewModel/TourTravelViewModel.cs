using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class TourTravelViewModel
    {
        public int Id { get; set; }

        public int TravelId { get; set; }

        public int TourId { get; set; }

        [DisplayName("Дата резерва")]
        public DateTime DateReservation { get; set; }

        [DisplayName("Дата начала")]
        public DateTime DateBegin { get; set; }

        [DisplayName("Дата конца")]
        public DateTime DateEnd { get; set; }
    }
}
