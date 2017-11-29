using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.Widget;
using Android.Locations;
using Android.Runtime;

namespace TOI_MobileClient.Dependencies
{
    public abstract class GpsScannerBase : Java.Lang.Object, IHardware
    {
        public abstract Location GetLocation();
        public abstract Task<Location> GetLocationAsync();

        public bool IsEnabled => false;
    }
}
