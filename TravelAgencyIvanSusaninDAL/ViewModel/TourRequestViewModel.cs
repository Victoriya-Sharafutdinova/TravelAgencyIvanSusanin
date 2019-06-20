using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class TourRequestViewModel
    {
        public int Id { get; set; }

        public string TourName { get; set; }

        public DateTime TourDateCreate { get; set; }

        public List<TourReservationViewModel> Reservations { get; set; }
    }
}
