using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TOI_MobileClient.Test
{
    [TestClass]
    public class RestClientTest
    {
        private RestClient _rc;

        [TestInitialize]
        public void SetupRestClient()
        {
            _rc = new RestClient(new MockHttpClient());
        }

        [TestMethod]
        public void GetTagInfo_ValidTagIdValidUrl_ReturnsTagObject()
        {
            Assert.Fail("Not implemented yet.");
        }

        [TestMethod]
        public void GetTagInfo_ValidTagIdInvalidUrl_ThrowsArgumentException()
        {
            Assert.Fail("Not implemented yet.");
        }

        [TestMethod]
        public void GetTagInfo_InvalidTagIdValidUrl_ReturnsNull()
        {
            Assert.Fail("Not implemented yet.");
        }

        [TestMethod]
        public void GetTagInfo_InvalidTagIdInvalidUrl_ThrowArgumentException()
        {
            Assert.Fail("Not implemented yet.");

        }

        [TestMethod]
        public void GetTagInfo_ValidTagIdValidUrlUnexpectedAnswerFormat_ThrowsFormatException()
        {
            Assert.Fail("Not implemented yet.");

        }
    }
}
