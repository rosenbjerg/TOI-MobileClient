using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TOI_MobileClient.Dependencies;
using Android.Net.Wifi;
using DepMan;
using TOI_MobileClient.Managers;

namespace TOI_MobileClient.Droid
{
    public class AndroidWifiScanner : WiFiScannerBase
    {
        private readonly WifiScanReceiver _scanner;

        public AndroidWifiScanner()
        {
            _scanner = new WifiScanReceiver(this);
        }

        public new bool IsEnabled =>
            ((WifiManager) Application.Context.GetSystemService(Context.WifiService)).IsWifiEnabled;


        public override async Task<IEnumerable<string>> ScanWifi(HashSet<string> deviceFilter = null)
        {
            if (!SettingsManager.WiFiEnabled) return null;
            if (!IsEnabled) return null;

            // if deviceFilter is null, use ToiFilter from SettingsManager, unless the ToiFilter is empty
            var filter = deviceFilter ?? (SettingsManager.ToiFilter?.Count == 0 ? null : SettingsManager.ToiFilter);
            var scanner = new WifiScanReceiver(this) {BssidFilter = filter};

            Application.Context.RegisterReceiver(scanner,
                new IntentFilter(WifiManager.ScanResultsAvailableAction));

            scanner.StartScan();
            var res = await scanner.Task;
            Application.Context.UnregisterReceiver(scanner);

            return res;
        }
    }

    internal class WifiScanReceiver : BroadcastReceiver
    {
        private readonly AndroidWifiScanner _androidWifi;
        private readonly TaskCompletionSource<List<string>> _tcs = new TaskCompletionSource<List<string>>();
        private static WifiManager _wifiMan;
        public Task<List<string>> Task => _tcs.Task;
        public HashSet<string> BssidFilter { get; set; }

        public WifiScanReceiver(AndroidWifiScanner androidWifi)
        {
            _androidWifi = androidWifi;
            _wifiMan = (WifiManager) Application.Context.GetSystemService(Context.WifiService);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var res = _wifiMan.ScanResults
                .Where(r => BssidFilter?.Contains(SettingsManager.PrepId(r.Bssid)) ?? true)
                .Select(r =>
                {
                    Console.WriteLine($"AP: {r.Ssid} + {r.Bssid}");
                    _androidWifi.WifiApFound?.Invoke(this, new WifiApFoundEventArg(r.Bssid));
                    return r.Bssid;
                });
            _tcs.SetResult(res.ToList());
        }

        public bool IsEnabled => _wifiMan.IsWifiEnabled;

        public bool StartScan()
        {
            return _wifiMan.StartScan();
        }
    }
}