using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninModel
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime DateCreate { get; set; }

        [ForeignKey("RequestId")]
        public virtual List<ReservationRequest> ReservationRequests { get; set; }
    }
}
