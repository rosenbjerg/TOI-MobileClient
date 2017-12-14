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
    public abstract class GpsScannerBase : Java.Lang.Object, IHardware, IScanner<LocationFoundEventArgs>
    {
        public abstract Location CurrentLocation { get; protected set; }

        public abstract bool IsEnabled { get; }

        public abstract Task ScanAsync();
        public abstract event EventHandler<LocationFoundEventArgs> ResultFound;
    }

    public class LocationFoundEventArgs : EventArgs, IScanResultEvent
    {
        public LocationModel Location;

        public LocationFoundEventArgs(LocationModel location)
        {
            Location = location;
        }

        public string Id => "G" + Location.Latitude + "P" + Location.Longitude + "S";
    }
}
