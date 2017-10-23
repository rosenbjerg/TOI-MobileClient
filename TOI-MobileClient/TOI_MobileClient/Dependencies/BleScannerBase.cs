﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace TOI_MobileClient
{
    public abstract class BleScannerBase
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

        public abstract bool IsEnabled { get; }

        public abstract Task<IReadOnlyList<BleDevice>> ScanDevices(HashSet<Guid> deviceFilter, int scanTimeout = 2000);
        
    }
    public class BleDevice
    {
        public Guid Address { get; set; }
        public int Rssi { get; set; }
    }
}