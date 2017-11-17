using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Bluetooth;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using Xamarin.Forms;

namespace TOI_MobileClient.Droid
{
    public class AndroidBleScanner : BleScannerBase
    {
        private bool _isScanning;


        public override bool IsEnabled => Ble.IsOn && Ble.IsAvailable;
        private readonly IReadOnlyList<BleDevice> _emptyListCache = new List<BleDevice>();

        public override async Task<IReadOnlyList<BleDevice>> ScanDevices(HashSet<string> deviceFilter, int scanTimeout = 2000)
        {
            if (_isScanning || !SettingsManager.BleEnabled) return _emptyListCache;
            if (!IsEnabled)
            {
                DependencyManager.Get<NotifierBase>().DisplayToast(SettingsManager.Language.BluetoothNotEnabled, true);
                return _emptyListCache;
            }
            _isScanning = true;
            
            Adapter.ScanTimeout = scanTimeout;
            var deviceList = new List<BleDevice>();

            void ScanHandler(object s, DeviceEventArgs a)
            {
                deviceList.Add(new BleDevice
                {
                    Rssi = a.Device.Rssi,
                    Address = a.Device.Id.ToString("N")
                });
            }

            Adapter.DeviceDiscovered += ScanHandler;
            await Adapter.StartScanningForDevicesAsync(null, device => deviceFilter?.Contains(device.Id.ToString("N")) ?? true);
            Adapter.DeviceDisconnected -= ScanHandler;
            _isScanning = false;
            return deviceList;
        }
    }
}