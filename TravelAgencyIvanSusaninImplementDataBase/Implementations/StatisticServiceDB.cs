using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencyIvanSusaninDAL.Interfaces;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class StatisticServiceDB : IStatisticService
    {
        private readonly AbstractDbContext context;

        public StatisticServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }
        public int GetMostPopularTour()
        {
            var most = context.TourTravels
                .GroupBy(rec => rec.TourId)
                .Select(rec => new { Id = rec.Key, Total = rec.Sum(x => x.Count) })
                .OrderByDescending(rec => rec.Total)
                .First();

            var name = context.Tours.FirstOrDefault(rec => rec.Id == most.Id)?.Name;

            var count = most.Total;

            return count;
        }
        public string GetMostPopularTourName()
        {
            var most = context.TourTravels
                .GroupBy(rec => rec.TourId)
                .Select(rec => new { Id = rec.Key, Total = rec.Sum(x => x.Count) })
                .OrderByDescending(rec => rec.Total)
                .First();

            var name = context.Tours.FirstOrDefault(rec => rec.Id == most.Id)?.Name;

            var count = most.Total;

            return name;
        }


        public int GetClientToursCount(int clientId)
        {
            int clientTours = context.Travels
                .Count(travel => travel.ClientId == clientId);

            if (clientTours != 0)
            {
                return context.Travels
                    .Where(travel => travel.ClientId == clientId)
                    .Sum(travel => travel.TourTravels.Sum(x => x.Count));
            }
            else
            {
                return 0;
            }
        }

        public decimal GetAverageCustomerCheck(int clientId)
        {
            int clientTours = context.Travels
                .Count(travel => travel.ClientId == clientId);

            if (clientTours != 0)
            {
                return context.Travels
                    .Where(travel => travel.ClientId == clientId)
                    .Average(travel => travel.TotalCost);
            }
            else
            {
                return 0;
            }
        }


        public string GetPopularTourClientName(int clientId)
        {
            var most = context.TourTravels
                .Where(rec => rec.Travel.ClientId == clientId)
                .GroupBy(rec => rec.TourId)
                .Select(rec => new { Id = rec.Key, Total = rec.Sum(x => x.Count) })
                .OrderByDescending(rec => rec.Total)
                .FirstOrDefault();

            if (most != null)
            {
                var name = context.Tours.FirstOrDefault(rec => rec.Id == most.Id)?.Name;

                var count = most.Total;

                return name;
            }
            else
            {
                return null;
            }
        }


        public int  GetPopularTourClient(int clientId)
        {
            var most = context.TourTravels
                .Where(rec => rec.Travel.ClientId == clientId)
                .GroupBy(rec => rec.TourId)
                .Select(rec => new { Id = rec.Key, Total = rec.Sum(x => x.Count) })
                .OrderByDescending(rec => rec.Total)
                .FirstOrDefault();

            if (most != null)
            {
                var name = context.Tours.FirstOrDefault(rec => rec.Id == most.Id)?.Name;

                var count = most.Total;

                return count;
            }
            else
            {
                return  0;
            }
        }

        public decimal GetAverPrice()
        {
            return context.Travels.Average(travel => travel.TotalCost);
        }
    }
}
