using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TOIClasses;

namespace TOI_MobileClient.Dependencies
{
    public abstract class CellularScannerBase : IHardware, IScanner<LocationFoundEventArgs>
    {
        public bool IsEnabled => true;
        public abstract Task ScanAsync();
        public abstract event EventHandler<LocationFoundEventArgs> ResultFound;
    }

    public class CellularIdFoundEventArgs : EventArgs, IScanResultEvent
    {
        public CellularIdFoundEventArgs(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
