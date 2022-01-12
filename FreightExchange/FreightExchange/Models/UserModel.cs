using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreightExchange.Models
{
    public class UserModel
    {
        public static string CollectionPath = "users";
        [Id]
        public string Id { get; set; }
        [MapTo("name")]
        public string Name { get; set; }
        [MapTo("surname")]
        public string SurName { get; set; }
        [MapTo("email")]
        public string Email { get; set; }
        [MapTo("rol")]
        public string Role { get; set; }
    }
}
