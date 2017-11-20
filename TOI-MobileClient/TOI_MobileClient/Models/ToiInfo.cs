using System.Collections.Generic;
using TOIClasses;

namespace TOI_MobileClient.Models
{
    public class ToiInfo : TagInfo
    {
        public string Id { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Contexts { get; set; }
    }
}