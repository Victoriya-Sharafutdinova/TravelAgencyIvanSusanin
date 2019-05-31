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
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Tour tour = context.Tours.FirstOrDefault(
                        record => record.Name == model.Name);

                    if (tour != null)
                    {
                        throw new Exception("Такой тур уже существует.");
                    }
                    else
                    {
                        tour = new Tour
                        {
                            Name = model.Name,
                            Description = model.Description,
                            Cost = model.Cost
                        };
                    }

                    context.Tours.Add(tour);
                    context.SaveChanges();

                    var duplicates = model.TourReservations
                        .GroupBy(record => record.ReservationId)
                        .Select(record => new
                        {
                            reservationId = record.Key,
                            numberReservations = record.Sum(rec => rec.NumberReservations)
                        });

                    foreach (var duplicate in duplicates)
                    {
                        context.TourReservations.Add(new TourReservation
                        {
                            TourId = tour.Id,
                            ReservationId = duplicate.reservationId,
                            NumberReservations = duplicate.numberReservations
                        });
                        context.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    transaction.Commit();
                }
            }
        }

        public void UpdElement(TourBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Tour tour = context.Tours.FirstOrDefault(
                        record => record.Name == model.Name && record.Id != model.Id);

                    if (tour != null)
                    {
                        throw new Exception("Уже есть тур с таким названием");
                    }

                    tour = context.Tours.FirstOrDefault(
                        record => record.Id == model.Id);

                    if (tour == null)
                    {
                        throw new Exception("Тур не найден");
                    }

                    tour.Name = model.Name;
                    tour.Description = model.Description;
                    tour.Cost = model.Cost;
                    context.SaveChanges();

                    var IDs = model.TourReservations.Select(
                        record => record.ReservationId)
                        .Distinct();

                    var updateReservations = context.TourReservations.Where(
                        record => record.TourId == model.Id && IDs.Contains(record.ReservationId));

                    foreach (var updateReservation in updateReservations)
                    {
                        updateReservation.NumberReservations = model.TourReservations.FirstOrDefault(
                            record => record.Id == updateReservation.Id)
                            .NumberReservations;
                    }

                    context.SaveChanges();

                    context.TourReservations.RemoveRange(context.TourReservations.Where(
                        record => record.TourId == model.Id && !IDs.Contains(record.ReservationId)));

                    context.SaveChanges();

                    var groupReservations = model.TourReservations.Where(
                        record => record.Id == 0)
                        .GroupBy(record => record.ReservationId)
                        .Select(record => new
                        {
                            reservationId = record.Key,
                            numberReservations = record.Sum(r => r.NumberReservations)
                        });

                    foreach (var groupReservation in groupReservations)
                    {
                        TourReservation reservation = context.TourReservations.FirstOrDefault(
                            record => record.TourId == model.Id && record.ReservationId == groupReservation.reservationId);

                        if (reservation != null)
                        {
                            reservation.NumberReservations += groupReservation.numberReservations;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.TourReservations.Add(new TourReservation
                            {
                                TourId = model.Id,
                                ReservationId = groupReservation.reservationId,                 
                                NumberReservations = groupReservation.numberReservations
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
                    Tour tour = context.Tours.FirstOrDefault(
                        record => record.Id == id);

                    if (tour != null)
                    {
                        context.TourReservations.RemoveRange(
                            context.TourReservations.Where(
                                record => record.TourId == id));

                        context.Tours.Remove(tour);

                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Тур не найден");
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
