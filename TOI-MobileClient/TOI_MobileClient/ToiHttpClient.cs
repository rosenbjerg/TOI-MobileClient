using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TOI_MobileClient
{
    class ToiHttpClient : IToiHttpClient
    {
        private readonly HttpClient _client;

        public ToiHttpClient()
        {
            _client = new HttpClient();
        }
        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<string> GetStringAsync(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new ArgumentException($"The request to {url} did not succeed.");

                return await response.Content.ReadAsStringAsync();
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
