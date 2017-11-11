using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.OS;
using TOI_MobileClient.Dependencies;

namespace TOI_MobileClient.Droid.Services
{
    public class ScannerServiceBinder : Binder
    {
        private readonly ToiScannerService _service;

        public ScannerServiceBinder(ToiScannerService service)
        {
            _service = service;
        }

        public ToiScannerService GetService()
        {
            return _service;
        }   
    }
}
