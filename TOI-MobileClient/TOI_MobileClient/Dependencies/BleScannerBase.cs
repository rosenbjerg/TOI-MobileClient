using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace TOI_MobileClient
{
    public abstract class BleScannerBase
    {
        protected BleScannerBase()
        {
            Ble = CrossBluetoothLE.Current;
            Adapter = Ble.Adapter;
        }

        protected bool IsScanning;
        protected IBluetoothLE Ble;
        protected IAdapter Adapter;

        public abstract bool IsEnabled { get; }

        public abstract Task<List<BleDevice>> ScanDevices(HashSet<Guid> deviceFilter, int limit = 10);
        
    }
    public class BleDevice
    {
        public Guid Address { get; set; }
        public int RSSI { get; set; }
    }
}