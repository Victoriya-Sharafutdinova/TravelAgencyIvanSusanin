using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninDAL.BindingModel
{
    public class TourBindingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Cost { get; set; }

        public List<TourReservationBindingModel> TourReservations { get; set; }
    }
}
