using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.OS;
using Android.Gms.Common.Apis;
using TOI_MobileClient.Dependencies;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.App;
using Android.Content;
using Android.Locations;
using DepMan;
using Newtonsoft.Json;
using TOIClasses;
using TOI_MobileClient.Managers;


namespace TOI_MobileClient.Droid
{
    public class AndroidGpsScanner :
        GpsScannerBase,
        GoogleApiClient.IConnectionCallbacks,
        GoogleApiClient.IOnConnectionFailedListener,
        Android.Gms.Location.ILocationListener
    {
        private readonly GoogleApiClient _client;
        private readonly LocationManager _locationManager;
        private bool _enabled;
        public override Location CurrentLocation { get; protected set; }

        public new bool IsEnabled => _locationManager.IsProviderEnabled(LocationManager.GpsProvider) && _enabled;

        public override async Task ScanAsync()
        {
            if (!_client.IsConnected || 
                !SettingsManager.GpsEnabled || 
                !IsEnabled)
                return;

            await Task.Run(() =>
            {
                var locationRequest = new LocationRequest();
                locationRequest.SetPriority(100);
                locationRequest.SetInterval(10000);
                locationRequest.SetFastestInterval(5000);
                var location = LocationServices.FusedLocationApi.GetLastLocation(_client);
                ResultFound?.Invoke(this, new LocationFoundEventArgs(new GpsLocation()
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                }));
            });
        }

        public override event EventHandler<LocationFoundEventArgs> ResultFound;

        public AndroidGpsScanner()
        {
            _client = new GoogleApiClient.Builder(Application.Context, this, this).AddApi(LocationServices.API).Build();
            _client.Connect();
            _locationManager = (LocationManager) Application.Context.GetSystemService(Context.LocationService);
        }

        public void OnConnected(Bundle connectionHint)
        {
            _enabled = true;
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Application.Context);

            if (!GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
                throw new Exception($"Google Play Services hasn't been installed: {queryResult}");

            var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
            throw new Exception(errorString);
        }

        public void OnConnectionSuspended(int cause)
        {
            _enabled = false;
        }

        public void OnLocationChanged(Location location)
        {
            CurrentLocation = location;
        }
    }
}