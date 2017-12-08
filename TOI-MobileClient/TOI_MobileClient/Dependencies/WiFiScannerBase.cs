using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TOI_MobileClient.Dependencies
{
    public abstract class WiFiScannerBase : IHardware, IScanner<WifiApFoundEventArg>
    {
        public abstract Task ScanAsync();
        public abstract event EventHandler<WifiApFoundEventArg> ResultFound;
        public bool IsEnabled => false;

        public EventHandler<WifiApFoundEventArg> WifiApFound;
    }

    public class WifiApFoundEventArg : EventArgs, IScanResultEvent
    {
        public string Id { get; }

        public WifiApFoundEventArg(string id)
        {
            Id = id;
        }
    }
}
