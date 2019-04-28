using System.ComponentModel;
using System;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    class RequestViewModel
    {
        public int Id { get; set; }

        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }
    }
}
