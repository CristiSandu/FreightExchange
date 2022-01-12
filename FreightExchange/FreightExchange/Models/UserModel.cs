using System;
using System.Collections.Generic;
using System.Text;

namespace FreightExchange.Models
{
    public class UserModel
    {
        public static string CollectionPath = "users";
        public string Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
