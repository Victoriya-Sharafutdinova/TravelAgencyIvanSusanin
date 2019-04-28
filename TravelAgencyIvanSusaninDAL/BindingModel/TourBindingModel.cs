using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninDAL.BindingModel
{
    [DataContract]
    public class TourBindingModel
    {
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Cost { get; set; }
    }
}
