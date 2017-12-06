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

        public new bool IsEnabled => Ble.IsOn && Ble.IsAvailable;

        private readonly IReadOnlyList<BleDevice> _emptyListCache = new List<BleDevice>();

        public override async Task<IReadOnlyList<BleDevice>> ScanBle(HashSet<string> deviceFilter = null,
            int scanTimeout = 2000)
        {
            if (_isScanning || !SettingsManager.BleEnabled) return _emptyListCache;
            if (!IsEnabled)
            {
                DependencyManager.Get<NotifierBase>().DisplayToast(SettingsManager.Language.BluetoothNotEnabled, true);
                return _emptyListCache;
            }
            _isScanning = true;

            // if deviceFilter is null, use ToiFilter from SettingsManager, unless the ToiFilter is empty
            var filter = deviceFilter ?? (SettingsManager.ToiFilter?.Count == 0 ? null : SettingsManager.ToiFilter);

            Adapter.ScanTimeout = scanTimeout;

            var deviceList = new List<BleDevice>();

            void OnAdapterOnDeviceDiscovered(object sender, DeviceEventArgs args)
            {
                var dev = new BleDevice
                {
                    Rssi = args.Device.Rssi,
                    Address = SettingsManager.PrepId(args.Device.Id.ToString("N")).TrimStart('0')
                };
                BleDeviceFound?.Invoke(sender, new BleDeviceFoundEventArgs(dev));
                deviceList.Add(dev);
            }

            Adapter.DeviceDiscovered += OnAdapterOnDeviceDiscovered;

            await Adapter.StartScanningForDevicesAsync(null, device =>
            {
                var devId = SettingsManager.PrepId(device.Id.ToString("N")).TrimStart('0');
                return filter == null || filter.Contains(devId);
            });
            Adapter.DeviceDiscovered -= OnAdapterOnDeviceDiscovered;

            _isScanning = false;
            return deviceList;
        }
    }
}