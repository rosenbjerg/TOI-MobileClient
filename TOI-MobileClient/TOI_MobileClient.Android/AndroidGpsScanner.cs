using System;
using System.Threading.Tasks;
using TOI_MobileClient.Dependencies;
using Android.Locations;
using Android.Gms.Location;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using TOI_MobileClient.Managers;
using Xamarin.Forms;


namespace TOI_MobileClient.Droid
{
    public class AndroidGpsScanner :
        GpsScannerBase
    {
        private readonly FusedLocationProviderClient _client;
        private readonly GpsLocationCallback _locationCallback;
        private readonly SettingsClient _settingsClient;
        private readonly LocationRequest _locationRequest;
        private readonly LocationSettingsRequest _locationSettingsRequest;

        private bool _running;
        private TaskCompletionSource<bool> _tcs;
        private LocationSettingsResponse _ready;

        public override bool IsEnabled => true; //tænk lige på noget smart

        void OnLocationResult(object sender, Location location)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await _client.RemoveLocationUpdatesAsync(_locationCallback);
            });
            ResultFound?.Invoke(this, new LocationFoundEventArgs(new GpsLocation()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            }));
            _tcs.SetResult(true);
        }
        
        public override async Task ScanAsync()
        {
            if (_running)
                return;
            _tcs = new TaskCompletionSource<bool>();
            _locationCallback.LocationUpdated += OnLocationResult;
            _running = true;

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    _ready = await _settingsClient.CheckLocationSettingsAsync(_locationSettingsRequest);
                    if (_ready.LocationSettingsStates.IsGpsPresent && _ready.LocationSettingsStates.IsGpsUsable)
                    {
                        await _client.RequestLocationUpdatesAsync(_locationRequest, _locationCallback);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Whoops... Something went wrong :(");
                    _tcs.SetException(ex);
                }
            });
            await _tcs.Task;
            _locationCallback.LocationUpdated -= OnLocationResult;
            _running = false;
        }

        public override event EventHandler<LocationFoundEventArgs> ResultFound;

        public AndroidGpsScanner()
        {
            var activity = Android.App.Application.Context;
            _client = LocationServices.GetFusedLocationProviderClient(activity);
            _settingsClient = LocationServices.GetSettingsClient(activity);

            _locationRequest = new LocationRequest()
                .SetInterval(10000)
                .SetFastestInterval(5000)
                .SetPriority(LocationRequest.PriorityHighAccuracy);

            _locationSettingsRequest = new LocationSettingsRequest.Builder().AddLocationRequest(_locationRequest).Build();


            _locationCallback = new GpsLocationCallback();
        }
    }
    class GpsLocationCallback : LocationCallback
    {
        public event EventHandler<Location> LocationUpdated;

        public override void OnLocationResult(LocationResult result)
        {
            base.OnLocationResult(result);
            LocationUpdated?.Invoke(this, result.LastLocation);
        }
    }
}