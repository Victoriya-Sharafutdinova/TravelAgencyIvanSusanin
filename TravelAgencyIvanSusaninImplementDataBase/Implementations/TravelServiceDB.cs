using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninModel;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class TravelServiceDB : ITravelService
    {
        private AbstractDbContext context;

        public TravelServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<TravelViewModel> GetList()
        {
            List<TravelViewModel> result = context.Travels.Select(rec => new TravelViewModel
            {
                Id = rec.Id,
                ClientId = rec.ClientId,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " + SqlFunctions.DateName("mm", rec.DateCreate) + " " + SqlFunctions.DateName("yyyy", rec.DateCreate),
                TotalCost = rec.TotalCost,
            })
            .ToList();
            return result;
        }

        public void CreateTravel(TravelBindingModel model)
        {
            context.Travels.Add(new Travel
            {
                ClientId = model.ClientId,
                DateCreate = DateTime.Now,
                TotalCost = model.TotalCost,
            });
            context.SaveChanges();
        }

        public void Reservation (TourTravelBindingModel model)
        {
            TourTravel element = context.TourTravels.FirstOrDefault(rec => rec.TravelId == model.TravelId && rec.TourId == model.TourId);
            if (element != null)
            {
                element.DateReservation = model.DateReservation;
            }
            else
            {
                context.TourTravels.Add(new TourTravel
                {
                    TravelId = model.TravelId,
                    TourId = model.TourId,
                    DateReservation = DateTime.Now,
                    DateBegin = model.DateBegin,
                    DateEnd = model.DateEnd
                });
            }
            context.SaveChanges();
        }
    }
}
