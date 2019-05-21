using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.ViewModel;

namespace TravelAgencyIvanSusaninDAL.Interfaces
{
    public interface ITravelService
    {
        List<TravelViewModel> GetList();

        void CreateTravel(TravelBindingModel model);

        void TakeTravelInWork(TravelBindingModel model);

        void FinishTravel(TravelBindingModel model);

        void PayTravel(TravelBindingModel model);

        void Reservation(TourTravelBindingModel model);
    }
}
