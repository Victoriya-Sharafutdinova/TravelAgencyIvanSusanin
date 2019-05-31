﻿using System.Collections.Generic;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.ViewModel;

namespace TravelAgencyIvanSusaninDAL.Interfaces
{
    public interface IRequestService
    {
        List<RequestViewModel> GetList();

        void CreateRequest(ReservationRequestBindingModel model);

        void Request(RequestBindingModel model);
    }
}
