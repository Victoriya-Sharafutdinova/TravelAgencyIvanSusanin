using System;
using System.Collections.Generic;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using System.Linq;
using TravelAgencyIvanSusaninModel;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class ReservationServiceDB : IReservationService
    {
        private AbstractDbContext context;

        public ReservationServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ReservationViewModel> GetList()
        {
            List<ReservationViewModel> result = context.Reservations.Select(rec => new ReservationViewModel
            {
                Id = rec.Id,
                Name = rec.Name,
                Description = rec.Description,
                Number = rec.Number,
                NumberReserve = rec.NumberReserve
            }).ToList();
            return result;
        }

        public ReservationViewModel GetElement(int id)
        {
            Reservation element = context.Reservations.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ReservationViewModel
                {
                    Id = element.Id,
                    Name = element.Name,
                    Description = element.Description,
                    Number = element.Number
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ReservationBindingModel model)
        {
            Reservation element = context.Reservations.FirstOrDefault(rec => rec.Name == model.Name);
            if (element != null)
            {
                throw new Exception("Уже есть бронь с таким названием");
            }
            context.Reservations.Add(new Reservation
            {
                Name = model.Name,
                Description = model.Description,
                Number = 0
            });
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Reservation element = context.Reservations.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Reservations.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public void UpdElement(ReservationBindingModel model)
        {
            Reservation element = context.Reservations.FirstOrDefault(rec => rec.Name == model.Name && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть бронь с таким названием");
            }
            element = context.Reservations.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Name = model.Name;
            element.Description = model.Description;
            context.SaveChanges();
        }
    }
}
