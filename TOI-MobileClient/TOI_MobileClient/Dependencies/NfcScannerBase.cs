using Android.Content;
using System;

namespace TOI_MobileClient.Dependencies
{
    public abstract class NfcScannerBase : IHardware
    {
        public abstract void HandleNfcIntent(Intent intent);
        public EventHandler<NfcEventArgs> NfcTagFound;

        public bool IsEnabled => false;

    }

    public class NfcEventArgs : EventArgs
    {
        public NfcEventArgs(string id)
        {
            TagId = id;
        }

        public string TagId { get; set; }
    }


}
