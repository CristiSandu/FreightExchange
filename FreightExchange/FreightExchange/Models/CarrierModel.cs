using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreightExchange.Models
{
    public class CarrierModel
    {
        public static string CollectionPath = "carrier";

        [Id]
        [MapTo("id")]
        public string Id { get; set; }

        [MapTo("carrier_id")]
        public string IdCarrier { get; set; }

        [MapTo("StartPlace")]
        public string StartPlace { get; set; }

        [MapTo("StartDate")]
        public DateTime StartDate { get; set; }

        [MapTo("EndPlace")]
        public string EndPlace { get; set; }

        [MapTo("EndDate")]
        public DateTime EndDate { get; set; }

        [MapTo("TruckType")]
        public string TruckType { get; set; }

        [MapTo("Volume")]
        public double Volume { get; set; }

        [MapTo("Dimentions")]
        public Dimensions Dimentions { get; set; }

        [MapTo("Weight")]
        public double Weight { get; set; }

        [MapTo("EnptyPrice")]
        public double EnptyPrice { get; set; }

        [MapTo("FullPrice")]
        public double FullPrice { get; set; }

        [MapTo("Phone")]
        public string Phone { get; set; }

        [MapTo("Email")]
        public string Email { get; set; }

        [MapTo("reservedList")]
        // id comanda 
        public List<string> ReservedList { get; set; }

    }
}
