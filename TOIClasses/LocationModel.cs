using System;
using System.Collections.Generic;
using System.Text;

namespace TOIClasses
{
    public class LocationModel : ModelBase
    {
        public GpsLocation LocationCenter { get; set; }
        public int Radius { get; set; }

        public LocationModel()
        {
            LocationCenter = new GpsLocation();
        }

        public bool WithinRange(GpsLocation location)
        {
            var a = LocationCenter.Latitude - location.Latitude;
            var b = LocationCenter.Longitude - location.Longitude;
            var dist = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            //Calculate the distance in meter, 111.325 km pr. degree
            var distInM = dist * 111.325 * 1000;

            return distInM <= Radius;
        }
    }
}
