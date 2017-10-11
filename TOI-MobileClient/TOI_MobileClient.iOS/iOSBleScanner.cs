using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBluetooth;

namespace TOI_MobileClient.iOS
{
    class iOSBleScanner : BleScannerBase
    {
        private bool _isScanning;

        public override async Task<List<BleDevice>> ScanDevices(HashSet<string> bda = null, int limit = 10, int scanTimeout = 10000)
        {
            if (!Ble.IsAvailable || !Ble.IsOn)
                return null;
            if (_isScanning) return null;
            _isScanning = true;

            Adapter.ScanTimeout = scanTimeout; // scan for 10 seconds
            var deviceList = new List<BleDevice>();
            var i = 0;

            Adapter.DeviceDiscovered += (s, a) =>
            {
                var dev = a.Device.NativeDevice as CBPeripheral;
                if (i++ < limit && bda != null && bda.Contains(dev?.UUID.ToString() ?? ""))
                {
                    deviceList.Add(new BleDevice
                    {
                        RSSI = a.Device.Rssi,
                        Address = dev?.UUID.ToString() ?? "",
                    });
                }
                else
                {
                    Adapter.StopScanningForDevicesAsync();
                }
            };

            await Adapter.StartScanningForDevicesAsync();
            _isScanning = false;
            return deviceList;
        }
    }
}