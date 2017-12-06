using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TOI_MobileClient.Dependencies
{
    public abstract class WiFiScannerBase : IHardware
    {
        public abstract Task<IEnumerable<string>> ScanWifi(HashSet<string> deviceFilter = null);

        public bool IsEnabled => false;

        public EventHandler<WifiApFoundEventArg> WifiApFound;
    }

    public class WifiApFoundEventArg : EventArgs
    {
        public string Bssid { get; }

        public WifiApFoundEventArg(string bssid)
        {
            Bssid = bssid;
        }
    }
}
