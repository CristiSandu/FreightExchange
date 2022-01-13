using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreightExchange.Models
{
    public class GoodsModel
    {
        public static string CollectionPath = "goods";
        [Id]
        public string Id { get; set; }
        [MapTo("type")]
        public string Name { get; set; }

        public string MainTitle => $"{Name}";
    }
}
