using System;
using System.Runtime.Serialization;

namespace TravelAgencyIvanSusaninDAL.BindingModel
{
    [DataContract]
    public class RequestBindingModel
    {
        public int Id { get; set; }

        [DataMember]
        public DateTime DateCreate { get; set; }
    }
}