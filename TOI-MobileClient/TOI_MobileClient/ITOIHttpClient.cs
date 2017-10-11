using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TOI_MobileClient
{
    interface ITOIHttpClient
    {
        Task<string> GetStringAsync(string url);
    }
}
