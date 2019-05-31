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

        public List<TourViewModel> GetFilteredList()
        {
            var result = new List<TourViewModel>();

            List<TourViewModel> tours = context.Tours.Select(rec => new TourViewModel
            {
                Id = rec.Id,
                Name = rec.Name,
                Description = rec.Description,
                Cost = rec.Cost,
                TourReservations = context.TourReservations
                .Where(recCD => recCD.TourId == rec.Id)
                .Select(recCD => new TourReservationViewModel
                {
                    Id = recCD.Id,
                    TourId = recCD.TourId,
                    ReservationId = recCD.ReservationId,
                    ReservationName = recCD.Reservation.Name,
                    ReservationDescription = recCD.Reservation.Description,
                    NumberReservations = recCD.NumberReservations
                }).ToList()
            }).ToList();

            foreach (var tour in tours)
            {
                var tourReservations = tour.TourReservations.Select(rec => new ReservationViewModel
                {
                    Id = rec.ReservationId,
                    Name = context.Reservations.FirstOrDefault(recD => recD.Id == rec.ReservationId).Name,
                    Description = context.Reservations.FirstOrDefault(recD => recD.Id == rec.ReservationId).Description,
                    Number = context.Reservations.FirstOrDefault(recD => recD.Id == rec.ReservationId).Number
                }).ToList();

                if (tourReservations.All(rec => rec.Number > 0))
                {
                    result.Add(tour);
                }
            }

            return result;
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
                        ReservationName = recTR.Reservation.Name,
                        ReservationDescription = recTR.Reservation.Description,
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
                            ReservationName = recTR.Reservation.Name,
                            ReservationDescription = recTR.Reservation.Description,
                            NumberReservations = recTR.NumberReservations
                        }).ToList()
                };
            }
            throw new Exception("Тур не найден");
        }

        public void AddElement(TourBindingModel model)
        {
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Tour element = context.Tours.FirstOrDefault(rec => rec.Name == model.Name);
                    if (element != null)
                    {
                        throw new Exception("Уже есть тур с таким названием");
                    }
                    element = new Tour
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Cost = model.Cost
                    };
                    context.Tours.Add(element);
                    context.SaveChanges();

                    var groupComponents = model.TourReservations
                                                .GroupBy(rec => rec.ReservationId)
                                                .Select(rec => new
                                                {
                                                    ReservationId = rec.Key,
                                                    NumberReservations = rec.Sum(r => r.NumberReservations)
                                                });

                    foreach (var groupComponent in groupComponents)
                    {
                        context.TourReservations.Add(new TourReservation
                        {
                            TourId = element.Id,
                            ReservationId = groupComponent.ReservationId,
                            NumberReservations = groupComponent.NumberReservations
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

        public void UpdElement(TourBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Tour element = context.Tours.FirstOrDefault(rec => rec.Name == model.Name && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть тур с таким названием");
                    }
                    element = context.Tours.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.Name = model.Name;
                    element.Description = model.Description;
                    element.Cost = model.Cost;
                    context.SaveChanges();

                    var compIds = model.TourReservations.Select(rec => rec.ReservationId).Distinct();
                    var updateComponents = context.TourReservations.Where(rec => rec.TourId == model.Id && compIds.Contains(rec.ReservationId));
                    foreach (var updateComponent in updateComponents)
                    {
                        updateComponent.NumberReservations = model.TourReservations.FirstOrDefault(rec => rec.Id == updateComponent.Id).NumberReservations;
                    }
                    context.SaveChanges();

                    context.TourReservations.RemoveRange(context.TourReservations.Where(rec => rec.TourId == model.Id && !compIds.Contains(rec.ReservationId)));
                    context.SaveChanges();

                    var groupComponents = model.TourReservations.Where(rec => rec.Id == 0).GroupBy(rec => rec.ReservationId)
                        .Select(rec => new
                        {
                            ReservationId = rec.Key,
                            NumberReservations = rec.Sum(r => r.NumberReservations)
                        });
                    foreach (var groupComponent in groupComponents)
                    {
                        TourReservation elementPC = context.TourReservations.FirstOrDefault(rec => rec.TourId == model.Id && rec.ReservationId == groupComponent.ReservationId);
                        if (elementPC != null)
                        {
                            elementPC.NumberReservations += groupComponent.NumberReservations;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.TourReservations.Add(new TourReservation
                            {
                                TourId = model.Id,
                                ReservationId = groupComponent.ReservationId,
                                NumberReservations = groupComponent.NumberReservations
                            });
                            context.SaveChanges();
                        }
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

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Tour element = context.Tours.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        context.TourReservations.RemoveRange(context.TourReservations.Where(rec => rec.TourId == id));
                        context.Tours.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
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
    }
}
