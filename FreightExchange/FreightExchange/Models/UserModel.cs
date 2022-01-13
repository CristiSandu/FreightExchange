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

        private string _mainTitle;
        [MapTo("MainTitle")]
        public string MainTitle
        {
            get { return _mainTitle; }
            set
            {
                if (value != _mainTitle)
                {
                    _mainTitle = $"{Name} {SurName}";
                }
            }
        }

        private string _secondTitle;
        [MapTo("SecondTitle")]
        public string SecondTitle
        {
            get { return _secondTitle; }
            set
            {
                if (value != _secondTitle)
                {
                    _secondTitle = $"{Email}";
                }
            }
        }

        private string _endTitle;
        [MapTo("EndTitle")]
        public string EndTitle
        {
            get { return _endTitle; }
            set
            {
                if (value != _endTitle)
                {
                    _endTitle = $"{Role}";
                }
            }
        }

        private string _icon2;
        [MapTo("Icon2")]
        public string Icon2
        {
            get { return _icon2; }
            set
            {
                if (value != _icon2)
                {
                    _icon2 = "envelope-open-text";
                }
            }
        }

    }
}
