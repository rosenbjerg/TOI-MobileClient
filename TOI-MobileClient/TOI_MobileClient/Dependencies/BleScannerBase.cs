using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using TOI_MobileClient.Dependencies;

namespace TOI_MobileClient
{
    public abstract class BleScannerBase : IHardware
    {
        protected BleScannerBase(bool test = false)
        {
            if (test) return;
            Ble = CrossBluetoothLE.Current;
            Adapter = Ble.Adapter;
        }

        protected bool IsScanning;
        protected IBluetoothLE Ble;
        protected IAdapter Adapter;

        public abstract Task<IReadOnlyList<BleDevice>>
            ScanDevices(HashSet<string> deviceFilter, int scanTimeout = 2000);

        public EventHandler<BleEventArgs> DeviceFound;

        public bool IsEnabled => false;
    }

    public class BleEventArgs : EventArgs
    {
        public readonly BleDevice Device;

        public BleEventArgs(BleDevice device)
        {
            Device = device;
        }
    }

    public class BleDevice
    {
        public string Address { get; set; }
        public int Rssi { get; set; }
    }
}