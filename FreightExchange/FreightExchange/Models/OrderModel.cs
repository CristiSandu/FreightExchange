using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreightExchange.Models
{
    public class OrderModel
    {
        public static string CollectionPath = "order";

        [Id]
        [MapTo("id")]
        public string Id { get; set; }

        [MapTo("client_id")]
        public string IdClient{ get; set; }

        [MapTo("StartPlace")]
        public string StartPlace { get; set; }

        [MapTo("StartDate")]
        public DateTime StartDate { get; set; }

        [MapTo("EndPlace")]
        public string EndPlace { get; set; }

        [MapTo("EndDate")]
        public DateTime EndDate { get; set; }

        [MapTo("MaxStartDate")]
        public DateTime MaxStartDate { get; set; }

        [MapTo("MaxEndDate")]
        public DateTime MaxEndDate { get; set; }

        [MapTo("MerchType")]
        public string MerchType { get; set; }

        [MapTo("Weight")]
        public double Weight { get; set; }
       
        [MapTo("Volume")]
        public double Volume { get; set; }

        [MapTo("PriceRange")]
        public double PriceRange { get; set; }

        [MapTo("Phone")]
        public string Phone { get; set; }

        [MapTo("Email")]
        public string Email { get; set; }

        [MapTo("IsAssigned")]
        public bool IsAssigned { get; set; }

        [MapTo("IdTransport")]
        public string IdTransport { get; set; }
    }
}
