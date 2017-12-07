using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.Widget;
using Android.Locations;
using Android.Runtime;
using TOIClasses;

namespace TOI_MobileClient.Dependencies
{
    public abstract class GpsScannerBase : Java.Lang.Object, IHardware
    {
        public abstract Location CurrentLocation { get; protected set; }
        public abstract Location GetLocation();
        public abstract Task<GpsLocation> GetLocationAsync();

        public bool IsEnabled => false;
        public EventHandler<LocationFoundEventArgs> LocationFound { get; set; }
    }

    public class LocationFoundEventArgs : EventArgs
    {
        public GpsLocation Location;

        public LocationFoundEventArgs(GpsLocation location)
        {
            Location = location;
        }
    }
}
