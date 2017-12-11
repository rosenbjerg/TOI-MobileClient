using System;
using System.Collections.Generic;
using System.Text;

namespace TOIClasses
{
    public class Feed : LocationModel
    {
        public FeedOwner Owner { get; set; }
        public string BaseUrl { get; set; }
        private string ApiKey { get; set; }
        public bool IsActive { get; set; }
    }
}
