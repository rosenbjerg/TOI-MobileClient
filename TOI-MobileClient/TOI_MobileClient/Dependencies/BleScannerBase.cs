﻿using System;
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
            ScanBle(HashSet<string> deviceFilter = null, int scanTimeout = 2000);

        public EventHandler<BleDeviceFoundEventArgs> BleDeviceFound;

        public bool IsEnabled => false;
    }

    public class BleDeviceFoundEventArgs : EventArgs
    {
        public readonly BleDevice Device;

        public BleDeviceFoundEventArgs(BleDevice device)
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