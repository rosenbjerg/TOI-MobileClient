using System;
using System.Collections.Generic;
using System.Linq;
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
        private const int ScanTimeout = 2000;
        private bool _isScanning;
        
        public override bool IsEnabled => Ble.IsOn && Ble.IsAvailable;

        public override event EventHandler<BleDeviceFoundEventArgs> ResultFound;
        public override async Task ScanAsync()
        {
            if (_isScanning || !SettingsManager.BleEnabled) return;
            if (!IsEnabled)
            {
                DependencyManager.Get<NotifierBase>().DisplayToast(SettingsManager.Language.BluetoothNotEnabled, true);
                return;
            }
            _isScanning = true;

            Adapter.ScanTimeout = ScanTimeout;
            void OnAdapterOnDeviceDiscovered(object sender, DeviceEventArgs args)
            {
                if (!Filter(args.Device)) return;

                var dev = new BleDevice
                {
                    Rssi = args.Device.Rssi,
                    Address = SettingsManager.PrepId(args.Device.Id.ToString("N")).TrimStart('0')
                };
                ResultFound?.Invoke(sender, new BleDeviceFoundEventArgs(dev));
            }

            Adapter.DeviceDiscovered += OnAdapterOnDeviceDiscovered;
            await Adapter.StartScanningForDevicesAsync();
            Adapter.DeviceDiscovered -= OnAdapterOnDeviceDiscovered;

            _isScanning = false;
        }

        private static bool Filter(IDevice dev)
        {
            var devId = SettingsManager.PrepId(dev.Id.ToString("N")).TrimStart('0');
            return SubscriptionManager.Instance.AllTags?.Contains(devId) ?? false;
        }
    }
}