using System.Collections.Generic;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.ViewModel;

namespace TravelAgencyIvanSusaninDAL.Interfaces
{
    public interface ITourService
    {
        List<TourViewModel> GetList();

        TourViewModel GetElement(int id);

        void AddElement(TourBindingModel model);

        void UpdElement(TourBindingModel model);

        void DelElement(int id);
    }
}
