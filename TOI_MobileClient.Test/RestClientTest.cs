using System;
using System.Collections.Generic;
using System.Linq;
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
            _rc = new RestClient(new MockHttpManager());
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
            CustomAsserts.ThrowsAsync<ArgumentException>(() =>
            {
                var r = _rc.Get<TagInfo>("sdljkahgf");
                r.Wait();
                return r.Result;
            });
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
            CustomAsserts.ThrowsAsync<ArgumentException>(() =>
            {
                var r = _rc.Get<TagInfo>("sdlhg");
                r.Wait();
                return r.Result;
            });

        }

        [TestMethod]
        public void GetTagInfo_ValidTagIdValidUrlUnexpectedAnswerFormat_ThrowsFormatException()
        {
            CustomAsserts.ThrowsAsync<FormatException>(() =>
            {
                var r = _rc.Get<TagInfo>("tags/badformat");
                r.Wait();
                return r.Result;
            });
        }

        [TestMethod]
        public void GetManyTags_ValidTagIdsValidUrl_ListOfTagInfos()
        {
            var ti = _rc.GetMany<TagInfo>("tags/valid", new List<Guid> {MockHttpManager.ValidGuid1, MockHttpManager.ValidGuid2, MockHttpManager.ValidGuid3 });
            ti.Wait();
            Assert.IsInstanceOfType(ti.Result, typeof(List<TagInfo>));
            Assert.AreEqual(ti.Result.ToList().Count, 3);
        }

        [TestMethod]
        public void GetManyTags_ValidTagIdsInvalidUrl_ThrowsArgumentException()
        {
            CustomAsserts.ThrowsAsync<ArgumentException>(() =>
            {
                var ti = _rc.GetMany<TagInfo>("tags/invalid", new List<Guid> { MockHttpManager.ValidGuid1, MockHttpManager.ValidGuid2});
                ti.Wait();
                return ti.Result;
            });
            
        }

        [TestMethod]
        public void GetManyTags_OneInvalidTagIdValidUrl_ListOfTagInfosNullForInvalid()
        {
            var ti = _rc.GetMany<TagInfo>("tags/valid", new List<Guid> { MockHttpManager.ValidGuid1, MockHttpManager.InvalidGuid, MockHttpManager.ValidGuid3 });
            ti.Wait();
            Assert.IsInstanceOfType(ti.Result, typeof(List<TagInfo>));
            Assert.IsNull(ti.Result.ToList()[1]);
            Assert.AreEqual(ti.Result.ToList().Count, 3);
        }

        [TestMethod]
        public void GetManyTags_EmptyTagIdsListValidUrl_ListOfTagInfos()
        {
            var ti = _rc.GetMany<TagInfo>("tags/valid", new List<Guid> ());
            ti.Wait();
            Assert.IsInstanceOfType(ti.Result, typeof(List<TagInfo>));
            Assert.AreEqual(ti.Result.ToList().Count, 0);
        }
    }
}
