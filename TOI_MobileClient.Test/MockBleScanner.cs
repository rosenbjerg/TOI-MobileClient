using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TOI_MobileClient.Test
{
    class MockBleScanner : BleScannerBase
    {
        public override Task<List<BleDevice>> ScanDevices(HashSet<string> bdaFilter = null, int limit = 10, int scanTimeout = 10000)
        {
            return Task.FromResult(new List<BleDevice>
            {
                new BleDevice
                {
                    Address = "CC:14:54:01:52:82",
                    RSSI = -67
                },
                new BleDevice
                {
                    Address = "CC:15:54:01:52:82",
                    RSSI = -64
                },
                new BleDevice
                {
                    Address = "CC:16:54:01:52:82",
                    RSSI = -30
                },
                new BleDevice
                {
                    Address = "CC:17:54:01:52:82",
                    RSSI = -98
                },
            }.Where(s => bdaFilter?.Contains(s.Address) ?? true).ToList());
        }
    }
}