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

        [DisplayName("Дата создания")]
        public DateTime TimeCreate { get; set; }

        [DisplayName("Расчетная стоимость")]
        public int TotalCost { get; set; }
    }
}
