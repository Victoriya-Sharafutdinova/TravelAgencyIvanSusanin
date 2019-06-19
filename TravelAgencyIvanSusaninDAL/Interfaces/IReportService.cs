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
        List<ClientTravelsViewModel> GetReservationReguest(ReportBindingModel model);

        void SaveReservationReguest(ReportBindingModel model);

        List<ClientTravelsViewModel> GetClientTravels();

        void SaveClientTravels(ReportBindingModel model);
    }
}
