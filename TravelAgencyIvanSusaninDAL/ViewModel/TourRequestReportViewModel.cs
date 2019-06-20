using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class TourRequestReportViewModel
    {
        [DisplayName("Тур")]
        public string TourName { get; set; }

        [DisplayName("Дата создания тура")]
        public DateTime TourDateCreate { get; set; }

        [DisplayName("Бронь")]
        public string ReservationName { get; set; }

        [DisplayName("Количество броней")]
        public int NumberReservations { get; set; }

        [DisplayName("Заявка")]
        public DateTime RequestDateCreate { get; set; }

        [DisplayName("Кол-во броней в заявке")]
        public int NumberReservationsRequest { get; set; }
    }
}
