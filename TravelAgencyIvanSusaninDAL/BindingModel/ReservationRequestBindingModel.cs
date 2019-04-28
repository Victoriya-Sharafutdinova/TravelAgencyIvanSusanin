using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninDAL.BindingModel
{
    [DataContract]
    public class ReservationRequestBindingModel
    {
        public int Id { get; set; }

        [DataMember]
        public int ReservationId { get; set; }

        [DataMember]
        public int RequestId { get; set; }

        [DataMember]
        public int NumberReservation { get; set; }
    }
}
