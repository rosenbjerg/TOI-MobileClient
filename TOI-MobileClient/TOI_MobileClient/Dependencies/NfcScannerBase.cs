using Android.Content;
using System;

namespace TOI_MobileClient.Dependencies
{
    public abstract class NfcScannerBase
    {
        public bool IsEnabled => false;
        public abstract Guid ScanNfc(Intent intent);
    }
}
