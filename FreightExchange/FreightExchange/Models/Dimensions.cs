using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreightExchange.Models
{
    public class Dimensions
    {
        public static string CollectionPath = "dimentions";

        [Id]
        public string Id { get; set; }

        [MapTo("lenght")]
        public double Lenght { get; set; }

        [MapTo("width")]
        public double Width { get; set; }

        [MapTo("height")]
        public double Height { get; set; }
    }
}
