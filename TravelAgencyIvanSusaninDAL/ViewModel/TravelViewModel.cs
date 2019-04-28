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
        public string ClientFIO { get; set; }

        public int TourId { get; set; }

        [DisplayName("Название тура")]
        public string TourName { get; set; }

        [DisplayName("Дата создания")]
        public string DateCreate { get; set; }

        [DisplayName("Расчетная стоимость")]
        public int TotalCost { get; set; }

        //[DisplayName("Дата резерва")]
        //public string DateReservation { get; set; }

        //[DisplayName("Дата начала")]
        //public DateTime DateBegin { get; set; }

        //[DisplayName("Дата конца")]
        //public DateTime DateEnd { get; set; }
    }
}
