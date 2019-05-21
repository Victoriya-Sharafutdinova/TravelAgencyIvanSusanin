using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class ClientTravelsViewModel
    {
        public string ClientName { get; set; }

        public string DateCreateTravel { get; set; }

        public List<TourTravelViewModel> TourTravels { get; set; }

        public decimal TotalSum { get; set; }

        public string StatusTravel { get; set; }
    }
}
