using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class TravelsReservationsViewModel
    {
        public int TravelId { get; set; }

        public List<TourReservationViewModel> Reservations { get; set; }
    }
}
