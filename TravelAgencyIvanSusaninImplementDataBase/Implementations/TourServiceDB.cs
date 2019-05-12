using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninModel;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class TourServiceDB : ITourService
    {
        private AbstractDbContext context;

        public TourServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<TourViewModel> GetList()
        {
            List<TourViewModel> result = context.Tours.Select(rec => new TourViewModel
            {
                Id = rec.Id,
                Name = rec.Name,
                Description = rec.Description,
                Cost = rec.Cost,
                TourReservations = context.TourReservations
                    .Where(recTR => recTR.TourId == rec.Id)
                    .Select(recTR => new TourReservationViewModel
                    {
                        Id = recTR.Id,
                        TourId = recTR.TourId,
                        ReservationId = recTR.ReservationId,
                        NumberReservations = recTR.NumberReservations
                    }).ToList()
            }).ToList();

            return result;
        }

        public TourViewModel GetElement(int id)
        {
            Tour element = context.Tours.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new TourViewModel
                {
                    Id = element.Id,
                    Name = element.Name,
                    Description = element.Description,
                    Cost = element.Cost,
                    TourReservations = context.TourReservations
                        .Where(recTR => recTR.TourId == element.Id)
                        .Select(recTR => new TourReservationViewModel
                        {
                            Id = recTR.Id,
                            TourId = recTR.TourId,
                            ReservationId = recTR.ReservationId,
                            NumberReservations = recTR.NumberReservations
                        }).ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(TourBindingModel model)
        {
            throw new NotImplementedException();
        }

        public void UpdElement(TourBindingModel model)
        {
            throw new NotImplementedException();
        }

        public void DelElement(int id)
        {
            throw new NotImplementedException();
        }
    }
}
