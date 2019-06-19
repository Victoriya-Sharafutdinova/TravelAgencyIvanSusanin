using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyIvanSusaninDAL.Interfaces
{
    public interface IStatisticService
    {
        string GetMostPopularTourName();

        int GetMostPopularTour();

        decimal GetAverageCustomerCheck(int clientId);

        int GetClientToursCount(int clientId);

        string  GetPopularTourClientName(int clientId);

        int GetPopularTourClient(int clientId);

        decimal GetAverPrice();
    }
}
