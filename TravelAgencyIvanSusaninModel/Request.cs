using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyIvanSusaninModel
{
    public class Request
    {
        public int Id { get; set; }

        public DateTime DateCreate { get; set; }

        [ForeignKey("RequestId")]
        public virtual List<ReservationRequest> ReservationRequests { get; set; }
    }
}
