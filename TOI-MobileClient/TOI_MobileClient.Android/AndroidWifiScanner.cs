using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TOI_MobileClient.Dependencies;
using Android.Net.Wifi;
using DepMan;
using TOI_MobileClient.Managers;
using Xamarin.Forms.Internals;

namespace TOI_MobileClient.Droid
{
    public class AndroidWifiScanner : WiFiScannerBase
    {
        public AndroidWifiScanner()
        {
        }

        public new bool IsEnabled =>
            ((WifiManager) Application.Context.GetSystemService(Context.WifiService)).IsWifiEnabled;


        public override async Task ScanAsync()
        {
            if (!SettingsManager.WiFiEnabled || !IsEnabled) return;
            
            // if deviceFilter is null, use ToiFilter from SettingsManager, unless the ToiFilter is empty
            var scanner = new WifiScanReceiver();
            scanner.ResultFound += (s,e) =>
            {
                ResultFound?.Invoke(this, e);
            };

            Application.Context.RegisterReceiver(scanner,
                new IntentFilter(WifiManager.ScanResultsAvailableAction));
            await scanner.StartScan();
            Application.Context.UnregisterReceiver(scanner);
        }

        public override event EventHandler<WifiApFoundEventArg> ResultFound;
    }

    internal class WifiScanReceiver : BroadcastReceiver
    {
        private readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();
        private static WifiManager _wifiMan;

        internal event EventHandler<WifiApFoundEventArg> ResultFound;

        public WifiScanReceiver()
        {
            _wifiMan = (WifiManager) Application.Context.GetSystemService(Context.WifiService);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            _wifiMan.ScanResults
                .Select(ntw => SettingsManager.PrepId(ntw.Bssid))
                .Where(SubscriptionManager.Instance.AllTags.Contains)
                .ForEach(bssid => ResultFound?.Invoke(this, new WifiApFoundEventArg(bssid)));

            _tcs.SetResult(true);
        }

        public bool IsEnabled => _wifiMan.IsWifiEnabled;

        public Task StartScan()
        {
            _wifiMan.StartScan();
            return _tcs.Task;
        }

    }
}