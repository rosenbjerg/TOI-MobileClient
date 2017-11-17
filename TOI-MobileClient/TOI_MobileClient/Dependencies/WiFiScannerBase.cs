using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.Gms.Tasks;

namespace TOI_MobileClient.Dependencies
{
    public abstract class WiFiScannerBase
    {
        public bool IsEnabled => false;
        public abstract Task<IEnumerable<String>> ScanWifi();
    }
}
