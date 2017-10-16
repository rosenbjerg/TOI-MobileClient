using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TOI_MobileClient;

namespace TOI_MobileClient.Test
{
    class MockHttpClient : IToiHttpClient
    {
        private Dictionary<string, string> mockUrlContent = new Dictionary<string, string>()
        {
            {"tags/valid", "{'key': 'value'}" },
            {"tags/badformat", "This is a bad json string!" },
            {"tags/invalid", null }
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
    }
}
