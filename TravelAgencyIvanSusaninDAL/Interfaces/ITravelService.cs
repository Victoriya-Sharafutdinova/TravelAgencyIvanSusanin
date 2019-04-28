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

        void CreateTravel(TravelBindingModel);

        void Reservation(TravelBindingModel);
    }
}
