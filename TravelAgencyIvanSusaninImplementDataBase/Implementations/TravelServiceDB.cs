using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninModel;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class TravelServiceDB : ITravelService
    {
        private readonly AbstractDbContext context;

        public TravelServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }


        public TravelViewModel GetElement(int id)
        {
            var element = context.Travels.FirstOrDefault(rec => rec.Id == id);

            if (element != null)
            {
                return new TravelViewModel
                {
                    Id = element.Id,
                    ClientId = element.ClientId,
                    FIO = context.Clients.FirstOrDefault(client => client.Id == element.ClientId).FIO,
                    TotalCost = element.TotalCost,
                    TravelStatus = element.TravelStatus.ToString(),
                    DateCreate = element.DateCreate.ToString(),
                    DateImplement = element.DateImplement.ToString(),
                    TourTravels = context.TourTravels.Where(recOC => recOC.TravelId == element.Id)
                        .Select(recOC => new TourTravelViewModel
                        {
                            Id = recOC.Id,
                            TravelId = recOC.TravelId,
                            TourId = recOC.TourId,
                            TourName = recOC.Tour.Name,
                            Count = recOC.Count
                        })
                        .ToList()
                };
            }
            throw new Exception("Элемент не найден");
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
                    var groupTours = model.TourTravels
                    .GroupBy(rec => rec.TourId)
                    .Select(rec => new
                    {
                        TourId = rec.Key,
                        Count = rec.Sum(r => r.Count), 
                        
                    });
                    foreach (var groupTour in groupTours)
                    {
                        context.TourTravels.Add(new TourTravel
                        {
                            TravelId = element.Id,
                            TourId = groupTour.TourId,
                            Count = groupTour.Count,
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
            element.DateImplement = DateTime.Now;
            element.TravelStatus = TravelStatus.Готов;
            context.SaveChanges();
        }

        public List<TravelViewModel> GetClientTravels(int clientId)
        {
            return GetList().Where(travel => travel.ClientId == clientId).ToList();
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
                    Count = recPC.Count,
                    DateBegin = recPC.DateBegin,
                    DateEnd = recPC.DateEnd
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

                    if (element.TravelStatus == TravelStatus.Принят || element.TravelStatus == TravelStatus.Зарезервирован)
                    {
                        var travelTours = context.TourTravels.Where(rec => rec.TravelId == element.Id);
                        foreach (var travelTour in travelTours)
                        {
                            var tourReservations = context.TourReservations.Where(rec => rec.TourId == travelTour.TourId);
                            foreach (var tourReservation in tourReservations)
                            {
                                if (element.TravelStatus == TravelStatus.Принят)
                                {
                                    var countReservations = context.Reservations.FirstOrDefault(reservation => reservation.Id == tourReservation.ReservationId).Number;
                                    if (tourReservation.NumberReservations > countReservations)
                                    {
                                        throw new Exception("Недостаточно броней");
                                    }
                                    else
                                    {
                                        tourReservation.Reservation.Number -= tourReservation.NumberReservations;
                                        context.SaveChanges();
                                        break;
                                    }
                                }
                                if (element.TravelStatus != TravelStatus.Зарезервирован) continue;
                                {
                                    int countReservations = tourReservation.Reservation.NumberReserve;

                                    if (tourReservation.NumberReservations > countReservations)
                                    {
                                        throw new Exception("Недостаточно броней");
                                    }
                                    else
                                    {
                                        tourReservation.Reservation.Number -= tourReservation.NumberReservations;
                                        context.SaveChanges();
                                        break;
                                    }
                                }
                            }                         
                        }
                        element.DateImplement = DateTime.Now;
                        element.TravelStatus = TravelStatus.Выполняется;
                        context.SaveChanges();
                        transaction.Commit();

                    }                  
                    else
                    {
                        throw new Exception("Заказ не в статусе \"Принят\" или \"Зарезервирован\"");
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Reservation (TravelBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var element = new Travel
                    {
                        ClientId = model.ClientId,
                        DateCreate = DateTime.Now,
                        TotalCost = model.TotalCost,
                        TravelStatus = TravelStatus.Зарезервирован,
                    };

                    context.Travels.Add(element);
                    context.SaveChanges();

                    var groupTours = model.TourTravels
                        .GroupBy(rec => rec.TourId)
                        .Select(rec => new { TourId = rec.Key, Count = rec.Sum(r => r.Count) });

                    foreach (var groupTour in groupTours)
                    {
                        var travelTour = new TourTravel
                        {
                            TravelId = element.Id,
                            TourId = groupTour.TourId,
                            Count = groupTour.Count
                        };

                        context.TourTravels.Add(travelTour);

                        var tourReservation = context.TourReservations.FirstOrDefault(rec => rec.TourId == travelTour.TourId);

                        var detail = context.Reservations.FirstOrDefault(rec => rec.Id == tourReservation.ReservationId);

                        var reserveReservations = tourReservation.NumberReservations;

                        var check = detail.Number - reserveReservations;

                        if (check >= 0)
                        {
                            detail.NumberReserve += reserveReservations;
                        }
                        else
                        {
                            throw new Exception("Недостаточно броней для резервации");
                        }

                        context.SaveChanges();
                    }
                    transaction.Commit();

                    var client = context.Clients.FirstOrDefault(x => x.Id == model.ClientId);

                    SendEmail(client?.Email, "Оповещение по путешествиям",
                        $"Путешествие №{element.Id} от {element.DateCreate.ToShortDateString()} зарезервировано успешно");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            
            }
            
        }

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;
            try
            {
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = Encoding.UTF8;
                objMailMessage.BodyEncoding = Encoding.UTF8;
                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]);
                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }

        public void SaveDataBaseCLient()
        {
            SaveEntity(context.Travels.ToList());
            SaveEntity(context.Clients.ToList());
            SaveEntity(context.TourTravels.ToList());     
        }

        public void SaveDataBaseAdmin()
        {
        }

        public void SaveEntity(IEnumerable entity)
        {
            var jsonFormatter = new DataContractJsonSerializer(entity.GetType());

            using (var fs = new FileStream($"backup/{GetNameEntity(entity)}.json",
                FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, entity);
            }
        }

        private static string GetNameEntity(IEnumerable entity)
        {
            return entity.AsQueryable().ElementType.ToString().Split('.')[1];
        }
    }
}
