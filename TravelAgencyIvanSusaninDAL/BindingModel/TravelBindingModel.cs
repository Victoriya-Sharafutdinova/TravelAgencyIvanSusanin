using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace TravelAgencyIvanSusaninDAL.BindingModel
{
    [DataContract]
    public class TravelBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int TotalCost { get; set; }

        public virtual List<TourTravelBindingModel> TourTravels { get; set; }
    }
}
