using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class TravelViewModel
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        [DisplayName("ФИО Клиента")]
        public string FIO { get; set; }

        public List<TourTravelViewModel> TourTravels { get; set; }

        [DisplayName("Дата создания")]
        public string DateCreate { get; set; }

        [DisplayName("Статус путешествия")]
        public string TravelStatus { get; set; }

        [DisplayName("Расчетная стоимость")]
        public int TotalCost { get; set; }

        [DisplayName("Дата завершения заказа")]
        public string DateImplement { get; set; }
    }
}
