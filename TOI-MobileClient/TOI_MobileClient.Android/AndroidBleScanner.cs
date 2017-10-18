using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Bluetooth;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Xamarin.Forms;

namespace TOI_MobileClient.Droid
{
    class AndroidBleScanner : BleScannerBase
    {
        private bool _isScanning;

        public override bool IsEnabled => Ble.IsOn && Ble.IsAvailable;

        public override async Task<List<BleDevice>> ScanDevices(HashSet<Guid> deviceFilter, int scanTimeout = 5000)
        {
            if (_isScanning) return null;
            _isScanning = true;
            
            Adapter.ScanTimeout = scanTimeout;
            var deviceList = new List<BleDevice>();

            void ScanHandler(object s, DeviceEventArgs a)
            {
                var dev = a.Device.NativeDevice as BluetoothDevice;
                deviceList.Add(new BleDevice
                {
                    RSSI = a.Device.Rssi,
                    Address = dev.Address,
                });
            }

            Adapter.DeviceDiscovered += ScanHandler;
            await Adapter.StartScanningForDevicesAsync(null, device => deviceFilter.Contains(device.Id));
            Adapter.DeviceDisconnected -= ScanHandler;
            _isScanning = false;
            return deviceList;
        }
    }
}