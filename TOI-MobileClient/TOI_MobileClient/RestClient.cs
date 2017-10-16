﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TOI_MobileClient
{
    
    public class RestClient
    {
        private readonly IToiHttpClient _client;

        public RestClient(IToiHttpClient client)
        {
            this._client = client;
        }

        public async Task<T> Get<T>(string url)
            where T : class, new()
        {
            var jsonString = await _client.GetStringAsync(url);
            if (string.IsNullOrEmpty(jsonString))
                return null;

            try
            {
                T obj = JsonConvert.DeserializeObject<T>(jsonString);
                return obj;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                Console.WriteLine(e);
                throw new FormatException(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
