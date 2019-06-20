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

        List<TravelViewModel> GetClientTravels(int clientId);

        void CreateTravel(TravelBindingModel model);

        void TakeTravelInWork(TravelBindingModel model);

        void FinishTravel(TravelBindingModel model);

        void PayTravel(TravelBindingModel model);

        void Reservation(int id, string type);

        TravelViewModel GetElement(int id);

        void SaveDataBaseClient();

        void SaveDataBaseAdmin();
    }
}
