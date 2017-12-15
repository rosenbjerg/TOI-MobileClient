using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TOIClasses;

namespace TOI_MobileClient.Dependencies
{
    public abstract class CellularScannerBase : IHardware, IScanner<CellularIdFoundEventArgs>
    {
        public bool IsEnabled => true;
        public abstract Task ScanAsync();
        public abstract event EventHandler<CellularIdFoundEventArgs> ResultFound;
    }

    public class CellularIdFoundEventArgs : EventArgs, IScanResultEvent
    {
        public CellularIdFoundEventArgs(string cid, string lac)
        {
            Id = cid;
            LocationCode = lac;
        }

        public string Id { get; }
        public string LocationCode { get; set; }
    }
}
