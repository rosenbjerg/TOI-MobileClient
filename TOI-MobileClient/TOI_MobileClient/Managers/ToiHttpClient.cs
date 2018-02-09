using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TOIClasses;

namespace TOI_MobileClient.Managers
{
    class ToiHttpClient
    {
        private static ToiHttpClient _instance;
        public static ToiHttpClient Instance => _instance ?? (_instance = new ToiHttpClient());

        private ToiHttpClient()
        {
            
        }

        private readonly HttpClient _http = new HttpClient();

        public async Task<TModel> Get<TModel>(string url, Dictionary<string, string> queries = null)
            where TModel : ModelBase
        {
            var res = await _http.GetAsync(url + GetAsQuery(queries));
            if (!res.IsSuccessStatusCode)
                return null;
            try
            {
                var list = JsonConvert.DeserializeObject<TModel>(await res.Content.ReadAsStringAsync());
                return list;
            }
            catch (JsonException e)
            {
                return null;
            }
        }
        public async Task<List<TModel>> GetMany<TModel>(string url, Dictionary<string, string> queries = null)
            where TModel : ModelBase
        {
            var res = await _http.GetAsync(url + GetAsQuery(queries));
            if (!res.IsSuccessStatusCode)
                return null;
            try
            {
                var list = JsonConvert.DeserializeObject<List<TModel>>(await res.Content.ReadAsStringAsync());
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static string GetAsQuery(Dictionary<string, string> queries)
        {
            if (queries != null)
                return "?" + string.Join("&", queries.Keys.Select(key => $"{key}={queries[key]}"));
            return "";
        }
    }
}