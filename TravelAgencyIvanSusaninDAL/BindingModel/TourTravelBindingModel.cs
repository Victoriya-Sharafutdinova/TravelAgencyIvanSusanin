using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace TravelAgencyIvanSusaninDAL.BindingModel
{
    [DataContract]
    public class TourTravelBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int TravelId { get; set; }

        [DataMember]
        public int TourId { get; set; }

        [DataMember]
        public DateTime DateReservation { get; set; }

        [DataMember]
        public DateTime DateBegin { get; set; }

        [DataMember]
        public DateTime DateEnd { get; set; }

        public int Count { get; set; }

    }
}
