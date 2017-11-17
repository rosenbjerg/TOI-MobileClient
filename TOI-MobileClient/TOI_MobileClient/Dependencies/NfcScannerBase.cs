using Android.Content;
using System;

namespace TOI_MobileClient.Dependencies
{
    public abstract class NfcScannerBase
    {
        public bool IsEnabled => false;
        public abstract void HandleNfcIntent(Intent intent);
        public EventHandler<NfcEventArgs> NfcTagFound;
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
