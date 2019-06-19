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

        public int Total { get; set; }

        public IEnumerable<Tuple<string, int>> Reservations { get; set; }
    }
}
