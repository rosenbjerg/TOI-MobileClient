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

        public new bool IsEnabled => _scanner.IsEnabled;
   
        public override async Task<IEnumerable<string>> ScanWifi(HashSet<string> filter = null)
        {
            if (!SettingsManager.WiFiEnabled) return null;
            if (!IsEnabled) return null;

            _scanner.BssidFilter = filter;
            
            Application.Context.RegisterReceiver(_scanner,
                new IntentFilter(WifiManager.ScanResultsAvailableAction));

            _scanner.StartScan();
            var res = await _scanner.Task;
            Application.Context.UnregisterReceiver(_scanner);

            return res;
        }

    }

    internal class WifiScanReceiver : BroadcastReceiver
    {
        private readonly AndroidWifiScanner _androidWifi;
        private readonly TaskCompletionSource<IEnumerable<string>> _tcs = new TaskCompletionSource<IEnumerable<string>>();
        private static WifiManager _wifiMan;
        public Task<IEnumerable<string>> Task => _tcs.Task;
        public HashSet<string> BssidFilter { get; set; }

        public WifiScanReceiver(AndroidWifiScanner androidWifi)
        {
            _androidWifi = androidWifi;
            _wifiMan = (WifiManager) Application.Context.GetSystemService(Context.WifiService);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var res = _wifiMan.ScanResults
                .Where(r => BssidFilter?.Contains(r.Bssid.ToUpperInvariant()) ?? true)
                .Select(r =>
                {
//                    Console.WriteLine($"{r.Ssid} + {r.Bssid}");
                    _androidWifi.WifiApFound?.Invoke(this, new WifiApFoundEventArg(r.Bssid));
                    return r.Bssid;
                });
            Console.WriteLine("WIFI_SCANNING");
            _tcs.SetResult(res.ToList());
        }
        
        public bool IsEnabled => _wifiMan.IsWifiEnabled;

        public bool StartScan()
        {
            _wifiMan.ScanResults.Clear();
            return _wifiMan.StartScan();
        }
    }

}
 