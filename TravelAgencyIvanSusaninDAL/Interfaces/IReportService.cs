using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.ViewModel;

namespace TravelAgencyIvanSusaninDAL.Interfaces
{
    public interface IReportService
    {
        List<TourRequestViewModel> GetTourRequest(ReportBindingModel model);

        void SaveTourRequest(ReportBindingModel model);

        List<TravelsReservationsViewModel> GetClientTravels(int id);

        void SaveClientTravels(int id);
    }
}
