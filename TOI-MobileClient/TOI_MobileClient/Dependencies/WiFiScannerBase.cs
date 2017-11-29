using System.Collections.Generic;
using System.Threading.Tasks;

namespace TOI_MobileClient.Dependencies
{
    public abstract class WiFiScannerBase : IHardware
    {
        public abstract Task<IEnumerable<string>> ScanWifi();

        public bool IsEnabled => false;

    }
}
