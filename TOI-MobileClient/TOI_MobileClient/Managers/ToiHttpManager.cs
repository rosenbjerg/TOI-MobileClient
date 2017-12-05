using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TOI_MobileClient
{
    class ToiHttpManager : IToiHttpManager
    {
        private readonly HttpClient _client;

        public ToiHttpManager()
        {
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<string> GetAsync(string url, string query = "")
        {
            try
            {
                var response = await _client.GetAsync(url + query);
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

        public async Task<string> PostAsync(string url, string body, bool isJson = true)
        {
            try
            {
                var cont = new StringContent(body, Encoding.UTF8, isJson ? "application/json" : "text/plain");
                var response = await _client.PostAsync(url, cont);
                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadAsStringAsync();
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}