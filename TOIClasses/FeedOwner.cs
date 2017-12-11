using System;
using System.Collections.Generic;
using System.Text;

namespace TOIClasses
{
    public class FeedOwner : ModelBase
    {
        public string Email { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
