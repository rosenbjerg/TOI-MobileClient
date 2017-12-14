using System;
using System.Threading.Tasks;
using TOI_MobileClient.Dependencies;
using Android.Locations;
using Android.Gms.Location;
using Android.App;
using Android.Content;
using TOI_MobileClient.Managers;


namespace TOI_MobileClient.Droid
{
    public class AndroidGpsScanner :
        GpsScannerBase
    {
        private readonly FusedLocationProviderClient _client;
        private readonly LocationManager _locationManager;

        private bool _enabled;
        public override Location CurrentLocation { get; protected set; }

        public override bool IsEnabled => _locationManager.IsProviderEnabled(LocationManager.GpsProvider) && _enabled;

        public override async Task ScanAsync() { }
        public override void Scan()
        {
            var gpsEnabled = (LocationAvailability)_client.LocationAvailability.Result;
            if (!SettingsManager.GpsEnabled || !gpsEnabled.IsLocationAvailable)
                return;

            var location = (Location) _client.LastLocation.Result;
            ResultFound?.Invoke(this, new LocationFoundEventArgs(new GpsLocation()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            }));

        }

        public override event EventHandler<LocationFoundEventArgs> ResultFound;

        public AndroidGpsScanner()
        {
            var activity = Android.App.Application.Context;
            _client = LocationServices.GetFusedLocationProviderClient(activity);
            _locationManager = (LocationManager) Application.Context.GetSystemService(Context.LocationService);
        }
    }
 
}