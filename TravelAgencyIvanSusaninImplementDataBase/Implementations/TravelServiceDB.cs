using System;
using System.Collections.Generic;
using System.Data.Entity;
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


        public void CreateTravel(TravelBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Travel element = new Travel
                    {
                        ClientId = model.ClientId,
                        DateCreate = DateTime.Now,
                        TotalCost = model.TotalCost,
                        TravelStatus = TravelStatus.Принят
                    };
                    context.Travels.Add(element);
                    context.SaveChanges();
                    // убираем дубли по машинам
                    var groupTours = model.TourTravels
                     .GroupBy(rec => rec.TourId)
                    .Select(rec => new
                    {
                        TourId = rec.Key,
                        Count = rec.Sum(r => r.Count)
                    });
                    // добавляем компоненты
                    foreach (var groupTour in groupTours)
                    {
                        context.TourTravels.Add(new TourTravel
                        {
                            TravelId = element.Id,
                            TourId = groupTour.TourId,
                            Count = groupTour.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishTravel(TravelBindingModel model)
        {
            Travel element = context.Travels.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.TravelStatus != TravelStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            element.TravelStatus = TravelStatus.Готов;
            context.SaveChanges();
        }

        public List<TravelViewModel> GetList()
        {
            List<TravelViewModel> result = context.Travels.Select(rec => new TravelViewModel
            {
                Id = rec.Id,
                ClientId = rec.ClientId,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
            SqlFunctions.DateName("mm", rec.DateCreate) + " " +
            SqlFunctions.DateName("yyyy", rec.DateCreate),
                DateImplement = rec.DateImplement == null ? "" :
            SqlFunctions.DateName("dd",
           rec.DateImplement.Value) + " " +
            SqlFunctions.DateName("mm",
           rec.DateImplement.Value) + " " +
            SqlFunctions.DateName("yyyy",
            rec.DateImplement.Value),
                TravelStatus = rec.TravelStatus.ToString(),
                TotalCost = rec.TotalCost,
                FIO = rec.Client.FIO,
                TourTravels = context.TourTravels
                .Where(recPC => recPC.TravelId == rec.Id)
                .Select(recPC => new TourTravelViewModel
                {
                    Id = recPC.Id,
                    TourId = recPC.TourId,
                    TravelId = recPC.TravelId,
                    TourName= recPC.Tour.Name,
                    Count = recPC.Count
                }).ToList()
            }).ToList();

            return result;
        }

        public void PayTravel(TravelBindingModel model)
        {
            Travel element = context.Travels.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.TravelStatus != TravelStatus.Готов)
            {
                throw new Exception("Путешествие не в статусе \"Готово\"");
            }
            element.TravelStatus = TravelStatus.Оплачен;
            context.SaveChanges();
        }

        public void TakeTravelInWork(TravelBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Travel element = context.Travels.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    if (element.TravelStatus != TravelStatus.Принят)
                    {
                        throw new Exception("Путешествие не в статусе \"Принят\"");
                    }

                    var travelTours = context.TourTravels.Include(rec => rec.Tour).Where(rec => rec.TravelId == element.Id);

                    foreach (var travelTour in travelTours)
                    {
                        var tourReservations = context.TourReservations.Include(rec => rec.Reservation).Where(rec => rec.TourId == travelTour.TourId);
                        foreach (var tourReservation in tourReservations)
                        {
                            int countReservations = tourReservation.Reservation.Number;
                            if (tourReservation.NumberReservations > countReservations)
                            {
                                throw new Exception("Недостаточно деталей");
                            }
                            else
                            {
                                tourReservation.Reservation.Number -= tourReservation.NumberReservations;
                                context.SaveChanges();
                                break;
                            }
                        }
                    }

                    element.DateImplement = DateTime.Now;
                    element.TravelStatus = TravelStatus.Выполняется;
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
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
