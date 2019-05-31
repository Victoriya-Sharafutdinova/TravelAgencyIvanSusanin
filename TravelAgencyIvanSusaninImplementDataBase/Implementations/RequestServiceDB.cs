using System;
using System.Collections.Generic;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninModel;
using System.Linq;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class RequestServiceDB : IRequestService
    {
        private AbstractDbContext context;

        public RequestServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public void CreateRequest(ReservationRequestBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var reservation = context.Reservations.FirstOrDefault(rec => rec.Id == model.ReservationId);
                    reservation.Number += model.NumberReservation;
                    context.SaveChanges();

                    Request request = new Request
                    {
                        DateCreate = DateTime.Now
                    };
                    context.Requests.Add(request);
                    context.SaveChanges();

                    context.ReservationRequests.Add(new ReservationRequest
                    {
                        RequestId = request.Id,
                        ReservationId = model.ReservationId,
                        NumberReservation = model.NumberReservation,
                        Request = request,
                        Reservation = reservation
                    });
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

        public List<RequestViewModel> GetList()
        {
            throw new NotImplementedException();
        }

        public void Request(RequestBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
