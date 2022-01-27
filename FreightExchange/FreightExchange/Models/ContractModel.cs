using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreightExchange.Models
{
    public class ContractModel
    {
        public static string CollectionPath = "contracts";

        [Id]
        public string Id { get; set; }

        [MapTo("transporter")]
        public UserModel Transporter { get; set; }

        [MapTo("sender")]
        public UserModel Sender { get; set; }

        [MapTo("transporter_id")]
        public string Transporter_Id { get; set; }

        [MapTo("sender_id")]
        public string Sender_Id { get; set; }

        [MapTo("startPlace")]
        public string StartPlace{ get; set; }

        [MapTo("endPlace")]
        public string EndPlace { get; set; }

        [MapTo("price")]
        public string Price { get; set; }

        [MapTo("freightDetailes")]
        public string FreightDetailes { get; set; }

        [MapTo("truckDetailes")]
        public string TruckDetailes { get; set; }

    }
}
