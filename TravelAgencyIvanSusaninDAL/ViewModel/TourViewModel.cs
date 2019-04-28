using System.ComponentModel;

namespace TravelAgencyIvanSusaninDAL.ViewModel
{
    public class TourViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        public string Name { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Стоимость")]
        public int Cost { get; set; }
    }
}
