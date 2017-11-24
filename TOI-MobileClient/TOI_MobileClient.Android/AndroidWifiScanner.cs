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
        public override async Task<IEnumerable<String>> ScanWifi()
        {
            var scanReceiver = new WifiScanReceiver();
            var wm = (WifiManager)Application.Context.GetSystemService(Context.WifiService);

            if (!wm.IsWifiEnabled)
            {
                DependencyManager.Get<NotifierBase>().DisplayToast(SettingsManager.Language.WifiNotEnabled, true);
                return null;
            }
            Application.Context.RegisterReceiver(scanReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            ((WifiManager) Application.Context.GetSystemService(Context.WifiService)).StartScan();
            var res = await scanReceiver.Task;
            Application.Context.UnregisterReceiver(scanReceiver);
            return res;
        }

    }

    internal class WifiScanReceiver : BroadcastReceiver
    {
        private TaskCompletionSource<IEnumerable<String>> tcs = new TaskCompletionSource<IEnumerable<String>>();
        private static WifiManager _wifiMan;
        public Task<IEnumerable<String>> Task => tcs.Task;

        public override void OnReceive(Context context, Intent intent)
        {
            _wifiMan = (WifiManager) Application.Context.GetSystemService(Context.WifiService);

            var res = _wifiMan.ScanResults.Select(r => r.Bssid);
            tcs.SetResult(res);
        }
    }
}