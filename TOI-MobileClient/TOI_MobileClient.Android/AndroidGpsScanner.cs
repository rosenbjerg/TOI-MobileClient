﻿using System;

using Android.OS;
using Android.Gms.Common.Apis;
using TOI_MobileClient.Dependencies;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.App;
using Android.Locations;


namespace TOI_MobileClient.Droid
{
    class AndroidGpsScanner : GpsLocatorBase, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    {
        GoogleApiClient _client;
        Location _currentLocation;
        public AndroidGpsScanner()
        {
            _client = new GoogleApiClient.Builder(Application.Context, this, this).AddApi(LocationServices.API).Build();
            _client.Connect();
        }

        public override Location GetLocation()
        {
            if (_client.IsConnected)
            {
                LocationRequest locationRequest = new LocationRequest();
                locationRequest.SetPriority(100);
                locationRequest.SetInterval(10000);
                locationRequest.SetFastestInterval(5000);
                
                _currentLocation = LocationServices.FusedLocationApi.GetLastLocation(_client);
                
                return _currentLocation;
            }
            else return null;
        }

        public void OnConnected(Bundle connectionHint)
        {
            Console.WriteLine("connected to google play services");
            IsEnabled = true;
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Application.Context);
            if(queryResult == ConnectionResult.Success)
            {
                //GooglePlayService er installereret så en anden fejl!
            }
            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);

                // error string viser hvad der er galt.
            }
            else
            {
                //Google play services er ikke instaleret
            }
        }

        public void OnConnectionSuspended(int cause)
        {

        }

        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
        }
    }
}