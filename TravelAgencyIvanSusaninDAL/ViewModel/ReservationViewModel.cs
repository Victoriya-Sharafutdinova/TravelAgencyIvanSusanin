using System.ComponentModel;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class ReservationViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        public string Name { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Общее количество")]
        public int Number { get; set; }

    }
}
