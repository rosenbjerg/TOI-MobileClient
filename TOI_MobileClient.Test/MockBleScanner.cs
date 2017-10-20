using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TOI_MobileClient.Test
{
    internal class MockBleScanner : BleScannerBase
    {
        public override bool IsEnabled => true;

        public override Task<List<BleDevice>> ScanDevices(HashSet<Guid> deviceFilter, int scanTimeout = 5000)
        {
            return Task.FromResult(new List<BleDevice>
            {
                new BleDevice
                {
                    Address = "CC:14:54:01:52:82",
                    Rssi = -67
                },
                new BleDevice
                {
                    Address = "CC:15:54:01:52:82",
                    Rssi = -64
                },
                new BleDevice
                {
                    Address = "CC:16:54:01:52:82",
                    Rssi = -30
                },
                new BleDevice
                {
                    Address = "CC:17:54:01:52:82",
                    Rssi = -98
                },
            }.Where(s => deviceFilter?.Contains(s.Address) ?? true).ToList());
        }
    }
}