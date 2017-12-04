﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TOI_MobileClient
{
    
    public class RestClient
    {
        private readonly IToiHttpManager _manager;

        public RestClient(IToiHttpManager manager)
        {
            this._manager = manager;
        }

        public async Task<T> Get<T>(string url)
            where T : class, new()
        {
            var jsonString = await _manager.GetAsync(url);
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

        public async Task<IEnumerable<T>> GetMany<T>(string url, Dictionary<string, string> query = null)
            where T : class, new()
        {
            var q = "";
            if (query != null)
            {
                q = "?" + string.Join("&", query.Keys.Select(key => $"{key}={query[key]}"));
            }
            var res = await _manager.GetAsync(url, q);
            if (string.IsNullOrEmpty(res))
                return null;
            var tagList = JsonConvert.DeserializeObject<List<T>>(res);
            return tagList;
        }

        public async Task<IEnumerable<T>> PostMany<T>(string url, IEnumerable<string> ids)
            where T : class, new()
        {
            var jArray = JsonConvert.SerializeObject(ids.ToList());
            var res = await _manager.GetAsync(url, jArray);
            if (string.IsNullOrEmpty(res))
                return null;
            var tagList = JsonConvert.DeserializeObject<List<T>>(res);
            return tagList;

        }
    }
}
