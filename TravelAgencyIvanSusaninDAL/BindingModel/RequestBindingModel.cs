using System;
using System.Collections.Generic;

namespace TravelAgencyIvanSusaninDAL.BindingModel
{
    public class RequestBindingModel
    {
        public int Id { get; set; }

        public DateTime DateCreate { get; set; }

        public List<ReservationRequestBindingModel> RequestReservations { get; set; }
    }
}