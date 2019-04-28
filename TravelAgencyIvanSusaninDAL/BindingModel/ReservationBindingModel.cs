using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninDAL.BindingModel
{
    [DataContract]
    public class ReservationBindingModel
    {
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Number { get; set; }
    }
}
