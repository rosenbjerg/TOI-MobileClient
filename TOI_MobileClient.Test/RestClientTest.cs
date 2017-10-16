using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOIClasses;

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
            var tagInfo = _rc.Get<TagInfo>("tags/valid");

            Assert.IsNotNull(tagInfo.Result);
        }

        [TestMethod]
        public void GetTagInfo_ValidTagIdInvalidUrl_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _rc.Get<TagInfo>("sdljkahgf"));
        }

        [TestMethod]
        public void GetTagInfo_InvalidTagIdValidUrl_ReturnsNull()
        {
            var tagInfo = _rc.Get<TagInfo>("tags/invalid");

            Assert.IsNull(tagInfo.Result);
        }

        [TestMethod]
        public void GetTagInfo_InvalidTagIdInvalidUrl_ThrowArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _rc.Get<TagInfo>("sdlhg"));

        }

        [TestMethod]
        public void GetTagInfo_ValidTagIdValidUrlUnexpectedAnswerFormat_ThrowsFormatException()
        {
            Assert.ThrowsException<FormatException>(() => _rc.Get<TagInfo>("tags/badformat"));

        }
    }
}
