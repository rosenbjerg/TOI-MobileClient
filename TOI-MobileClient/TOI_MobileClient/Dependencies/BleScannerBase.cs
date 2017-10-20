using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace TOI_MobileClient
{
    public abstract class BleScannerBase
    {
        protected BleScannerBase(bool init = false)
        {
            if (!init) return;
            Ble = CrossBluetoothLE.Current;
            Adapter = Ble.Adapter;
        }

        protected bool IsScanning;
        protected IBluetoothLE Ble;
        protected IAdapter Adapter;

        public abstract bool IsEnabled { get; }

        public abstract Task<List<BleDevice>> ScanDevices(HashSet<Guid> deviceFilter, int scanTimeout = 5000);
        
    }
    public class BleDevice
    {
        public Guid Address { get; set; }
        public int Rssi { get; set; }
    }
}