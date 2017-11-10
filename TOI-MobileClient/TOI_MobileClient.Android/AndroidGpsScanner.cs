using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Util;
using DepMan;
using Java.Security;
using Java.Util.Logging;
using TOI_MobileClient.Dependencies;

namespace TOI_MobileClient.Droid
{
    class AndroidGpsScanner : GpsLocatorBase, ILocationListener
    {
        private double _latitude;
        private double _longtitude;

        readonly LocationManager _locMan =
            Application.Context.GetSystemService(Context.LocationService) as LocationManager;

        public AndroidGpsScanner()
        {
            
        }
        public AndroidGpsScanner(IntPtr javaReference = default(IntPtr), JniHandleOwnership transfer = default(JniHandleOwnership))
        {
            string Provider = LocationManager.GpsProvider;

            if (_locMan.IsProviderEnabled(Provider))
            {
                IsEnabled = true;
                _locMan.RequestLocationUpdates(Provider, 2000, 1, this);
            }
            else
            {
               IsEnabled = false;
               DependencyManager.Get<NotifierBase>().DisplayToast("Locations is not enabled", true);
            }
        }


        public override Location GetLocation()
        {
            if(IsEnabled)
                return _locMan.GetLastKnownLocation(LocationManager.GpsProvider);
            else
                return null;

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IntPtr Handle { get; }

        public void OnLocationChanged(Location location)
        {
            _latitude = location.Latitude;
            _longtitude = location.Longitude;
        }

        public void OnProviderDisabled(string provider)
        {
            IsEnabled = false;
        }

        public void OnProviderEnabled(string provider)
        {
            IsEnabled = true;
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }
    }
}