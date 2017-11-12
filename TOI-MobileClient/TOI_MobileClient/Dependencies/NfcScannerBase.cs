using System;
using System.Collections.Generic;
using System.Text;

namespace TOI_MobileClient.Dependencies
{
    public abstract class NfcScannerBase
    {
        public bool IsEnabled => false;
        public abstract Guid ScanNfc();
    }
}
