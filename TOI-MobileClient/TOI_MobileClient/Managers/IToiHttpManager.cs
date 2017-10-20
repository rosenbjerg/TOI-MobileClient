using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TOI_MobileClient
{
    public interface IToiHttpManager : IDisposable
    {
        Task<string> GetStringAsync(string url);
        Task<string> PostAsync(string url, string body, bool isJson = true);
    }
}
