using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TOI_MobileClient
{
    
    public class RestClient
    {
        IToiHttpClient _client;

        public RestClient(IToiHttpClient client)
        {
            this._client = client;
        }
    }
}
