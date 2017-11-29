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
            _scanner = new WifiScanReceiver();
        }

        public bool IsEnabled => _scanner.IsEnabled;
   
        public override async Task<IEnumerable<string>> ScanWifi()
        {
            if (SettingsManager.WiFiEnabled && !IsEnabled)
            {
                //DependencyManager.Get<NotifierBase>().DisplayToast(SettingsManager.Language.WifiNotEnabled, true);
                return null;
            }
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
        private readonly TaskCompletionSource<IEnumerable<string>> _tcs = new TaskCompletionSource<IEnumerable<string>>();
        private static WifiManager _wifiMan;
        public Task<IEnumerable<string>> Task => _tcs.Task;

        public override void OnReceive(Context context, Intent intent)
        {
            _wifiMan = (WifiManager) Application.Context.GetSystemService(Context.WifiService);

            var res = _wifiMan.ScanResults.Select(r => r.Bssid);
            _tcs.SetResult(res);
        }

        public bool IsEnabled => _wifiMan.IsWifiEnabled;

        public bool StartScan()
        {
            return _wifiMan.StartScan();
        }
    }
}