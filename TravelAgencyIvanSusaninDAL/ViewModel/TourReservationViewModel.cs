using System.ComponentModel;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class TourReservationViewModel
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        public int TourId { get; set; }

        [DisplayName("Количество броней")]
        public int NumberReservations { get; set; }
    }
}
