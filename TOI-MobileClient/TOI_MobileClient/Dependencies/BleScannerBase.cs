﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using TOI_MobileClient.Dependencies;

namespace TOI_MobileClient
{
    public abstract class BleScannerBase : IHardware, IScanner<BleDeviceFoundEventArgs>
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

        public abstract Task ScanAsync();


        public abstract bool IsEnabled { get; }
        
        public abstract event EventHandler<BleDeviceFoundEventArgs> ResultFound;
    }

    public class BleDeviceFoundEventArgs : EventArgs, IScanResultEvent
    {
        public readonly BleDevice Device;

        public BleDeviceFoundEventArgs(BleDevice device)
        {
            Device = device;
        }

        public string Id => Device.Address;
    }

    public class BleDevice
    {
        public string Address { get; set; }
        public int Rssi { get; set; }
    }
}