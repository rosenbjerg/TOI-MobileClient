using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Locations;
using Android.Runtime;

namespace TOI_MobileClient.Dependencies
{
    public abstract class GpsLocatorBase : Java.Lang.Object
    {
        public bool IsEnabled
        {
            get;
            protected set;
        }

        public abstract Location GetLocation();


    }
}
