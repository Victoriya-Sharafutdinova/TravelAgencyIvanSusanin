using System.Collections.Generic;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.ViewModel;

namespace TravelAgencyIvanSusaninDAL.Interfaces
{
    public interface IReservationService
    {
        List<ReservationViewModel> GetList();

        ReservationViewModel GetElement(int id);

        void AddElement(ReservationBindingModel model);

        void UpdElement(ReservationBindingModel model);

        void DelElement(int id);
    }
}
