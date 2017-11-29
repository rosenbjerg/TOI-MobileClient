using System;
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
        public override Location CurrentLocation { get; protected set; }
        private readonly LocationManager _locationManager;
        private bool _enabled;

        public new bool IsEnabled => _locationManager.IsProviderEnabled(LocationManager.GpsProvider) && _enabled;

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

        public override Location GetLocation()
        {
            if (!_client.IsConnected) return null;
            if (!SettingsManager.GpsEnabled) return null;
            if (!IsEnabled) return null;

            var locationRequest = new LocationRequest();
            locationRequest.SetPriority(100);
            locationRequest.SetInterval(10000);
            locationRequest.SetFastestInterval(5000);
            return LocationServices.FusedLocationApi.GetLastLocation(_client);
        }

        public override async Task<Location> GetLocationAsync()
        {
            return await Task.Run(() =>
            {
                var loc = GetLocation();
                LocationFound?.Invoke(this, new LocationFoundEventArgs($"lat:{loc.Latitude}-long:{loc.Longitude}"));
                return loc;
            });
        }

    }
}