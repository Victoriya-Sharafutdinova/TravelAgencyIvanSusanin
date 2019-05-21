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
        List<ClientTravelsViewModel> GetDetailReguest(ReportBindingModel model);

        void SaveDetailReguest(ReportBindingModel model);

        List<ClientTravelsViewModel> GetClientTravels(ReportBindingModel model);

        void SaveClientTravels(ReportBindingModel model);
    }
}
