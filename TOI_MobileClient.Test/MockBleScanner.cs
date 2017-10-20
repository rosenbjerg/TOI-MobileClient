using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TOI_MobileClient.Test
{
    internal class MockBleScanner : BleScannerBase
    {
        public override bool IsEnabled => true;

        public override Task<IReadOnlyList<BleDevice>> ScanDevices(HashSet<Guid> deviceFilter, int scanTimeout = 5000)
        {
            var bleList = new List<BleDevice>();
            bleList.Add(new BleDevice
                {
                    Address = Guid.Parse("CC:14:54:01:52:82"),
                    Rssi = -67
                });
            bleList.Add(new BleDevice
            {
                Address = Guid.Parse("CC:15:54:01:52:82"),
                Rssi = -64
            });
            bleList.Add(new BleDevice
            {
                Address = Guid.Parse("CC:16:54:01:52:82"),
                Rssi = -30
            });
            bleList.Add(new BleDevice
                {
                    Address = Guid.Parse("CC:17:54:01:52:82"),
                    Rssi = -98
                }
            );
            
            var fbleList = bleList.Where(s => deviceFilter?.Contains(s.Address) ?? true).ToList();
            
            
            return Task.FromResult((IReadOnlyList<BleDevice>) fbleList.AsReadOnly());
        }
    }
}