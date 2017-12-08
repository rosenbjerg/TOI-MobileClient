using Android.Content;
using System;
using System.Threading.Tasks;

namespace TOI_MobileClient.Dependencies
{
    public abstract class NfcScannerBase : IHardware, IScanner<NfcEventArgs>
    {
        public abstract void HandleNfcIntent(Intent intent);
        
        public bool IsEnabled => false;

        public Task ScanAsync()
        {
            // Do nothing
            return null;
        }

        public abstract event EventHandler<NfcEventArgs> ResultFound;
    }

    public class NfcEventArgs : EventArgs, IScanResultEvent
    {
        public NfcEventArgs(string id)
        {
            Id = id;
        }
        
        public string Id { get; }
    }


}
