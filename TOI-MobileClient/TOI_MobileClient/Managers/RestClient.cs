using System;
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

        public async Task<IEnumerable<T>> GetMany<T>(string url, IEnumerable<string> ids = null)
            where T : class, new()
        {
            var res = "";
            if (ids != null)
            {
                var jArray = JsonConvert.SerializeObject(ids.ToList());
                res = await _manager.PostAsync(url, jArray);
            }
            else
            {
                res = await _manager.GetAsync(url);
            }
            if (string.IsNullOrEmpty(res))
                return null;
            var tagList = JsonConvert.DeserializeObject<List<T>>(res);
            return tagList;
        }
    }
}
