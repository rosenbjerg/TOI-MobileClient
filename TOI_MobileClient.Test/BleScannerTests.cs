﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TOI_MobileClient.Test
{
    [TestClass]
    class BleScannerTests
    {
        [TestMethod]
        public void ScanDevices_NoFilter_FourResults()
        {
            var scanner = new MockBleScanner();

            var devices = scanner.ScanDevices(null);

            Assert.AreEqual(devices.Result.Count, 4);
        }

        [TestMethod]
        public void ScanDevices_SingleAddressFilter_FourResults()
        {
            var scanner = new MockBleScanner();

            var devices = scanner.ScanDevices(new HashSet<Guid> { new Guid ("CC:17:54:01:52:82") });

            Assert.AreEqual(devices.Result.Count, 1);
            Assert.AreEqual(devices.Result.First().Address, "CC:17:54:01:52:82");
        }
    }
}
