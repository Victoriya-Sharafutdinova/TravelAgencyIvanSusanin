using System.ComponentModel;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class ReservationRequestViewModel
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        public int RequestId { get; set; }

        [DisplayName("Количество броней")]
        public int NumberReservation { get; set; }
    }
}
