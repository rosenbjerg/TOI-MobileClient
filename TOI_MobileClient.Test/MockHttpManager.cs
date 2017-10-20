using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TOIClasses;

namespace TOI_MobileClient.Test
{
    class MockHttpManager : IToiHttpManager
    {
        public static Guid ValidGuid1 = Guid.NewGuid();
        public static Guid ValidGuid2 = Guid.NewGuid();
        public static Guid ValidGuid3 = Guid.NewGuid();
        public static Guid InvalidGuid = Guid.NewGuid();

        private Dictionary<string, string> mockUrlContent = new Dictionary<string, string>()
        {
            {"tags/valid", "{'key': 'value'}" },
            {"tags/badformat", "This is a bad json string!" },
            {"tags/invalid", null }
        };

        private Dictionary<string, TagInfo> tags = new Dictionary<string, TagInfo>()
        {
            { ValidGuid1.ToString(), new TOIClasses.TagInfo() {Title="1", Description = "Mock tag", Image = "", Url = "mock.dk"}}, 
            { ValidGuid2.ToString(), new TOIClasses.TagInfo() {Title="2", Description = "Mock tag", Image = "", Url = "mock.dk"}},
            { ValidGuid3.ToString(), new TOIClasses.TagInfo() {Title="3", Description = "Mock tag", Image = "", Url = "mock.dk"}}
        };

        public void Dispose()
        {

        }

        public Task<string> GetStringAsync(string url)
        {
            if (!mockUrlContent.ContainsKey(url))
                throw new ArgumentException("This url does not exist in the mock set.");

            return Task.FromResult(mockUrlContent[url]);
        }

        public Task<string> PostAsync(string url, string body, bool isJson = true)
        {
            if(!mockUrlContent.ContainsKey(url))
                throw new ArgumentException("This url does not exist in the mock set.");

            var ids = JsonConvert.DeserializeObject<List<string>>(body);
            var strTags = new List<TagInfo>();

            foreach (var id in ids)
            {
                if(tags.ContainsKey(id))
                    strTags.Add(tags[id]);
                else
                    strTags.Add(null);
            }

            return Task.FromResult(JsonConvert.SerializeObject(strTags));
        }
    }
}
